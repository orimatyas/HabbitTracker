using Microsoft.Data.Sqlite;
using SQLitePCL;
using System.Globalization;

namespace HabbitTracker
{
    internal class Program
    {
        static string connectionString = @"Data Source = habit-logger.db";

        static void Main(string[] args)
        {
            SQLitePCL.Batteries.Init();
            using (var connection = new SqliteConnection(connectionString))
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
            GetUserInput();
        }
        private static void GetUserInput()
        {
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine(@"--Main Menu--

V - View all records
A - Add record
U - Update record
D - Delete record
X - Exit app");
                string consoleInput = Console.ReadLine();
                switch (consoleInput)
                {
                    case "v":
                        ViewRecords();
                        break;
                    case "a":
                        AddRecord();
                        break;
                    case "u":
                        UpdateRecord();
                        break;
                    case "d":
                        DeleteRecord();
                        break;
                    case "x":
                        Console.WriteLine("\nGoodbye!\n");
                        closeApp = true;
                        break;
                    default:
                        Console.WriteLine("Invalid input!");
                        break;
                }
            }
        }

        private static void ViewRecords()
        {
            Console.Clear();
            SQLitePCL.Batteries.Init();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = @$"SELECT * FROM drinking_water";
                    tableCmd.ExecuteNonQuery();
                    List<DrinkingWater> tableData = new();
                    SqliteDataReader reader = tableCmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            tableData.Add(
                                new DrinkingWater
                                {
                                    Id = reader.GetInt32(0),
                                    Date = reader.GetDateTime(1),
                                    Quantity = reader.GetInt32(2)
                                });
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    Console.WriteLine("----------");
                    foreach (var dw in tableData)
                    {
                        string formattedDate = dw.Date.ToString("dd-MM-yyyy");
                        Console.WriteLine($"{dw.Id} - {formattedDate} - Quantity: {dw.Quantity}");
                    }
                    Console.WriteLine("----------");
                }
            }
        }
        private static void AddRecord()
        {
            DateTime today = DateTime.Today;
            string date = today.ToString("yyyy-MM-dd");
            int quantity = GetNumberInput("Please enter the amount of water you drank:");
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = @$"INSERT INTO drinking_water(date, quantity) VALUES('{date}', {quantity})";
                    tableCmd.ExecuteNonQuery();
                }
            }
            Console.Clear();
        }
        private static void UpdateRecord()
        {
            throw new NotImplementedException();
        }
        private static void DeleteRecord()
        {
            Console.Clear();
            ViewRecords();
            var recordID = GetNumberInput("Enter the record ID to delete it from the history:");
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = $@"DELETE FROM drinking_water WHERE Id = {recordID}";
                    int rowCount = tableCmd.ExecuteNonQuery();
                    if (rowCount == 0)
                    {
                        Console.WriteLine($"The record with Id {recordID} does not exist.");
                    }
                    else
                    {
                        Console.WriteLine($"The record with Id {recordID} has been deleted.");
                    }
                    DeleteRecord();
                }
            }
        }
        private static int GetNumberInput(string message)
        {
            Console.WriteLine(message);
            string numberInput = Console.ReadLine();
            if (numberInput == "0")
            {
                GetUserInput();
            }

            int quantityInput = Convert.ToInt32(numberInput);
            return quantityInput;
        }
    }

    class DrinkingWater
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
    }
}