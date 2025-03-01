namespace GLPortal.Core.Enums;

using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum IssueState
{
    Opened,
    Closed
}
