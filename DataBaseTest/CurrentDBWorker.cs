using System;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Collections.Generic;
using Bogus;
using DataBaseTest.Models;
using System.Data.Odbc;
using System.Diagnostics;
using System.Data;

namespace DataBaseTest
{
    public class CurrentDBWorker : DBWorker
    {
        private string database { get; set; }
        private SqlConnection con { get; set; }
        private SqlCommand cmd { get; set; }
        private List<string> tables { get; set; } = new List<string>();

        public CurrentDBWorker(SqlConnection con, SqlCommand cmd, string database)
        {
            this.con = con;
            this.cmd = cmd;
            this.database = database;
            ListOfTables();
        }

        private void OutputMenu()
        {
            Console.WriteLine($"\tYou're in {database}");
            Console.WriteLine("\t0. Exit");
            Console.WriteLine("\t1. Create table");
            Console.WriteLine("\t2. Create tables with multiple connection");
            Console.WriteLine("\t3. Delete table");
            Console.WriteLine("\t4. Show tables");
            Console.WriteLine("\t5. Fill table");
            Console.WriteLine("\t6. Input table by random data");
            Console.WriteLine("\t7. Delete item (Only works by Id right now)");
            Console.WriteLine("\t8. Update item by Id");
            Console.WriteLine("\t9. Print table's users");
            Console.Write("\tInput: ");
        }

        public void MainMenu()
        {
            int choice = -1;
            while (choice != 0)
            {
                OutputMenu();
                choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 0:
                        Console.WriteLine();
                        break;
                    case 1:
                        CreateTable();
                        break;
                    case 2:
                        CreateTablesMultCon();
                        break;
                    case 3:
                        DeleteTable();
                        break;
                    case 4:
                        ShowTables();
                        break;
                    case 5:
                        FillTable();
                        break;
                    case 6:
                        BogusTableInput();
                        break;
                    case 7:
                        DeleteItem();
                        break;
                    case 8:
                        UpdateItem();
                        break;
                    case 9:
                        PrintUsers();
                        break;
                    default:
                        Console.WriteLine("Invalid command.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        private void CreateTable()
        {
            Console.Write("Input table's name: ");
            string table = Console.ReadLine();
            try
            {
                cmd.CommandText = $"CREATE TABLE {table} " +
                    "( " +
                    "Id int IDENTITY PRIMARY KEY NOT NULL," +
                    "Email nvarchar(50) NOT NULL," +
                    "FirstName nvarchar(50)," +
                    "LastName nvarchar(50)," +
                    "Phone nvarchar(30)," +
                    " );";
                cmd.ExecuteNonQuery();
                tables.Add(table);
                Console.WriteLine("Table is successfully created\n");
            }
            catch
            {
                Console.WriteLine("This table is already exists");
                Console.ReadLine();
            }
        }

        private void CreateTablesMultCon()
        {
            string dirSql = "SqlQueryMultCon";
            string[] tables = { "tblUsers.sql", "tblRoles.sql", "tblUsersAndRoles.sql" };
            foreach (var tableName in tables)
            {
                string sql = File.ReadAllText($"{dirSql}\\{tableName}");
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                Console.WriteLine($"Create table {tableName}");
            }
        }
        private void DeleteTable()
        {
            Console.Write("Input table's name: ");
            string table = Console.ReadLine();
            try
            {
                cmd.CommandText = $"DROP TABLE {table}";
                cmd.ExecuteNonQuery();
                tables.Remove(table);
                Console.WriteLine("Table is successfully deleted\n");
            }
            catch 
            {
                Console.WriteLine("This table doesn't exist");
                Console.ReadLine();
            }
        }

        private void ShowTables()
        {
            cmd.CommandText = $"SELECT name FROM {database}.sys.tables";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine(reader["name"]);
                }
            }
            Console.WriteLine();
        }

        private void FillTable()
        {
            Console.Write("Choose table: ");
            string table = Console.ReadLine();
            if (!IsTableExist(table)) return;

            Console.Write("How many items would you like to add? ");
            int count = int.Parse(Console.ReadLine());

            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"User #{i + 1}");

                Console.Write("Email: ");
                string Email = Console.ReadLine();

                Console.Write("Frist name: ");
                string FirstName = Console.ReadLine();

                Console.Write("Last name: ");
                string LastName = Console.ReadLine();

                try
                {
                    cmd.CommandText = $"INSERT INTO {table}" +
                    "(Email, FirstName, LastName)" +
                    $"VALUES ('{Email}', N'{FirstName}', N'{LastName}')";
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    Console.WriteLine("This table doesn't exist");
                    Console.ReadLine();
                    return;
                }
            }
            Console.WriteLine("Items are successfully added\n");
        }

        private void BogusTableInput()
        {
            Console.Write("Choose table: ");
            string table = Console.ReadLine();
            if (!IsTableExist(table)) return;

            Console.Write("How many would you like to add? ");
            int count = int.Parse(Console.ReadLine());

            var faker = new Faker<User>("uk")
                    .RuleFor(u => u.Email, (f, u) => f.Internet.Email())
                    .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
                    .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
                    .RuleFor(u => u.Phone, (f, u) => f.Phone.PhoneNumber());

            Stopwatch StopWatch = new Stopwatch();
            StopWatch.Start();
            List<User> users = new List<User>();
            for (int i = 0; i < count; i++)
            {
                var user = faker.Generate();
                users.Add(user);
            }

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn(nameof(User.Id)));
            dt.Columns.Add(new DataColumn(nameof(User.Email)));
            dt.Columns.Add(new DataColumn(nameof(User.FirstName)));
            dt.Columns.Add(new DataColumn(nameof(User.LastName)));
            dt.Columns.Add(new DataColumn(nameof(User.Phone)));

            Random random = new Random();
            for (int i = 0; i < count; i++)
            {
                int r = random.Next(0, 100);
                var user = users[r];
                DataRow row = dt.NewRow();
                row[nameof(User.Id)] = user.Id;
                row[nameof(User.Email)] = user.Email;
                row[nameof(User.FirstName)] = user.FirstName;
                row[nameof(User.LastName)] = user.LastName;
                row[nameof(User.Phone)] = user.Phone;
                dt.Rows.Add(row);
            }
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
            {
                bulkCopy.DestinationTableName = table;
                bulkCopy.WriteToServer(dt);
            }
            StopWatch.Stop();
            TimeSpan ts = StopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.WriteLine("Items are successfully added\n");
        }

        private void DeleteItem()
        {
            Console.Write("Choose table: ");
            string table = Console.ReadLine();
            if (!checkTableExistens(table)) return;

            while (true)
            {
                Console.WriteLine("By what would you like to delete user(s)?");
                Console.WriteLine("0. Id");
                Console.WriteLine("1. Email");
                Console.WriteLine("2. FirstName");
                Console.WriteLine("3. LastName");
                Console.Write("Input: ");
                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 0:
                        DeleteBySMTH(table, "Id");
                        return;
                    case 1:
                        DeleteBySMTH(table, "Email");
                        return;
                    case 2:
                        DeleteBySMTH(table, "FirstName");
                        return;
                    case 3:
                        DeleteBySMTH(table, "LastName");
                        return;
                    default:
                        Console.WriteLine("Invalid command.");
                        Console.ReadLine();
                        break;
                }


            }
        }

        private void DeleteBySMTH(string table, string what)
        {
            Console.Write($"Input {what}: ");
            string by_what = Console.ReadLine();
            try
            {
                cmd.CommandText = $"DELETE {table} WHERE {what}={by_what}";
                cmd.ExecuteNonQuery();
                Console.WriteLine("User is successfully deleted\n");
            }
            catch
            {
                Console.WriteLine($"There aren't users that have this {what}");
                Console.ReadLine();
            }
        }

        private void UpdateItem()
        {
            Console.Write("Choose table: ");
            string table = Console.ReadLine();
            if (!checkTableExistens(table)) return;

            while (true)
            {
                Console.WriteLine("What would you like to update?");
                Console.WriteLine("1. Email");
                Console.WriteLine("2. FirstName");
                Console.WriteLine("3. LastName");
                int choice = int.Parse(Console.ReadLine());
                Console.Write("Input Id: ");
                int id = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        UpdateBySMTH(table, "Email", id);
                        return;
                    case 2:
                        UpdateBySMTH(table, "FirstName", id);
                        return;
                    case 3:
                        UpdateBySMTH(table, "LastName", id);
                        return;
                    default:
                        Console.WriteLine("Invalid command");
                        Console.ReadLine();
                        break;
                }
            }
        }

        private void UpdateBySMTH(string table, string what, int id)
        {
            Console.Write($"Input {what}: ");
            string new_what = Console.ReadLine();

            try
            {
                cmd.CommandText = $"UPDATE {table} SET {what}='{new_what}' WHERE Id={id}";
                cmd.ExecuteNonQuery();
                Console.WriteLine("Users is successfully updated\n");
            }
            catch
            {
                Console.WriteLine("This Id doesn't exist");
                Console.ReadLine();
            }
        }

        private void PrintUsers()
        {
            Console.Write("Input table's name: ");
            string table = Console.ReadLine();
            checkTableExistens(table);
            cmd.CommandText = $"SELECT * from {table} WHERE Id < 11";
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
                    Console.WriteLine(user);
                }
            }
        }

        private bool checkTableExistens(string table)
        {
            if (!tables.Contains(table))
            {
                Console.WriteLine("This table doesn't exist");
                Console.ReadLine();
                return false;
            }
            return true;
        }

        private void ListOfTables()
        {
            cmd.CommandText = $"SELECT name FROM {database}.sys.tables";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    tables.Add((string)reader["name"]);
                }
            }
        }

        private bool IsTableExist(string table)
        {
            if (!tables.Contains(table))
            {
                Console.WriteLine("This table doesn't exist");
                Console.ReadLine();
                return false;
            }
            return true;
        }
    }
}
