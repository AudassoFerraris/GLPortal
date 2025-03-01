﻿using GLPortal.Application.DTOs;
using GLPortal.Core.Enums;
using GLPortal.Core.Models;
using GLPortal.Core.Settings;
using GLPortal.Infrastructure.Interfaces;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace GLPortal.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IGitLabService _gitLabService;
    private readonly IMemoryCache _cache;
    private readonly List<int> _configuredProjectIds;

    public ProjectService(IGitLabService gitLabService, IMemoryCache cache, IOptions<GitLabSettings> options)
    {
        _gitLabService = gitLabService;
        _cache = cache;
        _configuredProjectIds = options.Value.ProjectIds ?? new List<int>();
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

        foreach (var projectId in _configuredProjectIds)
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
}
