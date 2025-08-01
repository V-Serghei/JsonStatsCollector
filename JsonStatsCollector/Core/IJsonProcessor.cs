using JsonStatsCollector.Entity;

namespace JsonStatsCollector.Core;

public interface IJsonProcessor
{
    List<(string From, string Text)> LoadJsonFile(string filePath);
    void ProcessJsonData();
}