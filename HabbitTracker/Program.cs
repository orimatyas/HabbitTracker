using Microsoft.Data.Sqlite;
using SQLitePCL;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HabbitTracker
{
    internal class Program
    {

        static void Main(string[] args)
        {
            SQLitePCL.Batteries.Init();
            using (var connection = new SqliteConnection(Crud.connectionString))
            {
                connection.Open();
                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
Date TEXT,
Quantity INTEGER);";
                    tableCmd.ExecuteNonQuery();
                }
            }
            Helper.ShowMenu();
        }
    }
}
