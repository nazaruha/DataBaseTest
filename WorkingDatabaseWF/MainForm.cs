using WorkingDatabaseWF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bogus;
using System.Diagnostics;

namespace WorkingDatabaseWF
{
    public partial class MainForm : Form
    {
        SqlConnection con;
        string dirSql = "SqlTables";
        string dirScript = "Scripts";
        string dbName = "UsersRolesCities";
        string conSTR;
        string connectionSTR;
        //List<User> users = new List<User>();

        public MainForm()
        {
            InitializeComponent();
            GetTableData(dbName);
        }

        private void btn_GenerateTable_Click(object sender, EventArgs e)
        {
            conSTR = "Data Source=.;Integrated Security=True;";
            connectionSTR = $"{conSTR}Initial Catalog={dbName}";
            con = new SqlConnection(connectionSTR);
            con.Open();

            //CreateTabels(); 
            GenerateRegions();
        }

        private void CreateTabels()
        {
            ExecuteCommandFromFile("tblRegions.sql");
            ExecuteCommandFromFile("tblCities.sql");
            ExecuteCommandFromFile("tblUsers.sql");
            ExecuteCommandFromFile("tblRoles.sql");
            ExecuteCommandFromFile("tblUserRoles.sql");
            ExecuteCommandFromFile("tblUserAdresses.sql");
        }

        private void ExecuteCommandFromFile(string file)
        {
            string sql = ReadSqlFile(file);
            SqlCommand command = con.CreateCommand();
            command.CommandText = sql;
            command.ExecuteNonQuery(); //Sends command to DB
        }
        private string ReadSqlFile(string file)
        {
            string sql = File.ReadAllText($"{dirSql}\\{file}");
            return sql;
        }

        private void GetTableData(string dbName)
        {
            if (con == null)
            {
                conSTR = "Data Source=.;Integrated Security=True;";
                connectionSTR = $"{conSTR}Initial Catalog={dbName}";
                con = new SqlConnection(connectionSTR);
                con.Open();
            }
            SqlCommand cmd = con.CreateCommand();

            string script = File.ReadAllText($"{dirScript}\\viewUserAddresses.sql");
            cmd.CommandText = script;


            List<User> users = new List<User>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    User user = new User() { Id = Int32.Parse(reader["Id"].ToString()), Name = reader["Name"].ToString(), Region = reader["Region"].ToString(), City = reader["City"].ToString(), Street = reader["Street"].ToString(), HouseNumber = reader["HouseNumber"].ToString() };
                    users.Add(user);
                }
            }
            dgUsers.DataSource = users;
        }

        private void GenerateUsers()
        {
            int count = 10;
            Faker<User> faker = new Faker<User>()
                .RuleFor(u => u.Name, (f, u) => f.Name.FullName());

            List<User> users = new List<User>();
            for (int i = 0; i < count; i++)
            {
                users.Add(faker.Generate());
            }

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn(nameof(User.Id)));
            dt.Columns.Add(new DataColumn(nameof(User.Name)));

            for (int i = 0; i < count; i++)
            {
                DataRow row = dt.NewRow();
                row[nameof(User.Id)] = users[i].Id;
                row[nameof(User.Name)] = users[i].Name;
                dt.Rows.Add(row);
            }
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
            {
                bulkCopy.DestinationTableName = "tblUsers";
                bulkCopy.WriteToServer(dt);
            }
        }

        private void GenerateRegions() // remove repeatings
        {
            int count = 10;
            Faker<Regions> faker = new Faker<Regions>()
                .RuleFor(r => r.Name, (f, r) => f.Address.County());

            List<Regions> regions = new List<Regions>();
            for (int i = 0; i < count; i++)
            {
                var region = faker.Generate();
                regions.Add(region);
            }

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn(nameof(Regions.Id))); 
            dt.Columns.Add(new DataColumn(nameof(Regions.Name)));

            Random rand = new Random();
            for (int i = 0; i < count; i++)
            {
                //int r = rand.Next(0, count);
                //var region = regions[r];
                DataRow row = dt.NewRow();
                row[nameof(Regions.Id)] = regions[i].Id;
                row[nameof(Regions.Name)] = regions[i].Name;
                dt.Rows.Add(row);
            }
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
            {
                bulkCopy.DestinationTableName = "tblRegions";
                bulkCopy.WriteToServer(dt);
            }
        }

        private void GenerateCities()
        {

        }

        private void GenerateUserAddresses()
        {

        }


    }
}
