using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;
using DataLayer.Repository;

namespace UI
{
    internal class Program
    {

        private static bool _breakout;
        private static CustomerRepository _repo;

        private static void Main()
        {
            _repo = new CustomerRepository();
            _breakout = false;

            do
            {
                LoadByStoreProcedure();

            } while (!_breakout);

        }

        private static void LoadByStoreProcedure()
        {
            Console.Clear();

            string menuOption = StoredProcedureOptions();

            if (menuOption != "Quit")
            {
                string par = GiveDiscreets(menuOption);

                List<Customer> customers = _repo.GetByStoredProcedure(menuOption, par);

                Console.Clear();

                if (customers.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Not in the database...");
                    Console.ReadLine();
                    Console.ResetColor();
                }
                else
                {
                    foreach (var c in customers)
                    {
                        Console.WriteLine("CustomerID:       {0}", c.CustomerID);
                        Console.WriteLine("Customer Name:    {0}", c.ContactName);
                        Console.WriteLine("City:             {0}", c.City);
                        Console.WriteLine("Country:          {0}", c.Country);
                        Console.WriteLine("------------------------------------------");
                    }

                    Console.ReadLine();

                }

            }
            else
            {
                _breakout = true;
            }

        }

        private static string StoredProcedureOptions()
        {
            string stringInput = null;

            do
            {
                Console.Clear();
                Console.WriteLine("Which would you like to filter by?");
                Console.WriteLine("(1)CustomerID");
                Console.WriteLine("(2)City");
                Console.WriteLine("(3)Country");
                Console.WriteLine("(Q)Quit");
                Console.WriteLine();
                Console.Write("Enter choice: ");
                stringInput = Console.ReadLine().ToUpper();

                switch (stringInput)
                {
                    case "1":
                        return "CustByID";
                    case "2":
                        return "CustByCity";
                    case "3":
                        return "CustByCountry";
                    case "Q":
                        return "Quit";
                    case "":
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You must enter something..");
                        Console.ResetColor();
                        Console.ReadLine();
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid input. Try again.");
                        Console.ReadLine();
                        Console.ResetColor();
                        break;
                }

            } while (true);

        }

        private static string GiveDiscreets(string sp)
        {
            switch (sp)
            {
                case "CustByID":
                    _repo.DisplayDistincts("CustomerID");
                    break;
                case "CustByCity":
                    _repo.DisplayDistincts("City");
                    break;
                case "CustByCountry":
                    _repo.DisplayDistincts("Country");
                    break;
            }
            Console.WriteLine();
            Console.Write("Enter a filter: ");
            string input = Console.ReadLine();

            return input;
        }
    }
}