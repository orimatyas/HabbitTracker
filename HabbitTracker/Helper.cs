using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HabbitTracker
{
    public class Helper
    {
        public static string connectionString = @"Data Source = habit_logger.db";
        public static string? currentHabit;
        public static int? currentID;
        internal static string? currentUnit;

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

V - View all habit
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
                    case "v":
                        ReadOperations.ViewHabits();
                        break;
                    case "c":
                        GetHabit();
                        break;
                    case "a":
                        CreateOperations.AddHabit();
                        break;
                    case "u":
                        UpdateOperations.UpdateHabit();
                        break;
                    case "d":
                        DeleteOperations.DeleteHabit();
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
                        ReadOperations.ViewRecords();
                        break;
                    case "a":
                        CreateOperations.AddRecord();
                        break;
                    case "u":
                        UpdateOperations.UpdateRecord();
                        break;
                    case "d":
                        DeleteOperations.DeleteRecord();
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
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine(@"--Reports Options--

M - Make a report
X - Back to Main Menu
-----------------");

                string consoleInput = Console.ReadLine().Trim().ToLower();
                switch (consoleInput)
                {
                    case "m":
                        Report.CreateReport();
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
        public static void GetHabit()
        {
            Console.WriteLine(currentHabit);
            ReadOperations.ViewHabits();
            Console.WriteLine("Choose habit you want by typing the habit ID:");
            if (int.TryParse(Console.ReadLine(), out int newID))
            {
                currentID = newID;
                SQLitePCL.Batteries.Init();
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    using (var tableCmd = connection.CreateCommand())
                    {
                        tableCmd.CommandText = @$"SELECT name, unit FROM all_habits WHERE habit_id = '{currentID}'";
                        using (SqliteDataReader reader = tableCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                currentHabit = reader.GetString(0);
                                currentUnit = reader.GetString(1);
                            }
                        }
                    }
                }
                Console.WriteLine($"Selected habit : {currentHabit}");
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid habit ID.");
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
        public static bool GetUserConfirmation(string message)
        {
            while (true)
            {
                Console.WriteLine(message);
                string answer = Console.ReadLine().Trim().ToLower();

                if (answer == "yes")
                {
                    return true;
                }
                else if (answer == "no")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Invalid input! Please answer with 'yes' or 'no'.");
                }
            }
        }
        public static DateTime GetDateInput(string message)
        {
            DateTime dateInput;
            const string dateFormat = "yyyy-MM-dd";
            while (true)
            {
                Console.WriteLine(message);
                string input = Console.ReadLine().Trim();

                if (DateTime.TryParseExact(input, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateInput))
                {
                    return dateInput;
                }
                else
                {
                    Console.WriteLine($"Invalid date format. Please enter a date in the format {dateFormat} (e.g. 2023-01-31).");
                }
            }
        }
        public static string GetExistingDateForRecord(int idNumber)
        {
            SQLitePCL.Batteries.Init();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = @$"SELECT date FROM habit_records WHERE record_id = '{idNumber}'";
                    using (SqliteDataReader reader = tableCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            DateTime date = reader.GetDateTime(3);
                            return date.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            Console.WriteLine("No record found.");
                            return null;
                        }
                    }
                }
            }
        }
    }
}
