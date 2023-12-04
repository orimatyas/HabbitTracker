using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabbitTracker
{
    public class UpdateOperations
    {
        public static void UpdateHabit()
        {
            ReadOperations.ViewHabits();
            int? idNumber = Helper.GetNumberInput("Please enter the habit ID you want to update:");
            if (!idNumber.HasValue)
            {
                return;
            }
            int? name = Helper.GetNumberInput("Please enter the new habit name:");
            if (!name.HasValue)
            {
                return;
            }
            int? unit = Helper.GetNumberInput("Please enter the new unit:");
            if (!unit.HasValue)
            {
                return;
            }
            using (var connection = new SqliteConnection(Helper.connectionString))
            {
                connection.Open();
                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = @$"UPDATE all_habits SET name = '{name}', unit = '{unit}' WHERE habit_id = '{idNumber}'";
                    int rowCount = tableCmd.ExecuteNonQuery();
                    if (rowCount == 0)
                    {
                        Console.WriteLine($"The habit with ID {idNumber} does not exist.");
                        Console.ReadKey();
                        UpdateRecord();
                    }
                    else
                    {
                        Console.WriteLine($"The habit with ID {idNumber} has been updated.");
                        Console.ReadKey();
                    }
                }
            }
        }
        public static void UpdateRecord()
        {
            ReadOperations.ViewRecords();
            int? idNumber = Helper.GetNumberInput("Please enter the record ID you want to update:");
            if (!idNumber.HasValue)
            {
                return;
            }
            string date;
            if (Helper.GetUserConfirmation("Do you want to update the date? (yes/no)"))
            {
                DateTime newDate = Helper.GetDateInput("Please enter the new date (yyyy-MM-dd):");
                date = newDate.ToString("yyyy-MM-dd");
            }
            else
            {
                date = Helper.GetExistingDateForRecord(idNumber.Value);
            }
            int? quantity = Helper.GetNumberInput($"Please enter the amount of {Helper.currentHabit} you did:");
            if (!quantity.HasValue)
            {
                return;
            }
            using (var connection = new SqliteConnection(Helper.connectionString))
            {
                connection.Open();
                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = @$"UPDATE habit_records SET date = '{date}', quantity = '{quantity}' WHERE record_id = '{idNumber}'";
                    int rowCount = tableCmd.ExecuteNonQuery();
                    if (rowCount == 0)
                    {
                        Console.WriteLine($"The record with ID {idNumber} does not exist.");
                        Console.ReadKey();
                        UpdateRecord();
                    }
                    else
                    {
                        Console.WriteLine($"The record with ID {idNumber} has been updated.");
                        Console.ReadKey();
                    }
                }
            }
        }
    }
}
