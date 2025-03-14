namespace GLPortal.Core.Models;

using System.Text.Json.Serialization;

public class Release
{
    [JsonPropertyName("tag_name")]
    public string TagName { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}
