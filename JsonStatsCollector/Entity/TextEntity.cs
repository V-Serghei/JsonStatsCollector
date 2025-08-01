namespace JsonStatsCollector.Entity;

public abstract class TextEntity(string type, string text)
{
    public string Type { get; set; } = type;
    public string Text { get; set; } = text;
}