using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using JsonStatsCollector.Core;
using Microsoft.Win32;
using LiveCharts;
using LiveCharts.Wpf;

namespace JsonStatsCollector
{
    public partial class MainWindow : Window
    {
        #region initialization variables

        private List<string> _names = new List<string>();
        private Dictionary<string, int>  WordFrequency = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, int>  WordFrequency1 = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        private List<ChartData> StatisticsMessagesCount { get; set; } = new List<ChartData>();
        private List<ChartData> StatisticsWordsCount { get; set; } = new List<ChartData>();
        private List<ChartData> StatisticsLettersCount { get; set; } = new List<ChartData>();
        public List<ChartData> Top10Stats { get; set; } = new();
        public SeriesCollection  StatisticsMessages { get; set; }
        public SeriesCollection  StatisticsWords { get; set; }
        public SeriesCollection StatisticsLetters { get; set; }
        private string _jsonFilePath = string.Empty;
        private JsonProcessor _jsonProcessor = new JsonProcessor();

        #endregion

        #region constructor

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            
            StatisticsMessages = new SeriesCollection();
            StatisticsWords = new SeriesCollection();
            StatisticsLetters = new SeriesCollection();
        }

        #endregion

        #region button handlers
        
        /// <summary>
        /// open file dialog for selecting a JSON file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                FilePathTextBox.Text = openFileDialog.FileName;
                _jsonFilePath = openFileDialog.FileName;
                
            }
        }
        /// <summary>
        /// edit the names of the senders
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNameButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO: add option to remove names
            //TODO: add option to edit names
            TextBox newNameTextBox = new TextBox
            {
                Style = (Style)FindResource("DarkTextBoxStyle"),
                Margin = new Thickness(0, 5, 0, 0)
            };
    
            newNameTextBox.TextChanged += (s, args) =>
            {
                string name = newNameTextBox.Text.Trim();
                if (!string.IsNullOrEmpty(name) && !_names.Contains(name))
                {
                    _names.Add(name);
                }
            };
            NamesPanel.Children.Add(newNameTextBox);
        }

        /// <summary>
        /// analyze the JSON file and display statistics
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AnalyzeButton_Click(object sender, RoutedEventArgs e)
        {
            #region cleaning up previous data

            StatisticsWordsCount.Clear();
            StatisticsWords.Clear();
            StatisticsLettersCount.Clear();
            StatisticsLetters.Clear();
            StatisticsMessagesCount.Clear();
            StatisticsMessages.Clear();
            WordFrequency1.Clear();
            WordFrequency.Clear();

            #endregion

            #region loading JSON file

            var listOfMessage = await _jsonProcessor.LoadJsonFileOptimizedAsync(_jsonFilePath, _names.Count > 0 ? _names[0] : null);
            if (listOfMessage.Count == 0)
            {
                MessageBox.Show("No messages found in the JSON file.", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            #endregion
            
            #region the first method
            var stopwatch1 = Stopwatch.StartNew();
            var groupedMessages = listOfMessage
                .GroupBy(m => m.From)
                .ToList();

            StatisticsWordsCount = groupedMessages
                .Select(g => new ChartData(
                    g.Key,
                    g.Sum(m => m.Text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length)
                ))
                .ToList();

            StatisticsLettersCount = groupedMessages
                .Select(g => new ChartData(
                    g.Key,
                    g.Sum(m => m.Text.Count(c => char.IsLetter(c)))
                ))
                .ToList();

            StatisticsMessagesCount = groupedMessages.
                Select(g => new ChartData(g.Key, g.Count()))
                .ToList();

            WordFrequency1 = groupedMessages
                .SelectMany(group => group)
                .SelectMany(message => message.Text.
                    Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries))
                .GroupBy(word => word, StringComparer.OrdinalIgnoreCase)
                .OrderByDescending(g => g.Count())
                .Take(10)
                .ToDictionary(g => g.Key, g => g.Count());

            stopwatch1.Stop();
            Console.WriteLine($"Время обработки способ1: {stopwatch1.ElapsedMilliseconds} мс");

            #endregion

            #region the second method

            var stopwatch = Stopwatch.StartNew();

            var aggregatedStats = listOfMessage
                .GroupBy(m => m.From)
                .Select(g =>
                {
                    int messageCount = 0;
                    int wordCount = 0;
                    int letterCount = 0;

                    foreach (var msg in g)
                    {
                        messageCount++;
                        letterCount += msg.Text.Count(c => char.IsLetter(c));

                        var words = msg.Text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                        wordCount += words.Length;

                        foreach (var word in words)
                        {
                            WordFrequency.TryGetValue(word, out int currentCount);
                            WordFrequency[word] = currentCount + 1;
                        }
                    }

                    return new
                    {
                        From = g.Key,
                        Messages = messageCount,
                        Words = wordCount,
                        Letters = letterCount
                    };
                })
                .ToList();

            var top10Words = WordFrequency
                .OrderByDescending(kvp => kvp.Value)
                .Take(10)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value).ToList();


            StatisticsMessagesCount = aggregatedStats
                .Select(s => new ChartData(s.From, s.Messages))
                .ToList();

            StatisticsWordsCount = aggregatedStats
                .Select(s => new ChartData(s.From, s.Words))
                .ToList();

            StatisticsLettersCount = aggregatedStats
                .Select(s => new ChartData(s.From, s.Letters))
                .ToList();
            stopwatch.Stop();
            Console.WriteLine($"Время обработки2: {stopwatch.ElapsedMilliseconds} мс");

            #endregion
            
            #region statistics loading

            StatisticsMessages.AddRange(
                StatisticsMessagesCount
                    .Where(data => _names.Contains(data.Name))
                    .Select(data => new PieSeries {
                        Title = data.Name,
                        Values = new ChartValues<int> { data.Value },
                        DataLabels = true
                    })
            );
            
            Top10Stats = WordFrequency
                .Where(kvp => kvp.Key.Length >= 5 && kvp.Key.All(char.IsLetter))
                .Select(kvp => new ChartData(kvp.Key, kvp.Value))
                .OrderByDescending(data => data.Value)
                .Take(10)
                .ToList();
            Top10ListView.ItemsSource = Top10Stats;

            
            StatisticsWords.AddRange(
                StatisticsWordsCount
                    .Where(data => _names.Contains(data.Name))
                    .Select(data => new PieSeries {
                        Title = data.Name,
                        Values = new ChartValues<int> { data.Value },
                        DataLabels = true
                    })
            );

            StatisticsLetters.AddRange(
                StatisticsLettersCount
                    .Where(data => _names.Contains(data.Name))
                    .Select(data => new PieSeries {
                        Title = data.Name,
                        Values = new ChartValues<int> { data.Value },
                        DataLabels = true
                    })
            );

            #endregion
            
            
        }

        #endregion
       
    }
}