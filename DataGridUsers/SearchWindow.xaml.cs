using DataGridUsers.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DataGridUsers
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        private string table { get; set; }
        public List<User> users { get; set; } = new List<User>();
        public int usersSize { get; set; }
        private SqlCommand cmd { get; set; }
        private string FirstName { get; set; }
        private string LastName { get; set; }
        public SearchWindow(string table, SqlCommand cmd)
        {
            InitializeComponent();
            this.table = table;
            this.cmd = cmd;

            
        }

        private void RadioButtons_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as RadioButton).Name == "Email_rb")
            {
                Email_txt.IsReadOnly = false;
                FirstName_txt.Clear();
                FirstName_txt.IsReadOnly = true;
                LastName_txt.Clear();
                LastName_txt.IsReadOnly = true;
                FullName_txt.Clear();
                FullName_txt.IsReadOnly = true;
                Phone_txt.Clear();
                Phone_txt.IsReadOnly = true;
            }
            else if ((sender as RadioButton).Name == "FirstName_rb")
            {
                FirstName_txt.IsReadOnly = false;
                Email_txt.Clear();
                Email_txt.IsReadOnly = true;
                LastName_txt.Clear();
                LastName_txt.IsReadOnly = true;
                FullName_txt.Clear();
                FullName_txt.IsReadOnly = true;
                Phone_txt.Clear();
                Phone_txt.IsReadOnly = true;
            }
            else if ((sender as RadioButton).Name == "LastName_rb")
            {
                LastName_txt.IsReadOnly = false;
                Email_txt.Clear();
                Email_txt.IsReadOnly = true;
                FirstName_txt.Clear();
                FirstName_txt.IsReadOnly = true;
                FullName_txt.Clear();
                FullName_txt.IsReadOnly = true;
                Phone_txt.Clear();
                Phone_txt.IsReadOnly = true;
            }
            else if ((sender as RadioButton).Name == "FullName_rb")
            {
                FullName_txt.IsReadOnly = false;
                Email_txt.Clear();
                Email_txt.IsReadOnly = true;
                FirstName_txt.Clear();
                FirstName_txt.IsReadOnly = true;
                LastName_txt.Clear();
                LastName_txt.IsReadOnly = true;
                Phone_txt.Clear();
                Phone_txt.IsReadOnly = true;
            }
            else if ((sender as RadioButton).Name == "Phone_rb")
            {
                Phone_txt.IsReadOnly = false;
                Email_txt.Clear();
                Email_txt.IsReadOnly = true;
                FirstName_txt.Clear();
                FirstName_txt.IsReadOnly = true;
                LastName_txt.Clear();
                LastName_txt.IsReadOnly = true;
                FullName_txt.Clear();
                FullName_txt.IsReadOnly = true;
            }
        }

        private void GetUser(SqlDataReader reader)
        {
            User user = new User();
            user.Id = Int32.Parse(reader["Id"].ToString());
            user.Email = reader["Email"].ToString();
            user.FirstName = reader["FirstName"].ToString();
            user.LastName = reader["LastName"].ToString();
            user.Phone = reader["Phone"].ToString();
            users.Add(user);
        }

        //HOW TO FIND USERS
        //Select * from jjj
        //WHERE FirstName Like '%Влад%' And LastName = 'Щербак'

        private void FindUsers(string where)
        {
            switch (where)
            {
                case "Email":
                    cmd.CommandText = $"SELECT * FROM {table} " +
                    $"WHERE Email = '{Email_txt.Text}'";
                    break;
                case "FirstName":
                    cmd.CommandText = $"SELECT * FROM {table} " +
                    $"WHERE FirstName LIKE '%{FirstName_txt.Text}%'";
                    break;
                case "LastName":
                    cmd.CommandText = $"SELECT * FROM {table} " +
                    $"WHERE LastName LIKE '%{LastName_txt.Text}%'";
                    break;
                case "FullName":
                    cmd.CommandText = $"SELECT * FROM {table} " +
                    $"WHERE FirstName LIKE '%{FirstName}%' AND LastName LIKE '%{LastName}%'";
                    break;
                case "Phone":
                    cmd.CommandText = $"SELECT * FROM {table} " +
                    $"WHERE Phone LIKE '%{Phone_txt.Text}%'";
                    break;
                default:
                    return;
            }
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    GetUser(reader);
                }
            }
        }


        private void FullName_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex regex = new Regex(@"^\s*[A-ZА-ЯІ][a-zа-яі]*\s*[A-ZА-ЯІ][a-zа-яі]*\s*$");
            if (regex.IsMatch(FullName_txt.Text))
            {
                FullName_txt.BorderBrush = Brushes.White;
                Regex split_rgx = new Regex(@"\s*[A-ZА-ЯІ][a-zа-яі]*\s*");
                MatchCollection match = split_rgx.Matches(FullName_txt.Text);

                FirstName = correct_PartOfSNP(match[0]);
                LastName = correct_PartOfSNP(match[1]);
            }
            else
            {
                FullName_txt.BorderBrush = Brushes.Red;
            }
        }

        private string correct_PartOfSNP(Match item)
        {
            string txt;
            Regex space_rgx = new Regex(@"\s+");
            txt = item.ToString();
            txt = space_rgx.Replace(txt, "");
            return txt;
        }

        private void Search_btn_Click(object sender, RoutedEventArgs e)
        {
            users.Clear();
            Stopwatch StopWatch = new Stopwatch();
            StopWatch.Start();
            if (Email_rb.IsChecked == true)
            {
                FindUsers("Email");
            }
            else if (FirstName_rb.IsChecked == true)
            {
                FindUsers("FirstName");
            }
            else if (LastName_rb.IsChecked == true)
            {
                FindUsers("LastName");
            }
            else if (FullName_rb.IsChecked == true)
            {
                if (FullName_txt.BorderBrush == Brushes.Red) return;
                FindUsers("FullName");
            }
            else if (Phone_rb.IsChecked == true)
            {
                FindUsers("Phone");
            }
            StopWatch.Stop();
            TimeSpan ts = StopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            MessageBox.Show($"Time elapsed: {elapsedTime}", "Searching Time", MessageBoxButton.OK);
            usersSize = users.Count;
        }

        
    }
}
