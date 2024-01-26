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
using System.Windows.Shapes;
using MathGame.Classes;
using MySql.Data.MySqlClient;
namespace MathGame
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string connStr = "server=ND-COMPSCI;" + "user=TL_S2101550;" + "database=TL_S2101550_leaderboard;" + "port=3306;" + "password=Notre260605";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string query = "SELECT * FROM client WHERE Username = @Username AND Password = SHA(@Password)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", txtUsername.Text.ToUpper());
                cmd.Parameters.AddWithValue("@Password", txtPassword.Text);//anti_Sql stuff

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        GameManagerM.username = txtUsername.Text;
                        Multiplayer multiplayer = new Multiplayer();
                        multiplayer.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password");
                    }

                }

            }

        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string connStr = "server=ND-COMPSCI;" + "user=TL_S2101550;" + "database=TL_S2101550_leaderboard;" + "port=3306;" + "password=Notre260605";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string checkQuery = "SELECT COUNT(*) FROM client WHERE Username = @Username";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@Username", txtRegUsername.Text); //ANTI-SQLINJECTION STUFF
                        int userCount = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (userCount > 0)
                        {
                            MessageBox.Show("Username already exists. Choose a different username.");
                            return;
                        }
                        else
                        {
                            string insertUserQuery = "INSERT INTO client (Username, Password)" +
                            "VALUES (@Username, SHA(@password))";
                            using (MySqlCommand insertCustomerCmd = new MySqlCommand(insertUserQuery, conn)) //ANTI-SQLINJECTION STUFF
                            {
                                insertCustomerCmd.Parameters.AddWithValue("@Username", txtRegUsername.Text.ToUpper());
                                insertCustomerCmd.Parameters.AddWithValue("@password", txtRegPassword.Text);
                                insertCustomerCmd.ExecuteNonQuery();
                                MessageBox.Show("User successfully created");

                            }

                        }

                    }

                }
            }
            catch (Exception ex) { }

        }
    }
}





//CREATE TABLE client (
//    userID INT AUTO_INCREMENT PRIMARY KEY,
//    Username VARCHAR(40),
//    Password VARCHAR(255),
//	highestScore INT
//);