using GLPortal.Core.Models;
using GLPortal.Infrastructure.Interfaces;

using Microsoft.Extensions.Caching.Memory;

using System.Text.Json;

namespace GLPortal.Infrastructure.Services;
public class GitLabService(HttpClient httpClient, IMemoryCache memoryCache) : IGitLabService
{
    private readonly TimeSpan cacheExpiration = TimeSpan.FromMinutes(5);

    public async Task<IssuesList> GetIssuesAsync(IssueQueryParameters parameters)
    {
        IssuesList result;
        // if PerPage is -1 then all issues are retrieved with iterative calls and maximum PerPage value
        if (parameters.PerPage == -1)
        {
            result = new IssuesList();
            var iterativeParameters = parameters.Clone() as IssueQueryParameters;
            iterativeParameters!.PerPage = 100;
            iterativeParameters.Page = 1;
            
            while (true)
            {
                var partialResult = await GetIssuesAsync(iterativeParameters);
                if (!partialResult.Any())
                    break;
                result.AddRange(partialResult);
                result.TotalCount = partialResult.TotalCount;
                iterativeParameters.Page++;
            }

            return result;
        }


        string url = $"projects/{parameters.ProjectId}/issues";
        string queryString = parameters.ToString();
        if (!string.IsNullOrEmpty(queryString))
        {
            url += "?" + queryString;
        }

        var response = await httpClient.GetAsync(url);
        
        var content = await response.Content.ReadAsStringAsync();
        result = JsonSerializer.Deserialize<IssuesList>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
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

    public async Task<int> GetIssuesCountAsync(IssueQueryParameters parameters)
    {
        parameters.PerPage = 1;
        parameters.Page = 1;
        string url = $"projects/{parameters.ProjectId}/issues";
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
