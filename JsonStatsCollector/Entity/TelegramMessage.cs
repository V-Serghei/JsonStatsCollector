using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonStatsCollector.Entity;

public class TelegramMessage(string from, string text, string mediaType, List<TextEntity> textEntities)
{
    [JsonPropertyName("from")]
    public string From { get; set; }

    [JsonPropertyName("text")]
    public JsonElement TextRaw { get; set; }

    [JsonIgnore]
    public string? Text => ExtractText(TextRaw);

    private static string? ExtractText(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Array => string.Join("", element.EnumerateArray()
                .Where(e => e.TryGetProperty("text", out _))
                .Select(e => e.GetProperty("text").GetString())),
            _ => string.Empty
        };
    }
}