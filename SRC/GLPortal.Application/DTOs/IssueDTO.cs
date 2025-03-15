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
        UpdatedAt = source.UpdatedAt;
        ClosedAt = source.ClosedAt;
        GitLabState = source.State;
        Assignees = source.Assignees?.Select(u => u.Username).ToArray();
        Customers = ExtractLabels(source.Labels, customersRegex);
        var priorities = ExtractLabels(source.Labels, priorityRegex);
        Priority = priorities.FirstOrDefault();
        PriorityWarning = priorities.Length > 1;
        Milestone = source.Milestone?.Title;
        Labels = ExtractOtherLabels(source.Labels, new[] { customersRegex, priorityRegex });
        WebUrl = source.WebUrl;
    }

    public int Iid { get; set; }

    public string Title { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? ClosedAt { get; set; }

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

    /// <summary>
    /// Extract labels that doesn't match the regex
    /// </summary>
    /// <param name="labels"></param>
    /// <param name="regex"></param>
    /// <returns></returns>
    string? ExtractOtherLabels(IEnumerable<string>? labels, params Regex[] regex)
    {
        if (labels == null)
            return null;

        var notMatchingLabels = new List<string>();
        foreach (var label in labels)
        {
            var isMatching = false;
            foreach (var r in regex)
            {
                if (r.IsMatch(label))
                {
                    isMatching = true;
                    break;
                }
            }
            if (!isMatching)
                notMatchingLabels.Add(label);
        }
        if (notMatchingLabels.Any())
            return string.Join(", ", notMatchingLabels);
        return null;


    }

    string? ParseLabel(string label, Regex regex)
    {
        var match = regex.Match(label);
        if (match.Success && match.Groups.Count == 2)
            return match.Groups[1].Value;
        return null;
    }

    public string? Milestone { get; set; }

    /// <summary>
    /// LAbels other than customers and priority
    /// </summary>
    public string? Labels { get; set; }

    public string WebUrl { get; set; } = string.Empty;
}
