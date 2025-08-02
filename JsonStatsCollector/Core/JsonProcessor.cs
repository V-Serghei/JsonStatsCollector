using System.IO;
using System.Text.Json;
using System.Windows;
using JsonStatsCollector.Entity;

namespace JsonStatsCollector.Core;

public class JsonProcessor : IJsonProcessor
{
    public  List<(string From, string Text)> LoadJsonFile(string filePath)
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
                    
                    if (textProp.ValueKind == JsonValueKind.Array)
                    {
                        continue;
                    }

                    string? from = fromProp.GetString();
                    string? text = textProp.ValueKind switch
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

    public async Task<List<(string From, string Text)>> LoadJsonFileOptimizedAsync(string filePath, string? chatName = null)
    {
        var result = new List<(string, string)>();
        try
        {
            await using var stream = File.OpenRead(filePath);
            var root = await JsonSerializer.DeserializeAsync<RootObject>(stream);

            if (root?.Chats?.List is null)
            {
                return result;
            }

            var find = false;

            foreach (var chat in root.Chats.List)
            {
                if (!find)
                {
                    if (!string.IsNullOrWhiteSpace(chat.Name) &&
                        chat.Name.Equals(chatName, StringComparison.OrdinalIgnoreCase))
                    {
                        find = true;
                    }
                }
                else
                {
                    break;
                }
            }
            
            foreach (var chat in root.Chats.List)
            {
                if (!string.IsNullOrWhiteSpace(chatName))
                {
                    bool isMatch = false;
                    if (find)
                    {
                        isMatch = !string.IsNullOrWhiteSpace(chat.Name) &&
                                  chat.Name.Equals(chatName, StringComparison.OrdinalIgnoreCase);
                    }
                    else
                    {
                        isMatch = !string.IsNullOrWhiteSpace(chat.Name) &&
                                  (chat.Name.Contains(chatName) || chatName.Contains(chat.Name));
                    }
                    

                    if (!isMatch)
                    {
                        continue;
                    }
                }


                if (chat.Messages is null)
                {
                    continue;
                }

                foreach (var msg in chat.Messages)
                {
                    if (msg.Text.ValueKind == JsonValueKind.Array)
                    {
                        continue;
                    }

                    var from = msg.From;
                    string? text = null;
                    if (msg.Text.ValueKind == JsonValueKind.String)
                    {
                        text = msg.Text.GetString();
                    }

                    if (!string.IsNullOrWhiteSpace(from) && !string.IsNullOrWhiteSpace(text))
                    {
                        result.Add((from, text));
                    }
                }
            }
        }
        catch (JsonException ex)
        {
            MessageBox.Show($"Error parsing JSON file: {ex.Message}", "JSON Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        return result;
    }
    public string? ExtractTextFromJsonElement(JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.String)
        {
            return element.GetString();
        }

        if (element.ValueKind == JsonValueKind.Array)
        {
            return string.Join("", element.EnumerateArray()
                .Where(e => e.TryGetProperty("text", out var textProp) && textProp.ValueKind == JsonValueKind.String)
                .Select(e => e.GetProperty("text").GetString()));
        }

        return null;
    }
    public void ProcessJsonData()
    {
    }
}