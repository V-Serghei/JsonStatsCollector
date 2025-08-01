namespace JsonStatsCollector.Entity;

public class MessageItem(string sender, string messageText)
{
    public string Sender { get; set; } = sender;
    public string MessageText { get; set; } = messageText;
}