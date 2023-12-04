using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabbitTracker
{
    public class DeleteOperations
    {
        public static void DeleteHabit()
        {
            Console.Clear();
            ReadOperations.ViewHabits();
            var habitID = Helper.GetNumberInput(@"Enter the habit ID to delete it");
            if (!habitID.HasValue)
            {
                return;
            }
            using (var connection = new SqliteConnection(Helper.connectionString))
            {
                connection.Open();
                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = $@"DELETE FROM all_habits WHERE habit_id = '{habitID}'";
                    int rowCount = tableCmd.ExecuteNonQuery();
                    if (rowCount == 0)
                    {
                        Console.WriteLine($"The habit with ID {habitID} does not exist.");
                        Console.ReadKey();
                        DeleteRecord();
                    }
                    else
                    {
                        Console.WriteLine($"The habit with ID {habitID} has been deleted.");
                        Console.ReadKey();
                    }
                }
            }
        }
        public static void DeleteRecord()
        {
            Console.Clear();
            ReadOperations.ViewRecords();
            var recordID = Helper.GetNumberInput(@"Enter the record ID to delete it from the history");
            if (!recordID.HasValue)
            {
                return;
            }
            using (var connection = new SqliteConnection(Helper.connectionString))
            {
                connection.Open();
                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = $@"DELETE FROM habit_records WHERE record_id = '{recordID}'";
                    int rowCount = tableCmd.ExecuteNonQuery();
                    if (rowCount == 0)
                    {
                        Console.WriteLine($"The record with ID {recordID} does not exist.");
                        Console.ReadKey();
                        DeleteRecord();
                    }
                    else
                    {
                        Console.WriteLine($"The record with ID {recordID} has been deleted.");
                        Console.ReadKey();
                    }
                }
            }
        }
    }
}
