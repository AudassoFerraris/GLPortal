using GLPortal.Application.DTOs;
using GLPortal.Core.Models;

namespace GLPortal.Application.Services;

public interface IProjectService
{
    Task<List<ProjectSummaryDTO>> GetProjectsAsync();

    Task<IssuesDTOList> GetIssues(IssueQueryParameters parameters);

    Task<byte[]> ExportIssuesToExcelAsync(IssueQueryParameters queryParameters);
}


public class IssuesDTOList : List<IssueDTO>
{
    public int TotalCount { get; set; } = 0;
}

