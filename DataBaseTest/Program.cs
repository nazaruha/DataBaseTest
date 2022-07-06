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
    internal class Program
    {
        static void Main(string[] args)
        {
            DBWorker dBWorker = new DBWorker();
            dBWorker.MainMenu();
        }
    }
}
