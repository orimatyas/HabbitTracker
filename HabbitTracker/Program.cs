using Microsoft.Data.Sqlite;
using SQLitePCL;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HabbitTracker
{
    internal class Program
    {

        static void Main(string[] args)
        {
            CreateOperations.CreateDB(@"CREATE TABLE IF NOT EXISTS all_habits (
habit_id INTEGER PRIMARY KEY AUTOINCREMENT,
name TEXT,
unit TEXT);");
            CreateOperations.CreateDB(@"CREATE TABLE IF NOT EXISTS habit_records (
record_id INTEGER PRIMARY KEY AUTOINCREMENT,
habit_id INTEGER ,
date TEXT,
quantity INTEGER);");
            Helper.ShowMenu();
        }
    }
}
