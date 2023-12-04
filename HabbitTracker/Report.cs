using HabbitTracker.Model;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HabbitTracker
{
    public class Report
    {
        public static void CreateReport()
        {
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
                            Console.WriteLine($"{record.RecordId} - {record.Date} - {record.Name} - {record.Quantity} {record.Unit}");
                        }
                        Console.WriteLine("----------");
                    }
                }
            }
            else 
            {
                Console.WriteLine("Report cancelled...");
            }
        }
    }
}
