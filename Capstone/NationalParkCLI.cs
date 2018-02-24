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

        /// <summary>
        /// Runs the CLI for the national park system.
        /// </summary>
        public void Run()
        {
            ParkSqlDAL p = new ParkSqlDAL(ConnectionString);
            List<Park> parks = p.GetAllParks();
            parks.Insert(0, null); // 1-index parks to align with menu options

            while (true)
            {
                DisplayParkSelectionMenu(parks);
                int parkSelection = CLIHelper.GetIntegerInRange(">>", 1, parks.Count);

                if (parkSelection == parks.Count)
                {
                    // Quit at Main Menu
                    break;
                }

                while (!returnToMainMenu)
                {
                    DisplayParkDetailMenu(parks[parkSelection]);
                    int parkOption = CLIHelper.GetIntegerInRange(">>", 1, 3);

                    if (parkOption == 1)
                    {
                        DisplayCampgroundsList(parks[parkSelection]);
                        DisplayCampgroundsMenu();

                        int campgroundOption = CLIHelper.GetIntegerInRange(">>", 1, 2);

                        if (campgroundOption == 1)
                        {
                            List<Campground> campgrounds = DisplayCampgroundsList(parks[parkSelection]);
                            int cgSelection = CLIHelper.GetIntegerInRange("Which campground (enter 0 to cancel)?",
                                0, campgrounds.Count - 1);

                            if (cgSelection != 0)
                            {
                                Tuple<DateTime, DateTime> dateRange = GetDateRange();
                                if (dateRange == null)
                                {
                                    break;
                                }
                                DateTime arrivalDate = dateRange.Item1;
                                DateTime departureDate = dateRange.Item2;

                                List<Site> availableSites = SearchForAvailableSites(campgrounds[cgSelection], arrivalDate, departureDate);
                                ActOnAvailableSites(availableSites, arrivalDate, departureDate);

                                CompletionMessageAndReturnToMainMenu();
                            }
                        }
                    }
                    else if (parkOption == 2)
                    {
                        Tuple<DateTime, DateTime> dateRange = GetDateRange();
                        if (dateRange == null)
                        {
                            break;
                        }
                        DateTime arrivalDate = dateRange.Item1;
                        DateTime departureDate = dateRange.Item2;
                        int numberOfNights = (int)(departureDate - arrivalDate).TotalDays;

                        List<Site> availableSites = SearchForAvailableSites(parks[parkSelection], arrivalDate, departureDate);
                        ActOnAvailableSites(availableSites, arrivalDate, departureDate);

                        CompletionMessageAndReturnToMainMenu();
                    }
                    else if (parkOption == 3)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Allow the user to enter information for a reservation and make the reservation for
        /// a list of available sites and given duration.
        /// </summary>
        /// <param name="availableSites"></param>
        /// <param name="arrivalDate"></param>
        /// <param name="departureDate"></param>
        private void ActOnAvailableSites(List<Site> availableSites, DateTime arrivalDate, DateTime departureDate)
        {
            int numberOfNights = (int)(departureDate - arrivalDate).TotalDays;
            List<int> site_numbers = availableSites.Select(s => s.Site_Number).ToList();
            site_numbers.Add(0);

            DisplayAvailableSitesMenu(availableSites, numberOfNights);

            if (site_numbers.Distinct().Count() != site_numbers.Count)
            {
                Console.WriteLine("Sorry! There are duplicate site numbers across different parks.");
                Console.WriteLine("To make a reservation, please repeat the search for an individual campground.");
                return;
            }

            int reservationChoice = CLIHelper.GetIntegerInRange("Which site should be reserved (enter 0 to cancel)?",
                site_numbers, "Invalid selection. Please select a site id.");

            if (reservationChoice != 0)
            {
                string bookingName = CLIHelper.GetString("What name should the reservation be made under? ");

                MakeReservation(arrivalDate, departureDate, bookingName,
                    availableSites.Where(s => s.Site_Number == reservationChoice).First());
            }
        }

        /// <summary>
        /// Calls the CLIHelper method to get a date range.
        /// </summary>
        /// <returns></returns>
        private Tuple<DateTime, DateTime> GetDateRange()
        {
            return CLIHelper.GetFutureDateRange(
                "What is the arrival date? (MM/DD/YYYY): ",
                "What is the departure date? (MM/DD/YYYY): ",
                "Departure date must be at least one day after arrival date.",
                true,
                "1234",
                "Post-dated reservations require administrative access.",
                "Please enter password to continue (0 to quit) (hint: 1234): ");
        }

        /// <summary>
        /// Displays the details for one park, and provides a menu of options related
        /// to that park.
        /// </summary>
        /// <param name="park"></param>
        private static void DisplayParkDetailMenu(Park park)
        {
            Console.Clear();
            Console.WriteLine(park);

            Console.WriteLine();
            Console.WriteLine("  1)  View Campgrounds");
            Console.WriteLine("  2)  Search for Reservation (Whole Park)");
            Console.WriteLine("  3)  Return to Previous Screen");
        }

        /// <summary>
        /// Displays all parks.
        /// </summary>
        /// <param name="parks"></param>
        private void DisplayParkSelectionMenu(List<Park> parks)
        {
            Console.Clear();
            Console.WriteLine("Select a park for further details..");
            returnToMainMenu = false;

            for (int i = 1; i < parks.Count; i++)
            {
                Console.WriteLine("  " + i + ")  " + parks[i].Name);
            }
            Console.WriteLine("  " + parks.Count + ")  Quit ");
        }

        /// <summary>
        /// Displays all campgrounds at the selected park.
        /// </summary>
        /// <param name="park"></param>
        /// <returns></returns>
        private List<Campground> DisplayCampgroundsList(Park park)
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

        /// <summary>
        /// Displays the menu to take an action on a campground.
        /// </summary>
        private void DisplayCampgroundsMenu()
        {
            Console.WriteLine("Select a Command");
            Console.WriteLine("  1) Search for Available Reservation");
            Console.WriteLine("  2) Return to Previous Screen");
        }

        /// <summary>
        /// Searches for available sites at a given campground and given timespan.
        /// </summary>
        /// <param name="campground">The campground for the search taking place.</param>
        private List<Site> SearchForAvailableSites(Campground campground, DateTime arrivalDate, DateTime departureDate)
        {

            List<Site> availableSites = new List<Site>();

            while (availableSites.Count == 0)
            {
                SiteSqlDAL ssDal = new SiteSqlDAL(ConnectionString);
                availableSites.AddRange(ssDal.GetAvailableSites(campground, arrivalDate, departureDate));

                if (availableSites.Count == 0)
                {
                    string wantsAlternateRange = CLIHelper.GetString(
                        "No available sites. Would you like to enter an alternate date range? (yes or no): ");

                    if (wantsAlternateRange.ToLower() != "yes")
                    {
                        break;
                    }
                }
            }

            return availableSites;
        }

        /// <summary>
        /// Searches for available sites at a given park and given timespan.
        /// </summary>
        /// <param name="campground">The campground for the search taking place.</param>
        private List<Site> SearchForAvailableSites(Park park, DateTime arrivalDate, DateTime departureDate)
        {

            List<Site> availableSites = new List<Site>();

            CampgroundSqlDAL cgDAL = new CampgroundSqlDAL(ConnectionString);
            List<Campground> campgrounds = cgDAL.GetCampgroundsAtPark(park.Park_id);

            while (availableSites.Count == 0)
            {
                foreach (Campground campground in campgrounds)
                {
                    SiteSqlDAL ssDal = new SiteSqlDAL(ConnectionString);
                    availableSites.AddRange(ssDal.GetAvailableSites(campground, arrivalDate, departureDate));
                }

                if (availableSites.Count == 0)
                {
                    string wantsAlternateRange = CLIHelper.GetString(
                        "No available sites. Would you like to enter an alternate date range? (yes or no): ");

                    if (wantsAlternateRange.ToLower() != "yes")
                    {
                        break;
                    }
                }
            }

            return availableSites;
        }

        /// <summary>
        /// Displays a message thanking the user and returns to the main menu.
        /// </summary>
        private void CompletionMessageAndReturnToMainMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Thank you for using our reservation system.");
            Console.WriteLine("Please press enter to return to the main menu.");
            returnToMainMenu = true;
            Console.ReadLine();
        }

        /// <summary>
        /// Adds a reservation to the database at the selected site, under the provided name, and for
        /// the provided duration.
        /// </summary>
        /// <param name="arrivalDate">The date that the guest will arrive at the site.</param>
        /// <param name="departureDate">The date that the guest will leave the site.</param>
        /// <param name="name">A name to identify the guest by.</param>
        /// <param name="site">The site being reserved.</param>
        private void MakeReservation(DateTime arrivalDate, DateTime departureDate, string name, Site site)
        {
            Reservation newReservation = new Reservation();
            newReservation.Site_Id = site.Site_Id;
            newReservation.From_Date = arrivalDate;
            newReservation.To_Date = departureDate;
            newReservation.Name = name;

            ReservationSqlDAL rDal = new ReservationSqlDAL(ConnectionString);
            int reservationConfirmation = rDal.AddReservation(newReservation);
            Console.WriteLine("The reservation has been made and the confirmation id is {0}", reservationConfirmation);
        }

        /// <summary>
        /// Display information on all available sites. The total cost is calculated as the daily fee times
        /// the number of nights.
        /// </summary>
        /// <param name="availableSites">A list of sites that the user may select from.</param>
        /// <param name="numberOfNights">The number of nights that the guest will be staying.</param>
        private static void DisplayAvailableSitesMenu(List<Site> availableSites, int numberOfNights)
        {
            if (availableSites.GroupBy(s => s.Campground_Name).Count() == 1)
            {
                Console.WriteLine(Site.Header);

                foreach (Site site in availableSites)
                {
                    Console.WriteLine(site.InformationString(numberOfNights));
                }
            }
            else
            {
                int biggestNameLength = availableSites.Select(s => s.Campground_Name).OrderBy(n => n.Length).Last().Length + 2;

                Console.WriteLine(Site.HeaderWithCg(biggestNameLength));

                foreach (Site site in availableSites)
                {
                    Console.WriteLine(site.InformationStringWithCg(numberOfNights, biggestNameLength));
                }
            }

            Console.WriteLine();
        }
    }
}
