using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HabbitTracker
{
    internal class Helper
    {
        internal static string connectionString = @"Data Source = habit_logger.db";
        public static string currentHabit = "Drinking Water";
        public static void ShowMenu()
        {
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine(@"--Main Menu--

H - Habits
R - Records
P - Reports
X - Quit app
-----------------");
                Console.WriteLine($"Currently selected habit: {currentHabit}");
                string consoleInput = Console.ReadLine().Trim().ToLower();
                switch (consoleInput)
                {
                    case "h":
                        HabitsOption();
                        break;
                    case "r":
                        RecordsOption();
                        break;
                    case "p":
                        ReportsOption();
                        break;
                    case "x":
                        Console.WriteLine("\nGoodbye!\n");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid input!");
                        break;
                }
            }
        }
        public static void HabitsOption()
        {
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine(@"--Habits Option--

C - Choose habit
A - Add habit
U - Update habit
D - Delete habit
X - Back to Main Menu
-----------------");
                Console.WriteLine($"Currently selected habit: {currentHabit}");

                string consoleInput = Console.ReadLine().Trim().ToLower();
                switch (consoleInput)
                {
                    case "c":
                        Crud.ViewRecords();
                        break;
                    case "a":
                        Crud.AddRecord();
                        break;
                    case "u":
                        Crud.UpdateRecord();
                        break;
                    case "d":
                        Crud.DeleteRecord();
                        break;
                    case "x":
                        Console.Clear();
                        closeApp = true;
                        break;
                    default:
                        Console.WriteLine("Invalid input!");
                        break;
                }
            }
        }
        public static void RecordsOption()
        {
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine(@"--Records Options--

V - View all records
A - Add record
U - Update record
D - Delete record
X - Back to Main Menu
-----------------");
                Console.WriteLine($"Currently selected habit: {currentHabit}");

                string consoleInput = Console.ReadLine().Trim().ToLower();
                switch (consoleInput)
                {
                    case "v":
                        Crud.ViewRecords();
                        break;
                    case "a":
                        Crud.AddRecord();
                        break;
                    case "u":
                        Crud.UpdateRecord();
                        break;
                    case "d":
                        Crud.DeleteRecord();
                        break;
                    case "x":
                        Console.Clear();
                        closeApp = true;
                        break;
                    default:
                        Console.WriteLine("Invalid input!");
                        break;
                }
            }
        }
        public static void ReportsOption()
        {
            throw new NotImplementedException();
        }
        public static void CreateDB(string message)
        {
            SQLitePCL.Batteries.Init();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = message;
                    tableCmd.ExecuteNonQuery();
                }
            }
        }
        public static int? GetNumberInput(string message)
        {
            while (true)
            {
                Console.WriteLine(message);
                Console.WriteLine("...or press X to return.");
                string rawInput = Console.ReadLine().ToLower().Trim();

                if (rawInput == "x")
                {
                    return null;
                }

                if (int.TryParse(rawInput, out int numberInput))
                {
                    return numberInput;
                }

                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
        }
        //public static void CreateHabit()
        //{
        //    Console.WriteLine("Enter habit name:");
        //    string name = Console.ReadLine();
        //    Console.WriteLine("Enter unit of measurement (e.g., 'times', 'km', 'glasses'):");
        //    string unit = Console.ReadLine();
        //    using (var connection = new SqliteConnection(Crud.connectionString))
        //    {
        //        connection.Open();
        //        using (var tableCmd = connection.CreateCommand())
        //        {
        //            tableCmd.CommandText = @$"INSERT INTO all_habits(date, quantity) VALUES('{date}', {quantity})";
        //            tableCmd.ExecuteNonQuery();
        //        }
        //    }
        //}
    }
}
