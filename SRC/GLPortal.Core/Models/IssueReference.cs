namespace GLPortal.Core.Models;

using System.Text.Json.Serialization;

public class IssueReference
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("iid")]
    public int Iid { get; set; } // ID interno al progetto

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("web_url")]
    public string WebUrl { get; set; } = string.Empty;
}
