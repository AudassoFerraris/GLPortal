using ClosedXML.Excel;

using GLPortal.Application.DTOs;
using GLPortal.Core.Enums;
using GLPortal.Core.Models;
using GLPortal.Core.Settings;
using GLPortal.Infrastructure.Interfaces;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

using System.Text.RegularExpressions;

namespace GLPortal.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IGitLabService _gitLabService;
    private readonly IMemoryCache _cache;
    private readonly GitLabSettings _gitLabSettings;

    public ProjectService(IGitLabService gitLabService, IMemoryCache cache, IOptions<GitLabSettings> options)
    {
        _gitLabService = gitLabService;
        _cache = cache;
        _gitLabSettings = options.Value;
    }

    public async Task<List<ProjectSummaryDTO>> GetProjectsAsync()
    {
        return await _cache.GetOrCreateAsync("ProjectSummaries", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1); // Cache per 10 minuti
            return await LoadProjectsAsync();
        }) ?? new List<ProjectSummaryDTO>();
    }

    private async Task<List<ProjectSummaryDTO>> LoadProjectsAsync()
    {
        var summaries = new List<ProjectSummaryDTO>();

        foreach (var projectId in _gitLabSettings.ProjectIds)
        {
            var project = await _gitLabService.GetProjectByIdAsync(projectId);
            var openIssues = await _gitLabService.GetIssuesCountAsync(new IssueQueryParameters(projectId)
            {
                ProjectId = projectId,
                State = IssueState.Opened 
                }); 
            var openLastMonth = await _gitLabService.GetIssuesCountAsync(new IssueQueryParameters(projectId)
            {
                ProjectId = projectId,
                CreatedAfter = DateTime.Now.AddMonths(-1)
            });

            summaries.Add(new ProjectSummaryDTO
            {
                Id = project.Id,
                Name = project.Name,
                WebUrl = project.WebUrl,
                OpenIssues = openIssues,
                OpenLastMonth = openLastMonth
            });
        }

        return summaries;
    }

    public async Task<IssuesDTOList> GetIssues(IssueQueryParameters parameters)
    {
        var result = new IssuesDTOList();

        var issues = await _gitLabService.GetIssuesAsync(parameters);

        var priorityRegex = new Regex(_gitLabSettings.PriorityLabelRegex);
        var customerRegex = new Regex(_gitLabSettings.CustomerLabelRegex);

        result.AddRange(issues.Select(i => new IssueDTO(i, customerRegex, priorityRegex)));

        result.TotalCount = issues.TotalCount;

        return result;

    }

    public async Task<byte[]> ExportIssuesToExcelAsync(IssueQueryParameters queryParameters)
    {
        queryParameters.PerPage = -1;
        var project = await _gitLabService.GetProjectByIdAsync(queryParameters.ProjectId);
        var issues = await GetIssues(queryParameters);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Issues");

        // Add headers
        worksheet.Cell(1, 1).Value = "Id";
        worksheet.Cell(1, 2).Value = "Title";
        worksheet.Cell(1, 3).Value = "Created At";
        worksheet.Cell(1, 4).Value = "State";
        worksheet.Cell(1, 5).Value = "Assignees";
        worksheet.Cell(1, 6).Value = "Customers";
        worksheet.Cell(1, 7).Value = "Priority";

        // Add data
        for (int i = 0; i < issues.Count; i++)
        {
            var issue = issues[i];
            worksheet.Cell(i + 2, 1).Value = issue.Iid;
            worksheet.Cell(i + 2, 2).Value = issue.Title;
            worksheet.Cell(i + 2, 3).Value = issue.CreatedAt;
            worksheet.Cell(i + 2, 4).Value = issue.GitLabState.ToString();
            worksheet.Cell(i + 2, 5).Value = issue.AssigneesAsString;
            worksheet.Cell(i + 2, 6).Value = issue.CustomersAsString;
            worksheet.Cell(i + 2, 7).Value = issue.Priority;
        }

        var range = worksheet.RangeUsed()!;
        range.CreateTable();

        // Freeze pane to maintain headers and Id column fixed
        worksheet.SheetView.FreezeRows(1);
        worksheet.SheetView.FreezeColumns(1);
        worksheet.Columns().AdjustToContents();

        worksheet = workbook.Worksheets.Add("Parameters");
        
        
        worksheet.Cell(1, 1).Value = "Project";
        worksheet.Cell(1, 2).Value = project.Name;

        var row = 2;
        foreach (var paramValues in queryParameters.GetActiveParameters())
        {
            worksheet.Cell(row, 1).Value = paramValues.Key;
            worksheet.Cell(row, 2).Value = paramValues.Value.ToString();
            row++;
        }

        worksheet.Range(1, 1, row - 1, 1).Style.Font.Bold = true;
        worksheet.Columns().AdjustToContents();
        

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
