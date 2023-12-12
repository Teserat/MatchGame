using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        string bestTime;
        decimal previousTime;
        decimal newTime;
        bool check = false;

        public MainWindow()
        {
            InitializeComponent();
            bestScoreBlock.Visibility = Visibility.Hidden;
            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondsElapsed ++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
            newTime = ((decimal)(tenthsOfSecondsElapsed / 10F));

            if (matchesFound == 8)
            {
                timer.Stop();
                if (bestTime == null)
                {
                    bestTime = timeTextBlock.Text;
                    previousTime = newTime;
                }
                else if  (newTime < previousTime )
                {
                    bestTime = timeTextBlock.Text;
                    bestScoreBlock.Text = null;
                    bestScoreBlock.Text = "Best time" + "\n" + bestTime;
                }
                
                timeTextBlock.Text = timeTextBlock.Text + " - Again?";
                previousTime = newTime;
            }
            
        }

        private void SetUpGame()
        {
            List<string> animalEmoji = new List<string>()
            {
                "🐇","🐇",
                "🐢","🐢",
                "🐕","🐕",
                "🐈","🐈",
                "🐁","🐁",
                "🦅","🦅",
                "🐎","🐎",
                "🦔","🦔",

            };
            Random random = new Random();
            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if(textBlock.Name != "timeTextBlock" && textBlock.Name != "bestScoreBlock")
                {
                    textBlock.Visibility = Visibility.Visible;
                    int index = random.Next(animalEmoji.Count);
                    string nextEmoji = animalEmoji[index];
                    textBlock.Text = nextEmoji;
                    animalEmoji.RemoveAt(index);
                }

            }
            timer.Start();
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;
            if (bestTime != null && check == false)
            {
                bestScoreBlock.Visibility = Visibility.Visible;
                bestScoreBlock.Text = bestScoreBlock.Text + "\n" + bestTime;
                check = true;
            }

        }
        TextBlock lastTextBlockClicked;
        bool findingMatch = false;
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            if (findingMatch == false) //first click
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
            else if(textBlock.Text == lastTextBlockClicked.Text) //positive 
            {
                matchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch= false;
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
