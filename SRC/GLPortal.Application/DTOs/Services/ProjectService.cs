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
            var openIssues = await _gitLabService.GetIssuesCountAsync(projectId, new IssueQueryParameters 
                { 
                State = IssueState.Opened 
                }); 
            var openLastMonth = await _gitLabService.GetIssuesCountAsync(projectId, new IssueQueryParameters
            {
                CreatedAfter = DateTime.Now.AddMonths(-1)
            });
            //var issuesLastMonth = await _gitLabService.GetIssuesAsync(projectId, new IssueQueryParameters
            //{
            //    State = IssueState.Opened,
            //    OrderBy = "created_at",
            //    Sort = "desc"
            //});

            //var closedIssuesLastMonth = issuesLastMonth.Where(i => i.State == IssueState.Closed).ToList();

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

    public async Task<IssuesDTOList> GetIssues(int projectId, IssueQueryParameters parameters)
    {
        var result = new IssuesDTOList();

        var issues = await _gitLabService.GetIssuesAsync(projectId, parameters);

        var priorityRegex = new Regex(_gitLabSettings.PriorityLabelRegex);
        var customerRegex = new Regex(_gitLabSettings.CustomerLabelRegex);

        result.AddRange(issues.Select(i => new IssueDTO(i, customerRegex, priorityRegex)));

        result.TotalCount = issues.TotalCount;

        return result;

    }
}
