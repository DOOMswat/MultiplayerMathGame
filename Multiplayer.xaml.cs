using MathGame.Classes;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Tls;

namespace MathGame
{
    /// <summary>
    /// Interaction logic for Multiplayer.xaml
    /// </summary>
    public partial class Multiplayer : Window
    {
        private Random _rand = new Random();
        GameManagerM GameManagerM;
        bool isStarted;
        private Timer gameTimer;
        private int timeRemainingInSeconds = 60;
        string userAnswer = null;
        public Multiplayer()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            GameManagerM = new GameManagerM(DifficultyM.Easy, OperatorM.Multiply);
            GameManagerM.RestartQuestion();
            lblQuestion.Content = "Click Generate";
            isStarted = false;
            lblMessage.Content = String.Empty;
            lblMessage.Foreground = new SolidColorBrush(Colors.White);
            btnGenerate.IsEnabled = true;
            userAnswer = null;
            btnCheck.IsEnabled = false;
            gameTimer = new Timer(1000); // Timer ticks every second
            gameTimer.Elapsed += GameTimer_Tick;
            gameTimer.Enabled = false;
            UpdateLeaderboard();
            ToggleButtonVisibility(isStarted);
        }

        private void ToggleButtonVisibility(bool visible)
        {

            //Answer Buttons
            btnAnswer1.IsEnabled = visible;
            btnAnswer2.IsEnabled = visible;
            btnAnswer3.IsEnabled = visible;
            btnAnswer4.IsEnabled = visible;
        }

        private void btnDiffEasy_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button btn = (Button)sender;
                GameManagerM.DifficultyM = (DifficultyM)Int32.Parse(btn.Uid);
            }
        }

        private void btnMultiply_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button btn = (Button)sender;
                GameManagerM.OperatorM = (OperatorM)Int32.Parse(btn.Uid);
                lblQuestion.Content = GameManagerM.DisplayQuestion();
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
            GameManagerM.GenerateQuestion();
            lblQuestion.Content = GameManagerM.DisplayQuestion();
            int correctAnswerIndex = _rand.Next(4);
            int correctAnswer = GameManagerM.CalculateResult();

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
            if (GameManagerM.CheckAnswer(userAnswer))
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
            if (GameManagerM.Trials > 0)
            {
                GameManagerM.RemoveTrial();
                GenerateUserQuestion();
                lblLives.Content = $"Lives: {GameManagerM.Trials}";
                userAnswer = null;
            }

            if (GameManagerM.Trials == 0)
            {
                stopTimer();
                lblMessage.Content = "You Lost";
                lblMessage.Foreground = new SolidColorBrush(Colors.Red);
                btnGenerate.IsEnabled = false;
                
            }
        }

        private void CorrectAnswer()
        {
            if (GameManagerM.Guesses != GameManagerM.MaxGuesses - 1)
            {

                GameManagerM.AddPoints();
                lblPoint.Content = $"Points: {GameManagerM.Points}";
                GameManagerM.AddGuess();
                GenerateUserQuestion();

            }
            else
            {
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

        private void getScore()
        {
            string connStr = "server=ND-COMPSCI;" + "user=TL_S2101550;" + "database=TL_S2101550_leaderboard;" + "port=3306;" + "password=Notre260605";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string getScore = $"SELECT highestScore FROM client WHERE username = {GameManagerM.username}";
                    using (MySqlCommand cmd = new MySqlCommand(getScore))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            listBoxLeaderBoard.Items.Clear();

                            while (reader.Read())
                            {
                                string score = reader["highestScore"].ToString();
                                int highestScore = Int32.Parse(score);
                                GameManagerM.highestScore = highestScore;

                            }
                        }

                    }
                }
            }
            catch (Exception ex) { }
        }
        private void stopTimer()
        {
            lblLives.Content = $"Lives: 3";
            gameTimer.Stop();
            timeRemainingInSeconds = 60;

            string connStr = "server=ND-COMPSCI;" + "user=TL_S2101550;" + "database=TL_S2101550_leaderboard;" + "port=3306;" + "password=Notre260605";
            try
            {
                getScore();
                if (GameManagerM.highestScore > GameManagerM.Points)
                {
                    using (MySqlConnection conn = new MySqlConnection(connStr))
                    {
                        conn.Open();

                        string updateQuery = "UPDATE client SET highestScore = @HighestScore WHERE Username = @Username";
                        using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                        {
                            updateCmd.Parameters.AddWithValue("@Username", GameManagerM.username);
                            updateCmd.Parameters.AddWithValue("@HighestScore", GameManagerM.Points);
                            updateCmd.ExecuteNonQuery();
                        }
                        lblPoint.Content = $"Point: 0";
                        UpdateLeaderboard();
                    }
                }
                
            }
            catch (Exception ex) { MessageBox.Show("Error" + ex); }
        }
        private void UpdateLeaderboard()
        {
            string connStr = "server=ND-COMPSCI;" + "user=TL_S2101550;" + "database=TL_S2101550_leaderboard;" + "port=3306;" + "password=Notre260605";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string leaderboardQuery = "SELECT Username, highestScore FROM client ORDER BY highestScore DESC";

                    using (MySqlCommand leaderboardCmd = new MySqlCommand(leaderboardQuery, conn))
                    {
                        using (MySqlDataReader reader = leaderboardCmd.ExecuteReader())
                        {
                            listBoxLeaderBoard.Items.Clear();

                            while (reader.Read())
                            {
                                string username = reader["Username"].ToString();
                                int highestScore = Convert.ToInt32(reader["highestScore"]);
                                ListBoxItem item = new ListBoxItem();
                                item.Content = $"{username} - {highestScore} points";
                                listBoxLeaderBoard.Items.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex){}
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
            lblMessage.Content = "Time's up!";
            lblMessage.Foreground = new SolidColorBrush(Colors.Red);
            btnGenerate.IsEnabled = false;

        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            GameManagerM.username = null;
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}



