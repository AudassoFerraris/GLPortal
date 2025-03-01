namespace GLPortal.Core.Models;

using GLPortal.Core.Enums;

using System.Text.Json.Serialization;

public class Issue
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("iid")]
    public int Iid { get; set; } // ID interno al progetto

    [JsonPropertyName("project_id")]
    public int ProjectId { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("state")]
    public IssueState State { get; set; } = IssueState.Opened;

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("closed_at")]
    public DateTime? ClosedAt { get; set; }

    [JsonPropertyName("labels")]
    public List<string> Labels { get; set; } = new();

    [JsonPropertyName("milestone")]
    public Milestone? Milestone { get; set; }

    [JsonPropertyName("assignees")]
    public List<User> Assignees { get; set; } = new();

    [JsonPropertyName("author")]
    public User? Author { get; set; }

    [JsonPropertyName("web_url")]
    public string WebUrl { get; set; } = string.Empty;
}
