using Microsoft.Data.Sqlite;
using SQLitePCL;

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
                while(closeApp == false)
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
                        //case "u":
                        //    UpdateRecord();
                        //    break;
                        //case "d":
                        //    DeleteRecord();
                        //    break;
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
        private static void AddRecord()
            {
                DateTime today = DateTime.Today;
                string date = today.ToString("yyyy-MM-dd");
                int quantity = GetQuantityInput("Please enter the amount of water you drank:");
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
        private static int GetQuantityInput(string message)
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

                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                }
            }
        }
    }

    class DrinkingWater
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
    }
}