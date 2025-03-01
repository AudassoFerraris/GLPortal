namespace GLPortal.Core.Models;

using System.Text.Json.Serialization;

public class TimeTracking
{
    [JsonPropertyName("time_estimate")]
    public int TimeEstimate { get; set; } // Tempo stimato in secondi

    [JsonPropertyName("total_time_spent")]
    public int TotalTimeSpent { get; set; } // Tempo speso in secondi

    [JsonPropertyName("human_time_estimate")]
    public string? HumanTimeEstimate { get; set; } // Es: "2h 30m"

    [JsonPropertyName("human_total_time_spent")]
    public string? HumanTotalTimeSpent { get; set; } // Es: "1h 15m"
}
