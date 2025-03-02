using GLPortal.Core.Enums;
using GLPortal.Core.Models;

using System.Text.RegularExpressions;

namespace GLPortal.Application.DTOs;
public class IssueDTO
{
    public IssueDTO(Issue source, Regex customersRegex, Regex priorityRegex)
    {
        Iid = source.Iid;
        Title = source.Title;
        CreatedAt = source.CreatedAt;
        GitLabState = source.State;
        Assignees = source.Assignees?.Select(u => u.Username).ToArray();
        Customers = ExtractLabels(source.Labels, customersRegex);
        var priorities = ExtractLabels(source.Labels, priorityRegex);
        Priority = priorities.FirstOrDefault();
        PriorityWarning = priorities.Length > 1;
    }

    public int Iid { get; set; }

    public string Title { get; set; }

    public DateTime CreatedAt { get; set; }
    
    public IssueState GitLabState { get; set; }

    public string[]? Assignees { get; set; }

    public string? AssigneesAsString
    {
        get => Assignees != null && Assignees.Any() ? 
            string.Join(", ", Assignees) : null;
    }

    public string[]? Customers { get; set; }

    public string? CustomersAsString
    {
        get => Customers != null && Customers.Any() ? string.Join(", ", Customers) : null;
    }

    public string? Priority { get; set; }

    public bool PriorityWarning { get; set; }

    string[] ExtractLabels(IEnumerable<string>? labels, Regex regex)
    {
        if (labels == null)
            return Array.Empty<string>();
        var result = labels.Select(_ => ParseLabel(_, regex))
            .Where(_ => _ is not null)
            .Cast<string>()
            .ToArray();
        return result;
    }

    string? ParseLabel(string label, Regex regex)
    {
        var match = regex.Match(label);
        if (match.Success && match.Groups.Count == 2)
            return match.Groups[1].Value;
        return null;
    }
}
