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
            List<RecordsJoined> habitData = new List<RecordsJoined>();
            List<Stats> statsData = new List<Stats>();
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
                                        Quantity = reader.GetInt32(4)
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
                            Console.WriteLine($"{record.RecordId} - {formattedDate} - {record.Name} - {record.Quantity} {record.Unit}");
                        }
                        Console.WriteLine("----------");
                    }
                }
                statsData = GetHabitStats(selectedHabit, fromDate, toDate);
                SaveReportToFile(habitData, statsData, selectedHabit, fromDate, toDate);
            }
            else
            {
                Console.WriteLine("Report cancelled...");
            }
            
        }
        public static List<Stats> GetHabitStats(string selectedHabit, string fromDate, string toDate)
        {
            List<Stats> statsData = new List<Stats>();
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
                        tableCmd.ExecuteNonQuery();
                        List<Stats> tableData = new();
                        SqliteDataReader statsReader = tableCmd.ExecuteReader();
                        if (statsReader.HasRows)
                        {
                            while (statsReader.Read())
                            {
                                tableData.Add(
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
                        Console.WriteLine("----------");
                        foreach (var record in tableData)
                        {
                            Console.WriteLine(@$"-{selectedHabit} between {fromDate} and {toDate}-
Maximum amount of {selectedHabit}: {record.MaxQuantity} {record.Unit}
Minimum amount of {selectedHabit}: {record.MinQuantity} {record.Unit}
Average amount of {selectedHabit}: {record.AvgQuantity:F2} {record.Unit}
Total amount of {selectedHabit}: {record.TotalQuantity} {record.Unit}");
                        }
                        Console.WriteLine("----------");
                    }
                }
            }
            else
            {
                Console.WriteLine("Report cancelled...");
            }
            return statsData;
        }
        private static void SaveReportToFile(List<RecordsJoined> habitData, List<Stats> statsData, string selectedHabit, string fromDate, string toDate)
        {
            if (Helper.GetUserConfirmation("Do you want to save the report? (yes/no)"))
            {
                Console.WriteLine("Enter the filename:");
                string filename = Console.ReadLine();
                StringBuilder sb = new StringBuilder();
                foreach (var record in habitData)
                {
                    string formattedDate = record.Date.ToString("yyyy-MM-dd");
                    sb.AppendLine($"{record.RecordId} - {formattedDate} - {record.Name} - {record.Quantity} {record.Unit}");
                }
                foreach (var record in statsData)
                {
                    sb.AppendLine(@$"-{selectedHabit} between {fromDate} and {toDate}-
Maximum amount of {selectedHabit}: {record.MaxQuantity} {record.Unit}
Minimum amount of {selectedHabit}: {record.MinQuantity} {record.Unit}
Average amount of {selectedHabit}: {record.AvgQuantity:F2} {record.Unit}
Total amount of {selectedHabit}: {record.TotalQuantity} {record.Unit}");
                }
                File.WriteAllText(filename, sb.ToString());
                Console.WriteLine($"Report saved to {filename}");
            }
        }
    }
}
