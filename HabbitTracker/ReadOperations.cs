using HabbitTracker.Model;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabbitTracker
{
    public class ReadOperations
    {
        public static void ViewHabits()
        {
            Console.Clear();
            SQLitePCL.Batteries.Init();
            using (var connection = new SqliteConnection(Helper.connectionString))
            {
                connection.Open();
                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = @$"SELECT * FROM all_habits";
                    tableCmd.ExecuteNonQuery();
                    List<Habit> tableData = new();
                    SqliteDataReader reader = tableCmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            tableData.Add(
                                new Habit
                                {
                                    HabitId = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Unit = reader.GetString(2),
                                });
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    Console.WriteLine("----------");
                    foreach (var h in tableData)
                    {
                        Console.WriteLine($"{h.HabitId} - {h.Name} - Unit: {h.Unit}");
                    }
                    Console.WriteLine("----------");
                }
            }
        }
        public static void ViewRecords()
        {
            Console.Clear();
            SQLitePCL.Batteries.Init();
            using (var connection = new SqliteConnection(Helper.connectionString))
            {
                connection.Open();
                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = @"SELECT hr.record_id, hr.date, ah.name, ah.unit, hr.quantity
                    FROM habit_records hr
                    INNER JOIN all_habits ah ON hr.habit_id = ah.habit_id";
                    tableCmd.ExecuteNonQuery();
                    List<RecordsJoined> tableData = new();
                    SqliteDataReader reader = tableCmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            tableData.Add(
                                new RecordsJoined
                                {
                                    RecordId = reader.GetInt32(0),
                                    Date = reader.GetDateTime(1),
                                    Name = reader.GetString(2),
                                    Unit = reader.GetString(3),
                                    Quantity = reader.GetInt32(4),
                                });
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    Console.WriteLine("----------");
                    foreach (var record in tableData)
                    {
                        string formattedDate = record.Date.ToString("yyyy-MM-dd");
                        Console.WriteLine($"{record.RecordId} - {formattedDate} - {record.Name} - {record.Unit} : {record.Quantity}");
                    }
                    Console.WriteLine("----------");
                }
            }
        }
    }
}
