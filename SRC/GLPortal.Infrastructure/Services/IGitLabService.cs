using GLPortal.Core.Models;

namespace GLPortal.Infrastructure.Services;

public interface IGitLabService
{
    /// <summary>
    /// Retrieve a list of issues based on passed parameters
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    Task<IssuesList> GetIssuesAsync(IssueQueryParameters parameters);

    /// <summary>
    /// Returns the total count of issues based on passed parameters
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    Task<int> GetIssuesCountAsync(IssueQueryParameters parameters);

    /// <summary>
    /// Retrieve single issue information
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="issueId"></param>
    /// <returns></returns>
    Task<Issue> GetIssueByIdAsync(int projectId, int issueId);


    /// <summary>
    /// Retrieve data about a project
    /// </summary>
    /// <param name="projectId"></param>
    /// <returns></returns>
    Task<Project> GetProjectByIdAsync(int projectId);

    Task<List<Label>> GetProjectLabelsAsync(int projectId);
}


public class IssuesList : List<Issue>
{
    public int TotalCount { get; set; } = 0;
}