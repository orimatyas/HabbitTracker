using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabbitTracker
{
    internal class Helper
    {
        public static void ShowMenu()
        {
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine(@"--Main Menu--

V - View all records
A - Add record
U - Update record
D - Delete record
X - Quit app");
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

    }

}
