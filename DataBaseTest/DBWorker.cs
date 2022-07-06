using System;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Collections.Generic;
using Bogus;
using DataBaseTest.Models;

namespace DataBaseTest
{
    public class DBWorker
    {
        protected string connectionDB { get; set; }
        protected SqlConnection con { get; set; }
        protected SqlCommand cmd { get; set; }

        public DBWorker()
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration configuration = builder.Build();
            connectionDB = configuration.GetConnectionString("DefaultConnection");
            con = new SqlConnection(connectionDB);
            con.Open();
            cmd = con.CreateCommand(); // creating command
        }

        private void OutputMenu()
        {
            Console.WriteLine("\t0. Exit");
            Console.WriteLine("\t1. Create DB");
            Console.WriteLine("\t2. Delete DB");
            Console.WriteLine("\t3. DB List");
            Console.WriteLine("\t4. Connect to database");
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
                        break;
                    case 1:
                        CreateDB();
                        break;
                    case 2:
                        DeleteDB();
                        break;
                    case 3:
                        DBList();
                        break;
                    case 4:
                        LogInDB();
                        break;
                    default:
                        Console.WriteLine("Invalid command.");
                        Console.ReadLine();
                        break;
                }
            }
            Console.WriteLine(connectionDB);
        }

        private void CreateDB()
        {
            Console.Write("Input database's name: ");
            string database = Console.ReadLine();
            try
            {
                cmd.CommandText = $"CREATE DATABASE {database}";
                cmd.ExecuteNonQuery(); // Executing command
                Console.WriteLine("Database is successfully created\n");
            }
            catch
            {
                Console.WriteLine("Database is already exsists with this name.");
                Console.ReadLine();
            }
        }

        private void DeleteDB()
        {
            Console.Write("Input database's name: ");
            string databaseToDelete = Console.ReadLine();
            try
            {
                cmd.CommandText = $"DROP DATABASE {databaseToDelete}";
                cmd.ExecuteNonQuery();
                Console.WriteLine("Database is successfully deleted\n");
            }
            catch
            {
                Console.WriteLine("Database doesn't exist");
                Console.ReadLine();
            }
        }

        private void DBList()
        {
            cmd.CommandText = "SELECT name FROM master.sys.databases";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine(reader["name"]);
                }
            }
        }

        private void LogInDB()
        {
            Console.Write("Input database which you want connect to: ");
            string database = Console.ReadLine();
            con = new SqlConnection(connectionDB + $"Initial Catalog={database}");
            con.Open();
            cmd = con.CreateCommand();
            CurrentDBWorker currentDB = new CurrentDBWorker(con, cmd, database);
            currentDB.MainMenu();
            //try
            //{

            //}
            //catch
            //{
            //    Console.WriteLine("Database doesn't exist");
            //    Console.ReadLine();
            //}

        }
    }
}
