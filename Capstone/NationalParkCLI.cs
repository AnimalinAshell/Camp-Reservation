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
        private string ConnectionString { get; set; }

        public NationalParkCLI (string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public void Run()
        {
            ParkSqlDAL p = new ParkSqlDAL(ConnectionString);
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

        private void ViewCampgrounds(Park park)
        {
            Console.Clear();
            CampgroundSqlDAL cgDAL = new CampgroundSqlDAL(ConnectionString);
            List<Campground> campgrounds = cgDAL.GetCampgroundsAtPark(park.Park_id);
            campgrounds.Insert(0, null);

            Console.WriteLine("".PadRight(5) + Campground.Header);
            for (int i = 1; i < campgrounds.Count; i++)
            {
                Console.WriteLine($"#{i,-1}   {campgrounds[i]}");
            }

            Console.WriteLine("Select a Command");
            Console.WriteLine("  1) Search for Available Reservation");
            Console.WriteLine("  2) Return to Previous Screen");


            int campgroundOption;
            while (true)
            {
                campgroundOption = CLIHelper.GetInteger(">>");
                if (campgroundOption == 1)
                {
                    SearchForReservation(park);
                }
                else if (campgroundOption == 2)
                {
                    break;
                }
            }
        }

        private void SearchForReservation(Park park)
        {
            CLIHelper.GetInteger("Which campground (enter 0 to cancel)?"); // NEED TO VALIDATE AND ALLOW CANCEL
            CLIHelper.GetDate("What is the arrival date? MM/DD/YYYY");
            CLIHelper.GetDate("What is the departure date? MM/DD/YYYY");


        }
    }
}
