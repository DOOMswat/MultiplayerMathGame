using MathGame.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
namespace MathGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random _rand = new Random();
        GameManager gameManager;
        bool isStarted;
        private Timer gameTimer;
        private int timeRemainingInSeconds = 60;
        string userAnswer = null;
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            gameManager = new GameManager(Difficulty.Easy, Operator.Multiply);
            gameManager.RestartQuestion();
            lblQuestion.Content = "Click Generate";
            isStarted = false;
            lblMessage.Content = String.Empty;
            lblMessage.Foreground = new SolidColorBrush(Colors.White);
            btnGenerate.IsEnabled = true;
            userAnswer = null;
            btnCheck.IsEnabled = false;
            pbGuess.Value = gameManager.Guesses;
            gameTimer = new Timer(1000); // Timer ticks every second
            gameTimer.Elapsed += GameTimer_Tick;
            gameTimer.Enabled = false;
            ToggleButtonVisibility(isStarted);
            UpdateDifficultySettings();
            UpdateOperatorSettings();
        }

        private void ToggleButtonVisibility(bool visible)
        {
            //Operator buttons
            btnMultiply.IsEnabled = visible;
            btnDivide.IsEnabled = visible;
            btnSubstract.IsEnabled = visible;
            btnAdd.IsEnabled = visible;
            //Difficulty Buttons
            btnDiffEasy.IsEnabled = !visible;
            btnDiffMedium.IsEnabled = !visible;
            btnDiffHard.IsEnabled = !visible;
            //Answer Buttons
            btnAnswer1.IsEnabled = visible;
            btnAnswer2.IsEnabled = visible;
            btnAnswer3.IsEnabled = visible;
            btnAnswer4.IsEnabled = visible;
        }

        private void UpdateOperatorSettings()
        {
            lblOperatorChosen.Content = gameManager.Operator.ToString();
        }

        private void UpdateDifficultySettings()
        {
            lblDifficultyChosen.Content = gameManager.Difficulty.ToString();
            lblLives.Content = $"Lives: {gameManager.Trials.ToString()}";
            pbGuess.Maximum = gameManager.MaxGuesses;
        }

        private void btnDiffEasy_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button btn = (Button)sender;
                gameManager.Difficulty = (Difficulty)Int32.Parse(btn.Uid);
                UpdateDifficultySettings();
            }
        }

        private void btnMultiply_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button btn = (Button)sender;
                gameManager.Operator = (Operator)Int32.Parse(btn.Uid);
                lblQuestion.Content = gameManager.DisplayQuestion();
                UpdateOperatorSettings();
                GenerateUserQuestion();
            }
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            isStarted = true;
            GenerateUserQuestion();
            ToggleButtonVisibility(isStarted);
        }

        private void GenerateUserQuestion()
        {
            gameManager.GenerateQuestion();
            lblQuestion.Content = gameManager.DisplayQuestion();
            int correctAnswerIndex = _rand.Next(4);
            int correctAnswer = gameManager.CalculateResult();

            for (int i = 0; i < 4; i++)
            {
                if (i == correctAnswerIndex)
                {
                    switch (i)
                    {
                        case 0:
                            btnAnswer1.Content = correctAnswer.ToString();
                            break;
                        case 1:
                            btnAnswer2.Content = correctAnswer.ToString();
                            break;
                        case 2:
                            btnAnswer3.Content = correctAnswer.ToString();
                            break;
                        case 3:
                            btnAnswer4.Content = correctAnswer.ToString();
                            break;
                    }
                }
                else
                {
                    int randomNumber = _rand.Next(100);
                    switch (i)
                    {
                        case 0:
                            btnAnswer1.Content = randomNumber.ToString();
                            break;
                        case 1:
                            btnAnswer2.Content = (correctAnswer + 5).ToString();
                            break;
                        case 2:
                            btnAnswer3.Content = (correctAnswer + 1).ToString();
                            break;
                        case 3:
                            btnAnswer4.Content = randomNumber.ToString();
                            break;
                    }
                }
            }
            startTimer();
        }


        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            Init();
        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            if (gameManager.CheckAnswer(userAnswer))
            {
                CorrectAnswer();
                btnAnswer1.BorderBrush = new SolidColorBrush(Colors.Black);
                btnAnswer1.BorderThickness = new Thickness(1, 1, 1, 1);
                btnAnswer2.BorderBrush = new SolidColorBrush(Colors.Black);
                btnAnswer2.BorderThickness = new Thickness(1, 1, 1, 1);
                btnAnswer3.BorderBrush = new SolidColorBrush(Colors.Black);
                btnAnswer3.BorderThickness = new Thickness(1, 1, 1, 1);
                btnAnswer4.BorderBrush = new SolidColorBrush(Colors.Black);
                btnAnswer4.BorderThickness = new Thickness(1, 1, 1, 1);
                userAnswer = null;
            }
            else
            {
                WrongAnswer();
                btnAnswer1.BorderBrush = new SolidColorBrush(Colors.Black);
                btnAnswer1.BorderThickness = new Thickness(1, 1, 1, 1);
                btnAnswer2.BorderBrush = new SolidColorBrush(Colors.Black);
                btnAnswer2.BorderThickness = new Thickness(1, 1, 1, 1);
                btnAnswer3.BorderBrush = new SolidColorBrush(Colors.Black);
                btnAnswer3.BorderThickness = new Thickness(1, 1, 1, 1);
                btnAnswer4.BorderBrush = new SolidColorBrush(Colors.Black);
                btnAnswer4.BorderThickness = new Thickness(1, 1, 1, 1);
                userAnswer = null;
            }
        }

        private void WrongAnswer()
        {
            if (gameManager.Trials > 0)
            {
                gameManager.RemoveTrial();
                GenerateUserQuestion();
                lblLives.Content = $"Lives: {gameManager.Trials}";
                userAnswer = null;
            }

            if (gameManager.Trials == 0)
            {
                lblMessage.Content = "You Lost";
                lblMessage.Foreground = new SolidColorBrush(Colors.Red);
                btnGenerate.IsEnabled = false;
                stopTimer();
            }
        }

        private void CorrectAnswer()
        {
            if (gameManager.Guesses != gameManager.MaxGuesses - 1)
            {
                gameManager.AddGuess();
                GenerateUserQuestion();
                pbGuess.Value = gameManager.Guesses;

            }
            else
            {
                pbGuess.Value++;
                lblMessage.Content = "You Won";
                lblMessage.Foreground = new SolidColorBrush(Colors.Green);
                btnGenerate.IsEnabled = false;
                stopTimer();
            }
        }

        private void txtInputted()
        {
            if (isStarted) btnCheck.IsEnabled = (userAnswer.Length > 0);
        }

        private void btnAnswer1_Click(object sender, RoutedEventArgs e)
        {
            btnAnswer1.BorderBrush = new SolidColorBrush(Colors.Blue);
            btnAnswer1.BorderThickness = new Thickness(2, 2, 2, 2);


            //Other
            btnAnswer2.BorderBrush = new SolidColorBrush(Colors.Black);
            btnAnswer2.BorderThickness = new Thickness(1, 1, 1, 1);
            btnAnswer3.BorderBrush = new SolidColorBrush(Colors.Black);
            btnAnswer3.BorderThickness = new Thickness(1, 1, 1, 1);
            btnAnswer4.BorderBrush = new SolidColorBrush(Colors.Black);
            btnAnswer4.BorderThickness = new Thickness(1, 1, 1, 1);

            userAnswer = btnAnswer1.Content.ToString();
            txtInputted();


        }

        private void btnAnswer2_Click(object sender, RoutedEventArgs e)
        {
            btnAnswer2.BorderBrush = new SolidColorBrush(Colors.Blue);
            btnAnswer2.BorderThickness = new Thickness(2, 2, 2, 2);


            //Other
            btnAnswer1.BorderBrush = new SolidColorBrush(Colors.Black);
            btnAnswer1.BorderThickness = new Thickness(1, 1, 1, 1);
            btnAnswer3.BorderBrush = new SolidColorBrush(Colors.Black);
            btnAnswer3.BorderThickness = new Thickness(1, 1, 1, 1);
            btnAnswer4.BorderBrush = new SolidColorBrush(Colors.Black);
            btnAnswer4.BorderThickness = new Thickness(1, 1, 1, 1);

            userAnswer = btnAnswer2.Content.ToString();
            txtInputted();
        }

        private void btnAnswer3_Click(object sender, RoutedEventArgs e)
        {
            btnAnswer3.BorderBrush = new SolidColorBrush(Colors.Blue);
            btnAnswer3.BorderThickness = new Thickness(2, 2, 2, 2);


            //Other
            btnAnswer2.BorderBrush = new SolidColorBrush(Colors.Black);
            btnAnswer2.BorderThickness = new Thickness(1, 1, 1, 1);
            btnAnswer1.BorderBrush = new SolidColorBrush(Colors.Black);
            btnAnswer1.BorderThickness = new Thickness(1, 1, 1, 1);
            btnAnswer4.BorderBrush = new SolidColorBrush(Colors.Black);
            btnAnswer4.BorderThickness = new Thickness(1, 1, 1, 1);

            userAnswer = btnAnswer3.Content.ToString();
            txtInputted();
        }

        private void btnAnswer4_Click(object sender, RoutedEventArgs e)
        {
            btnAnswer4.BorderBrush = new SolidColorBrush(Colors.Blue);
            btnAnswer4.BorderThickness = new Thickness(2, 2, 2, 2);


            //Other
            btnAnswer2.BorderBrush = new SolidColorBrush(Colors.Black);
            btnAnswer2.BorderThickness = new Thickness(1, 1, 1, 1);
            btnAnswer3.BorderBrush = new SolidColorBrush(Colors.Black);
            btnAnswer3.BorderThickness = new Thickness(1, 1, 1, 1);
            btnAnswer1.BorderBrush = new SolidColorBrush(Colors.Black);
            btnAnswer1.BorderThickness = new Thickness(1, 1, 1, 1);

            userAnswer = btnAnswer4.Content.ToString();
            txtInputted();
        }




        private void startTimer()
        {
            gameTimer.Start();
            UpdateTimerLabel();
        }
        private void stopTimer()
        {
            gameTimer.Stop();
            timeRemainingInSeconds = 60;
        }


        private void GameTimer_Tick(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                timeRemainingInSeconds--;
                if (timeRemainingInSeconds <= 0)
                {
                    stopTimer();
                    TimeExpired();
                }
                UpdateTimerLabel();
            });
        }

        private void UpdateTimerLabel()
        {
            int minutes = timeRemainingInSeconds / 60;
            int seconds = timeRemainingInSeconds % 60;
            lblTimer.Content = $"Timer: {minutes}:{seconds:D2}";
        }

        private void TimeExpired()
        {
            lblMessage.Content = "Time's up! You Lost";
            lblMessage.Foreground = new SolidColorBrush(Colors.Red);
            btnGenerate.IsEnabled = false;

        }


        private void btnMultiplayer_Click(object sender, RoutedEventArgs e)
        {
            Login Login = new Login();
            Login.Show();
            this.Close();
        }
    }
}

 

