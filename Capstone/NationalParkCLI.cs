using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using Capstone.DAL;


namespace Capstone
{
    public class NationalParkCLI
    {

        public void Run(string connectionString)
        {
            ParkSqlDAL p = new ParkSqlDAL(connectionString);
            List<Park> parks = p.GetAllParks();
            parks.Insert(0, null);
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Select a park for further details..");
                int parkSelection;

                for (int i = 1; i < parks.Count; i++)
                {
                    Console.WriteLine("  " + i + ")  " + parks[i].Name);
                }
                Console.WriteLine("  " + parks.Count + ")  Quit ");

                while (true)
                {
                    parkSelection = CLIHelper.GetInteger(">>");
                    if (parkSelection > 0 && parkSelection <= parks.Count)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid option. Please choose a number from the list.");
                    }
                }


                if (parkSelection == parks.Count)
                {
                    break;
                }
                else if (parkSelection > 0 && parkSelection < parks.Count)
                {
                    Console.Clear();
                    Console.WriteLine(parks[parkSelection]);
                }

                int parkOption;

                Console.WriteLine();
                Console.WriteLine("  1)  View Campgrounds");
                Console.WriteLine("  2)  Search for Reservation");
                Console.WriteLine("  3)  Return to Previous Screen");

                while (true)
                {
                    parkOption = CLIHelper.GetInteger(">>");

                    if (parkOption == 1)
                    {
                        ViewCampgrounds(parks[parkSelection]);
                    }
                    else if (parkOption == 2)
                    {
                        SearchForReservation(parks[parkSelection]);
                    }
                    else if (parkOption == 3)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid option. Please choose a number from the list.");
                    }
                }

            }
        }

        private void SearchForReservation(Park park)
        {

        }

        private void ViewCampgrounds(Park park)
        {

        }
    }
}
