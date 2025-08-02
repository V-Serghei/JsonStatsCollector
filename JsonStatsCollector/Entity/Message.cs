using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonStatsCollector.Entity;

public class Message
{
    [JsonPropertyName("from")]
    public string? From { get; set; }

    [JsonPropertyName("text")]
    public JsonElement Text { get; set; } 
}