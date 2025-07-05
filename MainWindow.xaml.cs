using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MatchGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;
        TextBlock lastTextBlockClicked = null!; // Initialize with null-forgiving operator
        bool findingMatch = false;

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += Timer_Tick;

            SetUpGame();
        }

        private void SetUpGame()
        {
            List<string> animalEmoji = new List<string>()
                {
                    "🦣","🦣",
                    "🐵","🐵",
                    "🦝","🦝",
                    "🐗","🐗",
                    "🐼","🐼",
                    "🐲","🐲",
                    "🐔","🐔",
                    "🐴","🐴",
                };

            Random random = new Random();
            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name != "timeTextblock")
                {
                    int index = random.Next(animalEmoji.Count);
                    string nextEmoji = animalEmoji[index];
                    textBlock.Text = nextEmoji;
                    animalEmoji.RemoveAt(index);

                    // Ensure the emoji is visible at the start of a new game
                    textBlock.Visibility = Visibility.Visible;
                }
            }
            timer.Start();
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            if (findingMatch == false)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
            else if (textBlock.Text == lastTextBlockClicked.Text)
            {
                matchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timeTextblock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();
                timeTextblock.Text = timeTextblock.Text + " - You won";
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8)
            {
                SetUpGame();
            }
        }
    }
}