using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HabbitTracker
{
    public class CreateOperations
    {
        public static void AddHabit()
        {
            Console.WriteLine("Enter habit name:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter unit of measurement (e.g., 'times', 'km', 'glasses'):");
            string unit = Console.ReadLine();
            using (var connection = new SqliteConnection(Helper.connectionString))
            {
                connection.Open();
                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = @$"INSERT INTO all_habits(name, unit) VALUES('{name}', '{unit}')";
                    tableCmd.ExecuteNonQuery();
                }
            }
        }
        public static void AddRecord()
        {
            int? habitID = Helper.currentID;
            DateTime today = DateTime.Today;
            string date = today.ToString("yyyy-MM-dd");
            int? quantity = Helper.GetNumberInput($"Please enter the amount of {Helper.currentHabit} you did ({Helper.currentUnit}):");
            if (!quantity.HasValue)
            {
                return;
            }
            using (var connection = new SqliteConnection(Helper.connectionString))
            {
                connection.Open();
                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = @$"INSERT INTO habit_records(habit_id, date, quantity) VALUES('{habitID}', '{date}', {quantity})";
                    tableCmd.ExecuteNonQuery();
                }
            }
            Console.Clear();
        }
        public static void CreateDB(string message)
        {
            SQLitePCL.Batteries.Init();
            using (var connection = new SqliteConnection(Helper.connectionString))
            {
                connection.Open();
                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = message;
                    tableCmd.ExecuteNonQuery();
                }
            }
        }
    }
}
