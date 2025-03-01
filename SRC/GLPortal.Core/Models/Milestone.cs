namespace GLPortal.Core.Models;

using System.Text.Json.Serialization;

public class Milestone
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
    public string State { get; set; } = string.Empty; // "active", "closed"

    [JsonPropertyName("due_date")]
    public DateTime? DueDate { get; set; }
}
