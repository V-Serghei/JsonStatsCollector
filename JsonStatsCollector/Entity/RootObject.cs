using System.Text.Json.Serialization;

namespace JsonStatsCollector.Entity;

public class RootObject
{
    [JsonPropertyName("chats")]
    public ChatContainer? Chats { get; set; }
}