using GLPortal.Application.DTOs;
using GLPortal.Core.Models;

namespace GLPortal.Application.Services;

public interface IProjectService
{
    Task<List<ProjectSummaryDTO>> GetProjectsAsync();

    Task<IssuesDTOList> GetIssues(int projectId, IssueQueryParameters parameters);
}


public class IssuesDTOList : List<IssueDTO>
{
    public int TotalCount { get; set; } = 0;
}