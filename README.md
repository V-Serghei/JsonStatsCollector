# JsonStatsCollector

JsonStatsCollector is a Windows WPF application for analyzing Telegram JSON exports. It reads an exported chat file, filters messages by chat or participant names, and displays message, word, letter, and top-word statistics.

## Features

- Loads Telegram JSON export files.
- Filters statistics by selected chat or participant names.
- Shows message count, word count, and letter count charts.
- Displays the top 10 most frequent words.
- Uses a dark WPF interface with LiveCharts visualizations.

## Requirements

- Windows.
- .NET SDK `10.0.301` or newer within the .NET 10 line.
- .NET Windows Desktop Runtime 10.

The SDK version is pinned in `global.json` with `rollForward: latestFeature`, so newer .NET 10 feature bands are supported.

## Dependencies

- `LiveCharts` `0.9.7`
- `LiveCharts.Wpf` `0.9.7`

`LiveCharts 0.9.7` is the latest version available for the package line currently used by this project. NuGet may show compatibility warnings because the package targets .NET Framework assets. The application still builds on .NET 10, but migrating charts to `LiveChartsCore` would be the cleaner long-term option if full modern .NET compatibility is required.

## Project Structure

- `JsonStatsCollector/MainWindow.xaml` - main application UI.
- `JsonStatsCollector/MainWindow.xaml.cs` - UI event handlers and statistics preparation.
- `JsonStatsCollector/Core/JsonProcessor.cs` - Telegram JSON loading and parsing.
- `JsonStatsCollector/Entity` - JSON model classes.

## Run Locally

Restore dependencies:

```powershell
dotnet restore
```

Build the project:

```powershell
dotnet build
```

Run the application:

```powershell
dotnet run --project .\JsonStatsCollector\JsonStatsCollector.csproj
```

## Usage

1. Click the browse button and select a Telegram JSON export file.
2. Add a chat or participant name in the control panel.
3. Run the analysis.

The first added name is used to find the target chat. Charts are generated for the names added in the control panel.
