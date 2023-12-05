using HabbitTracker.Model;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HabbitTracker
{
    public class Report
    {
        public static void HabitReport()
        {
            StringBuilder habitBuilder = new StringBuilder();
            ReadOperations.ViewRecords();
            Console.WriteLine("Enter the name of the habit:");
            string selectedHabit = Console.ReadLine();
            DateTime newDateFrom = Helper.GetDateInput("Enter the start date (yyyy-MM-dd):");
            string fromDate = newDateFrom.ToString("yyyy-MM-dd");
            DateTime newDateTo = Helper.GetDateInput("Enter the end date (yyyy-MM-dd):");
            string toDate = newDateTo.ToString("yyyy-MM-dd");
            Console.WriteLine($"You selected {selectedHabit} from {fromDate} to {toDate}.");
            if (Helper.GetUserConfirmation("Do you want to continue? (yes/no)"))
            {
                Console.Clear();
                SQLitePCL.Batteries.Init();
                using (var connection = new SqliteConnection(Helper.connectionString))
                {
                    connection.Open();
                    using (var tableCmd = connection.CreateCommand())
                    {
                        tableCmd.CommandText = $@"SELECT hr.record_id, hr.date, ah.name, ah.unit, hr.quantity
FROM habit_records hr
INNER JOIN all_habits ah ON hr.habit_id = ah.habit_id
WHERE ah.name = '{selectedHabit}'
AND hr.date BETWEEN '{fromDate}' AND '{toDate}'";
                        tableCmd.ExecuteNonQuery();
                        List<RecordsJoined> habitData = new();
                        SqliteDataReader reader = tableCmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                habitData.Add(
                                    new RecordsJoined
                                    {
                                        RecordId = reader.GetInt32(0),
                                        Date = reader.GetDateTime(1),
                                        Name = reader.GetString(2),
                                        Unit = reader.GetString(3),
                                        Quantity = reader.GetInt32(4)
                                    });
                            }
                        }
                        else
                        {
                            Console.WriteLine("No rows found.");
                        }
                        habitBuilder.AppendLine("----------");
                        foreach (var record in habitData)
                        {
                            string formattedDate = record.Date.ToString("yyyy-MM-dd");
                            habitBuilder.AppendLine($"{record.RecordId} - {formattedDate} - {record.Name} - {record.Quantity} {record.Unit}");
                        }
                        habitBuilder.AppendLine("----------");
                    }
                }
            }
            else
            {
                Console.WriteLine("Report cancelled...");
            }
            Console.WriteLine(habitBuilder.ToString());
            string habitOutput = habitBuilder.ToString();
            string statOutput = GetHabitStats(selectedHabit, fromDate, toDate);
            SaveReportToFile(habitOutput, statOutput);
        }
        public static string GetHabitStats(string selectedHabit, string fromDate, string toDate)
        {
            StringBuilder statBuilder = new StringBuilder();
            if (Helper.GetUserConfirmation("Do you want detailed stats? (yes/no)"))
            {
                SQLitePCL.Batteries.Init();
                using (var connection = new SqliteConnection(Helper.connectionString))
                {
                    connection.Open();
                    using (var tableCmd = connection.CreateCommand())
                    {
                        tableCmd.CommandText = $@"SELECT MAX(hr.quantity) as MaxQuantity,
MIN(hr.quantity) as MinQuantity,
AVG(hr.quantity) as AverageQuantity,
SUM(hr.quantity) as TotalQuantity,
COUNT(*) as RecordCount,
ah.unit
FROM habit_records hr
INNER JOIN all_habits ah ON hr.habit_id = ah.habit_id
WHERE ah.name = '{selectedHabit}'
AND hr.date BETWEEN '{fromDate}' AND '{toDate}'";
                        List<Stats> statsData = new();
                        SqliteDataReader statsReader = tableCmd.ExecuteReader();
                        if (statsReader.HasRows)
                        {
                            while (statsReader.Read())
                            {
                                statsData.Add(
                                    new Stats
                                    {
                                        MaxQuantity = statsReader.GetInt32(0),
                                        MinQuantity = statsReader.GetInt32(1),
                                        AvgQuantity = statsReader.GetInt32(2),
                                        TotalQuantity = statsReader.GetInt32(3),
                                        Unit = statsReader.GetString(5)
                                    });
                            }
                        }
                        else
                        {
                            Console.WriteLine("No rows found.");
                        }
                        statBuilder.AppendLine("----------");
                        foreach (var record in statsData)
                        {
                            statBuilder.AppendLine(@$"-{selectedHabit} between {fromDate} and {toDate}-
Maximum amount of {selectedHabit}: {record.MaxQuantity} {record.Unit}
Minimum amount of {selectedHabit}: {record.MinQuantity} {record.Unit}
Average amount of {selectedHabit}: {record.AvgQuantity:F2} {record.Unit}
Total amount of {selectedHabit}: {record.TotalQuantity} {record.Unit}");
                        }
                        statBuilder.AppendLine("----------");
                    }
                }
            }
            else
            {
                Console.WriteLine("Report cancelled...");
            }
            Console.WriteLine(statBuilder.ToString());
            return statBuilder.ToString();
        }
        private static void SaveReportToFile(string habitOutput, string statOutput)
        {
            if (Helper.GetUserConfirmation("Do you want to save the report? (yes/no)"))
            {
                Console.WriteLine("Enter the filename:");
                string filename = Console.ReadLine();
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(habitOutput);
                sb.AppendLine(statOutput);
                File.WriteAllText(filename, sb.ToString());
                Console.WriteLine($"Report saved to {filename}");
            }
        }
    }
}
