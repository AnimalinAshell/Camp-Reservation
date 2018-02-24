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

        bool returnToMainMenu;

        public void Run()
        {
            ParkSqlDAL p = new ParkSqlDAL(ConnectionString);
            List<Park> parks = p.GetAllParks();
            parks.Insert(0, null);
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Select a park for further details..");
                returnToMainMenu = false;

                for (int i = 1; i < parks.Count; i++)
                {
                    Console.WriteLine("  " + i + ")  " + parks[i].Name);
                }
                Console.WriteLine("  " + parks.Count + ")  Quit ");

                int parkSelection = CLIHelper.GetIntegerInRange(">>", 1, parks.Count);

                if (parkSelection == parks.Count)
                {
                    // Quit at Main Menu
                    break;
                }

                while (!returnToMainMenu)
                {
                    Console.Clear();
                    Console.WriteLine(parks[parkSelection]);

                    Console.WriteLine();
                    Console.WriteLine("  1)  View Campgrounds");
                    Console.WriteLine("  2)  Search for Reservation");
                    Console.WriteLine("  3)  Return to Previous Screen");

                    int parkOption = CLIHelper.GetIntegerInRange(">>", 1, 3);

                    if (parkOption == 1)
                    {
                        ViewCampgrounds(parks[parkSelection]);
                        ActOnCampgrounds(parks[parkSelection]);
                    }
                    else if (parkOption == 2)
                    {
                        SearchForReservation(parks[parkSelection]);
                    }
                    else if (parkOption == 3)
                    {
                        returnToMainMenu = true;
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
            Console.WriteLine();

            return campgrounds;
        }

        private void ActOnCampgrounds(Park park)
        {
            Console.WriteLine("Select a Command");
            Console.WriteLine("  1) Search for Available Reservation");
            Console.WriteLine("  2) Return to Previous Screen");

            int campgroundOption = CLIHelper.GetIntegerInRange(">>", 1, 2);

            if (campgroundOption == 1)
            {
                SearchForReservation(park);
            }

        }

        private void SearchForReservation(Park park)
        {
            DateTime arrivalDate;
            DateTime departureDate;

            List<Campground> campgrounds = ViewCampgrounds(park);

            int cgSelection = CLIHelper.GetIntegerInRange("Which campground (enter 0 to cancel)?",
                0, campgrounds.Count - 1);
            if (cgSelection == 0)
            {
                return;
            }

            List<Site> availableSites;
            List<int> site_numbers;

            while (true)
            {
                Tuple<DateTime, DateTime> dateRange = CLIHelper.GetFutureDateRange(
                    "What is the arrival date? (MM/DD/YYYY): ",
                    "What is the departure date? (MM/DD/YYYY): ",
                    "Departure date must be at least one day after arrival date.",
                    true, 
                    "1234",
                    "Post-dated reservations require administrative access.",
                    "Please enter password to continue (0 to quit) (hint: 1234): ");
                if (dateRange == null)
                {
                    returnToMainMenu = true;
                    break;
                }
                arrivalDate = dateRange.Item1;
                departureDate = dateRange.Item2;

                SiteSqlDAL ssDal = new SiteSqlDAL(ConnectionString);
                availableSites = ssDal.GetAvailableSites(campgrounds[cgSelection], arrivalDate, departureDate);
                site_numbers = availableSites.Select(s => s.Site_Number).ToList();
                site_numbers.Add(0);

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
                        Console.WriteLine(availableSites[i].ToString((int)(departureDate - arrivalDate).TotalDays));
                    }

                    Console.WriteLine();
                    int reservationChoice = CLIHelper.GetIntegerInRange("Which site should be reserved (enter 0 to cancel)?",
                        site_numbers, "Invalid selection. Please select a site id.");

                    if (reservationChoice != 0)
                    {
                        Reservation newReservation = new Reservation();
                        newReservation.Site_Id = availableSites.Where(s => s.Site_Number == reservationChoice).First().Site_Id;
                        newReservation.From_Date = arrivalDate;
                        newReservation.To_Date = departureDate;
                        newReservation.Name = CLIHelper.GetString("What name should the reservation be made under? ");

                        ReservationSqlDAL rDal = new ReservationSqlDAL(ConnectionString);
                        int reservationConfirmation = rDal.AddReservation(newReservation);
                        Console.WriteLine("The reservation has been made and the confirmation id is {0}", reservationConfirmation);
                    }
                    break;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Thank you for using our reservation system.");
            Console.WriteLine("Please press enter to return to the main menu.");
            returnToMainMenu = true;
            Console.ReadLine();

        }
    }
}
