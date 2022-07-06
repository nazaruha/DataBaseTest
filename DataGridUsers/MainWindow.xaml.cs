using DataGridUsers.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
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

namespace DataGridUsers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string database { get; set; } = "kaka";
        private string table { get; set; } = "jjj";
        private string connectionDB { get; set; }
        private SqlConnection con { get; set; }
        private SqlCommand cmd { get; set; }
        private int MaxUsersInPage { get; set; } = 20;
        private int currentPage { get; set; } = 1;
        private int pages { get; set; }
        private bool flag { get; set; } = false;

        List<User> users = new List<User>();        

        public MainWindow()
        {
            InitializeComponent();
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration configuration = builder.Build();
            connectionDB = configuration.GetConnectionString("DefaultConnection");
            con = new SqlConnection(connectionDB + $"Initial Catalog={database}");
            con.Open();
            cmd = con.CreateCommand(); // creating command

            CountPages();
            GetUsers();
            dgUsers.ItemsSource = users;

            // OUTPUT ALL USERS
            //cmd.CommandText = "Select Email, FirstName, LastName, Phone From jjj";
            //SqlDataAdapter sda = new SqlDataAdapter(cmd);
            //DataTable dt = new DataTable("jjj");
            //sda.Fill(dt);
            //dgUsers.ItemsSource = dt.DefaultView;


        }

        private void GetUsers(int start = 0, int end = 21)
        {
            cmd.CommandText = $"SELECT * FROM ( SELECT *, ROW_NUMBER() OVER (ORDER BY Id) AS row FROM {table})" +
                $"a WHERE row > {start} AND row < {end}";
            users.Clear();
            Stopwatch StopWatch = new Stopwatch();
            StopWatch.Start();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    User user = new User();
                    user.Id = Int32.Parse(reader["Id"].ToString());
                    user.Email = reader["Email"].ToString();
                    user.FirstName = reader["FirstName"].ToString();
                    user.LastName = reader["LastName"].ToString();
                    user.Phone = reader["Phone"].ToString();
                    users.Add(user);
                }
            }
            StopWatch.Stop();
            TimeSpan ts = StopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            GenerationTime_txt.Text = $"Time elapsed: {elapsedTime}";
        }

        private void GetFoundUsers(int start = 0)
        {
            dgUsers.ItemsSource = null;
            if (users.Count <= MaxUsersInPage)
            {
                dgUsers.ItemsSource = users;
                pages = 1;
            }
            else
                dgUsers.ItemsSource = users.GetRange(start, MaxUsersInPage);
            Pages_txt.Text = $"{currentPage} of {pages}";
            flag = true;
        }

        private void CountPages()
        {
            cmd.CommandText = $"SELECT COUNT(*) FROM {table}";
            string reader = cmd.ExecuteScalar().ToString();
            pages = Int32.Parse(reader) / MaxUsersInPage;
            Pages_txt.Text = $"{currentPage} of {pages}";
        }

        private void Prev_btn_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage == 1) return;
            currentPage--;
            int end = MaxUsersInPage * currentPage;
            int start = end - MaxUsersInPage;
            if (!flag)
                GetUsers(start - 1, end + 1);
            else
            {
                GetFoundUsers(start);
                return;
            }
            Pages_txt.Text = $"{currentPage} of {pages}";
            dgUsers.ItemsSource = null;
            dgUsers.ItemsSource = users;

        }

        private void Next_btn_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage == pages) return;
            currentPage++;
            int end = MaxUsersInPage * currentPage;
            int start = end - MaxUsersInPage;
            if (!flag)
                GetUsers(start, end + 1);
            else
            {
                GetFoundUsers(start);
                return;
            }
            Pages_txt.Text = $"{currentPage} of {pages}";
            dgUsers.ItemsSource = null;
            dgUsers.ItemsSource = users;
        }

        private void Search_btn_Click(object sender, RoutedEventArgs e)
        {
            SearchWindow searchWindow = new SearchWindow(table, cmd);
            searchWindow.ShowDialog();
            if (searchWindow.users.Count == 0) return;
            users = searchWindow.users;
            pages = searchWindow.usersSize / MaxUsersInPage;
            currentPage = 1;
            GetFoundUsers();
        }

        private void Reset_btn_Click(object sender, RoutedEventArgs e)
        {
            GetUsers();
            dgUsers.ItemsSource = null;
            dgUsers.ItemsSource = users;
            currentPage = 1;
            CountPages();
            Pages_txt.Text = $"{currentPage} of {pages}";
            flag = false;
        }
    }
}
