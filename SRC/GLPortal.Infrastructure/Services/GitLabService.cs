using GLPortal.Core.Models;
using GLPortal.Infrastructure.Interfaces;

using Microsoft.Extensions.Caching.Memory;

using System.Text.Json;

namespace GLPortal.Infrastructure.Services;
public class GitLabService(HttpClient httpClient, IMemoryCache memoryCache) : IGitLabService
{
    private readonly TimeSpan cacheExpiration = TimeSpan.FromMinutes(5);

    public async Task<IssuesList> GetIssuesAsync(int projectId, IssueQueryParameters parameters)
    {
        string url = $"projects/{projectId}/issues";
        string queryString = parameters.ToString();
        if (!string.IsNullOrEmpty(queryString))
        {
            url += "?" + queryString;
        }

        var response = await httpClient.GetAsync(url);
        
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<IssuesList>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? throw new Exception("Can't deserialize response to a list of Issue objects");

        if (response.Headers.TryGetValues("X-Total", out var values) && int.TryParse(values.FirstOrDefault(), out var count))
        {
            result.TotalCount  = count;
        }

        return result;
    }

    public async Task<Issue> GetIssueByIdAsync(int projectId, int issueId)
    {
        var response = await httpClient.GetStringAsync($"projects/{projectId}/issues/{issueId}");
        return JsonSerializer.Deserialize<Issue>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? throw new Exception("Can't deserialize response to Issue object");
    }

    /// <summary>
    /// Get a project by its ID. The result is cached for 5 minutes.
    /// </summary>
    /// <param name="projectId"></param>
    /// <returns></returns>
    /// <exception cref="Exception">When can't retrieve the project</exception>
    public async Task<Project> GetProjectByIdAsync(int projectId)
    {
        var result = await memoryCache.GetOrCreateAsync($"gitlab_project_{projectId}",
            async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = cacheExpiration;
                var project = await GetProjectByIdFromApiAsync(projectId)!;
                return project;
            });
        return result!;
    }

    public async Task<int> GetIssuesCountAsync(int projectId, IssueQueryParameters parameters)
    {
        parameters.PerPage = 1;
        parameters.Page = 1;
        string url = $"projects/{projectId}/issues";
        string queryString = parameters.ToString();
        if (!string.IsNullOrEmpty(queryString))
        {
            url += "?" + queryString;
        }

        var response = await httpClient.GetAsync(url);
        if (response.Headers.TryGetValues("X-Total", out var values) && int.TryParse(values.FirstOrDefault(), out var count))
        {
            return count;
        }
        return 0;
    }

    private async Task<Project> GetProjectByIdFromApiAsync(int projectId)
    {
        var response = await httpClient.GetStringAsync($"projects/{projectId}");
        return JsonSerializer.Deserialize<Project>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? throw new Exception("Can't deserialize response to Project object");
    }
}
