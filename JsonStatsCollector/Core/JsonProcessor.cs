using System.IO;
using System.Text.Json;
using System.Windows;
using JsonStatsCollector.Entity;

namespace JsonStatsCollector.Core;

public class JsonProcessor : IJsonProcessor
{
    public List<(string From, string Text)> LoadJsonFile(string filePath)
    {
        try
        {
            var json = File.ReadAllText(filePath);
            using var doc = JsonDocument.Parse(json);
            Console.WriteLine($"Root element type: {doc.RootElement.ValueKind}");

            var result = new List<(string From, string Text)>();
            var root = doc.RootElement;

            if (root.ValueKind != JsonValueKind.Object || !root.TryGetProperty("chats", out var chats) || chats.ValueKind != JsonValueKind.Object)
            {
                Console.WriteLine("Invalid 'chats' property.");
                return result;
            }
            Console.WriteLine($"Chats element type: {chats.ValueKind}");

            if (!chats.TryGetProperty("list", out var chatList) || chatList.ValueKind != JsonValueKind.Array)
            {
                Console.WriteLine("Invalid 'list' property.");
                return result;
            }
            Console.WriteLine($"List element type: {chatList.ValueKind}");

            foreach (var chat in chatList.EnumerateArray())
            {
                if (!chat.TryGetProperty("messages", out var messages) || messages.ValueKind != JsonValueKind.Array)
                {
                    continue;
                }

                foreach (var msg in messages.EnumerateArray())
                {
                    if (!msg.TryGetProperty("from", out var fromProp))
                    {
                        continue;
                    }

                    if (!msg.TryGetProperty("text", out var textProp))
                    {
                        continue;
                    }

                    // Игнорируем сообщения, где "text" — массив
                    if (textProp.ValueKind == JsonValueKind.Array)
                    {
                        continue;
                    }

                    string from = fromProp.GetString();
                    string text = textProp.ValueKind switch
                    {
                        JsonValueKind.String => textProp.GetString(),
                        _ => ""
                    };

                    if (!string.IsNullOrWhiteSpace(from) && !string.IsNullOrWhiteSpace(text))
                    {
                        result.Add((from, text));
                    }
                }
            }
            return result;
        }
        catch (Exception e)
        {
            MessageBox.Show($"Error loading JSON file: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return new List<(string From, string Text)>();
        }
    }

    public void ProcessJsonData()
    {
    }
}