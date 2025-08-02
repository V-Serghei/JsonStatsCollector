using System.Text.Json.Serialization;

namespace JsonStatsCollector.Entity;

public class Chat
{
    [JsonPropertyName("name")]
    public string? Name { get; set; } 
    [JsonPropertyName("messages")]
    public List<Message>? Messages { get; set; }
}