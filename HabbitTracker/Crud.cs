﻿using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabbitTracker
{
    internal class Crud
    {

        internal static string connectionString = @"Data Source = habit-logger.db";

        internal static void AddRecord()
        {
            DateTime today = DateTime.Today;
            string date = today.ToString("yyyy-MM-dd");
            int? quantity = Helper.GetNumberInput("Please enter the amount of water you drank:");
            if (!quantity.HasValue)
            {
                return;
            }
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
        internal static void ViewRecords()
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
        internal static void UpdateRecord()
        {
            ViewRecords();
            DateTime today = DateTime.Today;
            string date = today.ToString("yyyy-MM-dd");
            int? idNumber = Helper.GetNumberInput("Please enter the ID you want to update:");
            if (!idNumber.HasValue)
            {
                return;
            }
            int? quantity = Helper.GetNumberInput("Please enter the amount of water you drank:");
            if (!quantity.HasValue)
            {
                return;
            }
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = @$"UPDATE drinking_water SET date = '{date}', quantity = {quantity} WHERE Id = {idNumber}";
                    int rowCount = tableCmd.ExecuteNonQuery();
                    if (rowCount == 0)
                    {
                        Console.WriteLine($"The record with Id {idNumber} does not exist.");
                        Console.ReadKey();
                        UpdateRecord();
                    }
                    else
                    {
                        Console.WriteLine($"The record with Id {idNumber} has been updated.");
                        Console.ReadKey();
                    }
                }
            }
        }
        internal static void DeleteRecord()
        {
            Console.Clear();
            ViewRecords();
            var recordID = Helper.GetNumberInput(@"Enter the record ID to delete it from the history");
            if (!recordID.HasValue)
            {
                return;
            }
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
                        Console.ReadKey();
                        DeleteRecord();
                    }
                    else
                    {
                        Console.WriteLine($"The record with Id {recordID} has been deleted.");
                        Console.ReadKey();
                    }
                }
            }
        }

    }
}