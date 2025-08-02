using System.Text.Json.Serialization;

namespace JsonStatsCollector.Entity;

public class ChatContainer
{
    [JsonPropertyName("list")]
    public List<Chat>? List { get; set; }
}