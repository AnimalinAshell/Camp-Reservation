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

        public NationalParkCLI(string connectionString)
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
                        ActOnCampgrounds(parks[parkSelection]);
                        break;
                    }
                    else if (parkOption == 2)
                    {
                        SearchForReservation(parks[parkSelection]);
                        break;
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

        private List<Campground> ViewCampgrounds(Park park)
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

            return campgrounds;
        }

        private void ActOnCampgrounds(Park park)
        {
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
                    break;
                }
                else if (campgroundOption == 2)
                {
                    break;
                }
            }
        }

        private void SearchForReservation(Park park)
        {
            int cgSelection;
            DateTime arrivalDate;
            DateTime departureDate;

            List<Campground> campgrounds = ViewCampgrounds(park);

            while (true)
            {
                cgSelection = CLIHelper.GetInteger("Which campground (enter 0 to cancel)?");

                if (cgSelection == 0)
                {
                    return;
                }
                else if (cgSelection > 0 && cgSelection < campgrounds.Count)
                {
                    break;
                }
            }

            List<Site> availableSites;
            int[] site_ids;

            while (true)
            {
                while (true)
                {
                    arrivalDate = CLIHelper.GetDate("What is the arrival date? (MM/DD/YYYY): ");
                    departureDate = CLIHelper.GetDate("What is the departure date? (MM/DD/YYYY): ");

                    if (arrivalDate.Date < departureDate.Date)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Departure date must be at least one day after arrival date.");
                    }
                }

                SiteSqlDAL ssDal = new SiteSqlDAL(ConnectionString);
                availableSites = ssDal.GetAvailableSites(campgrounds[cgSelection], arrivalDate, departureDate);
                site_ids = availableSites.Select(s => s.Site_Id).ToArray();

                if (availableSites.Count == 0)
                {
                    string wantsAlternateRange = CLIHelper.GetString("No available sites. Would you like to enter an alternate date range? (yes or no): ");
                    if (wantsAlternateRange.ToLower() != "yes")
                    {
                        break;
                    }
                }
                else
                {
                    Console.WriteLine(Site.Header);
                    for (int i = 0; i < availableSites.Count; i++)
                    {
                        Console.WriteLine(availableSites[i]);
                    }

                    Console.WriteLine();
                    while (true)
                    {
                        int reservationChoice = CLIHelper.GetInteger("Which site should be reserved (enter 0 to cancel)? ");
                        if (reservationChoice == 0)
                        {
                            break;
                        }
                        else if (site_ids.Contains(reservationChoice))
                        {
                            Reservation newReservation = new Reservation();
                            newReservation.Site_Id = ((Site)availableSites.Where(s => s.Site_Id == reservationChoice)).Site_Id;
                            newReservation.From_Date = arrivalDate;
                            newReservation.To_Date = departureDate;
                            newReservation.Name = CLIHelper.GetString("What name should the reservation be made under? ");

                            ReservationSqlDAL rDal = new ReservationSqlDAL(ConnectionString);
                            rDal.AddReservation(newReservation);
                        }
                        else
                        {
                            Console.WriteLine("Invalid selection. Please select a site id.");
                        }
                    }
                    break;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Thank you for using our reservation system.");
            Console.WriteLine("Please press enter to return to the main menu.");
            Console.ReadLine();

        }
    }
}
