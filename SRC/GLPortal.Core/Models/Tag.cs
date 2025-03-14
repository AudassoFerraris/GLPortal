namespace GLPortal.Core.Models;

using System.Text.Json.Serialization;

public class Tag
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("target")]
    public string Target { get; set; } = string.Empty;

    [JsonPropertyName("commit")]
    public Commit? Commit { get; set; }

    [JsonPropertyName("release")]
    public Release? Release { get; set; }

    [JsonPropertyName("protected")]
    public bool IsProtected { get; set; }
}
