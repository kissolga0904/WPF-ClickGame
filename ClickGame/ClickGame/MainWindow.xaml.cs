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

namespace ClickGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();
        List<Ellipse> removeThis = new List<Ellipse>();

        int spawnRate = 60;
        int currentRate;
        int lastScore = 0;
        int health = 350;
        int posX;
        int posY;
        int score = 0;
        double growthRate = 0.6;

        Random rnd = new Random();

        MediaPlayer playClickSound = new MediaPlayer();
        MediaPlayer playerPopSound = new MediaPlayer();

        Uri ClickedSound;
        Uri PoppedSound;

        Brush brush;

        public MainWindow()
        {
            InitializeComponent();

            gameTimer.Tick += GameLoop;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Start();

            currentRate = spawnRate;

            ClickedSound = new Uri("C:/Users/kisso/Desktop/Pti BSc/3. félév/Programozási környezetek/ClickGame/ClickGame/sound/clickedpop.mp3");
            ClickedSound = new Uri("C:/Users/kisso/Desktop/Pti BSc/3. félév/Programozási környezetek/ClickGame/ClickGame/sound/pop.mp3");
        }

        private void GameLoop(object sender, EventArgs e)
        {
            txtScore.Content = "Score: " + score;
            txtLastScore.Content = "Last score: " + lastScore;
            currentRate -= 2;
            if(currentRate < 1)
            {
                currentRate = spawnRate;
                posX = rnd.Next(15, 700);
                posY = rnd.Next(50, 350);

                brush = new SolidColorBrush(Color.FromRgb((byte)rnd.Next(1, 255), (byte)rnd.Next(1, 255), (byte)rnd.Next(1, 255)));
                Ellipse circle = new Ellipse
                {
                    Tag = "circle",
                    Height = 10,
                    Width = 10,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    Fill = brush
                };
                Canvas.SetLeft(circle, posX);
                Canvas.SetTop(circle, posY);
                MyCanvas.Children.Add(circle);
            }

            foreach (var x in MyCanvas.Children.OfType<Ellipse>())
            {
                x.Height += growthRate;
                x.Width += growthRate;
                x.RenderTransformOrigin = new Point(0.5, 0.5);
                if(x.Width > 70)
                {
                    removeThis.Add(x);
                    health -= 15;
                    playerPopSound.Open(PoppedSound);
                    playerPopSound.Play();
                }
            }

            if(health > 1)
            {
                healthBar.Width = health; 
            }
            else
            {
                GameOverFunction();
            }
            foreach(Ellipse i in removeThis)
            {
                MyCanvas.Children.Remove(i);
            }
            if(score > 5)
            {
                spawnRate = 25;
            }
            if(score > 20)
            {
                spawnRate = 15;
                growthRate = 1.5;
            }
        }

        private void ClickOnCanvas(object sender, MouseButtonEventArgs e)
        {
            if(e.OriginalSource is Ellipse)
            {
                Ellipse circle = (Ellipse)e.OriginalSource;
                MyCanvas.Children.Remove(circle);
                score++;

                playClickSound.Open(ClickedSound);
                playClickSound.Play();
            }
        }

        private void GameOverFunction()
        {
            gameTimer.Stop();
            MessageBox.Show("Game Over!" + Environment.NewLine + "Score: " + score + Environment.NewLine + "Click ok to play again!");

            foreach (var y in MyCanvas.Children.OfType<Ellipse>())
            {
                removeThis.Add(y);
            }
            foreach (Ellipse i in removeThis)
            {
                MyCanvas.Children.Remove(i);
            }

            growthRate = .6;
            spawnRate = 60;
            lastScore = score;
            score = 0;
            currentRate = 5;
            health = 350;
            removeThis.Clear();
            gameTimer.Start();
        }
    }
}
