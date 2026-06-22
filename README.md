# JsonStatsCollector

WPF-приложение для анализа JSON-экспорта Telegram. Приложение загружает файл экспорта, фильтрует сообщения по выбранному чату/именам и показывает статистику по количеству сообщений, слов, букв и топ-10 часто встречающихся слов.

## Что внутри

- `JsonStatsCollector/MainWindow.xaml` - интерфейс приложения.
- `JsonStatsCollector/MainWindow.xaml.cs` - обработчики выбора файла, добавления имен и запуска анализа.
- `JsonStatsCollector/Core/JsonProcessor.cs` - загрузка и разбор JSON-экспорта.
- `JsonStatsCollector/Entity` - модели для десериализации Telegram JSON.

## Требования

- Windows.
- .NET SDK `10.0.301` или новее в линейке .NET 10.
- .NET Windows Desktop Runtime 10.

Версия SDK закреплена в `global.json` с `rollForward: latestFeature`, поэтому более свежий feature-band .NET 10 тоже подойдет.

## Зависимости

- `LiveCharts` `0.9.7`
- `LiveCharts.Wpf` `0.9.7`

На момент проверки `dotnet list package --outdated` не показывает более новых версий для этих пакетов в текущей ветке. При этом NuGet может предупреждать, что `LiveCharts 0.9.7` восстановлен как .NET Framework-пакет. Это существующий нюанс библиотеки; если понадобится полностью убрать предупреждение совместимости, лучше мигрировать графики на современную ветку `LiveChartsCore`, но это уже отдельное изменение API.

## Запуск

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

## Как пользоваться

1. Нажать `Обзор` и выбрать JSON-файл экспорта Telegram.
2. Добавить имя чата/участника через `Добавить имя`.
3. Нажать `Анализировать`.

Первое добавленное имя используется как фильтр для поиска нужного чата. Статистика на графиках выводится только для добавленных имен.

## Git hygiene

В репозиторий не должны попадать:

- `bin/`, `obj/`, `Debug/`, `Release/`;
- `.vs/`, `.idea/`, `_ReSharper.Caches/`;
- локальные пользовательские настройки `*.user`, `*.DotSettings.user`;
- временные файлы редакторов и ОС.

`.gitignore` уже настроен под эти правила. Если эти файлы уже были добавлены в git раньше, их нужно один раз убрать из индекса командой `git rm --cached`, не удаляя локально с диска.

Команды для очистки уже отслеживаемого IDE/cache-мусора:

```powershell
git rm -r --cached .idea
git rm --cached JsonStatsCollector.sln.DotSettings.user
git rm --cached JsonStatsCollector\JsonStatsCollector.csproj.DotSettings.user
git status
```

После этого проверьте статус, добавьте нужные изменения и сделайте коммит уже вручную:

```powershell
git add .gitignore README.md global.json JsonStatsCollector\JsonStatsCollector.csproj
git status
git commit -m "chore: update project baseline and git hygiene"
```

`git rm --cached` уже подготовит удаления из индекса, чтобы в pull request эти файлы исчезли из последнего состояния репозитория, но остались у вас локально.
