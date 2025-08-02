using System.Text.Json;
using JsonStatsCollector.Entity;

namespace JsonStatsCollector.Core;

public interface IJsonProcessor
{
    List<(string From, string Text)> LoadJsonFile(string filePath);
    
    Task<List<(string From, string Text)>> LoadJsonFileOptimizedAsync(string filePath, string? chatName = null);
    string? ExtractTextFromJsonElement(JsonElement element);
    void ProcessJsonData();
}