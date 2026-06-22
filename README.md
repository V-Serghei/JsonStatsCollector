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

1. Click `Обзор` and select a Telegram JSON export file.
2. Add a chat or participant name with `Добавить имя`.
3. Click `Анализировать`.

The first added name is used to find the target chat. Charts are generated for the names added in the control panel.

<p align="center">
  <a href="#русская-версия"><b>Русская версия</b></a>
</p>

## Русская Версия

JsonStatsCollector - это Windows WPF-приложение для анализа JSON-экспорта Telegram. Приложение читает файл экспорта, фильтрует сообщения по названию чата или именам участников и показывает статистику по сообщениям, словам, буквам и самым частым словам.

## Возможности

- Загрузка JSON-файлов экспорта Telegram.
- Фильтрация статистики по выбранному чату или участникам.
- Графики количества сообщений, слов и букв.
- Список топ-10 самых частых слов.
- Темный WPF-интерфейс с визуализацией через LiveCharts.

## Требования

- Windows.
- .NET SDK `10.0.301` или новее в линейке .NET 10.
- .NET Windows Desktop Runtime 10.

Версия SDK закреплена в `global.json` с `rollForward: latestFeature`, поэтому более свежие feature-band версии .NET 10 также подойдут.

## Зависимости

- `LiveCharts` `0.9.7`
- `LiveCharts.Wpf` `0.9.7`

`LiveCharts 0.9.7` - последняя версия в используемой сейчас линейке пакета. NuGet может показывать предупреждения совместимости, потому что пакет использует .NET Framework assets. Приложение при этом собирается на .NET 10, но для полностью современной совместимости графики лучше отдельно перенести на `LiveChartsCore`.

## Структура Проекта

- `JsonStatsCollector/MainWindow.xaml` - основной интерфейс приложения.
- `JsonStatsCollector/MainWindow.xaml.cs` - обработчики интерфейса и подготовка статистики.
- `JsonStatsCollector/Core/JsonProcessor.cs` - загрузка и разбор Telegram JSON.
- `JsonStatsCollector/Entity` - модели JSON.

## Локальный Запуск

Восстановить зависимости:

```powershell
dotnet restore
```

Собрать проект:

```powershell
dotnet build
```

Запустить приложение:

```powershell
dotnet run --project .\JsonStatsCollector\JsonStatsCollector.csproj
```

## Использование

1. Нажмите `Обзор` и выберите JSON-файл экспорта Telegram.
2. Добавьте название чата или имя участника через `Добавить имя`.
3. Нажмите `Анализировать`.

Первое добавленное имя используется для поиска нужного чата. Графики строятся для имен, добавленных в панели управления.
