using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
using Capstone.DAL;
using Capstone.Models;
using System.Configuration;

namespace Capstone.Tests
{
    [TestClass]
    public class ReservationnSqlDALTests
    {
        Park tempPark = new Park();
        Campground testCamp = new Campground();
        Site testSite = new Site();
        int siteId;

        private void ManualInitialize()
        {
            // Put a temporary park into the database
            tempPark.Area = 0;
            tempPark.Description = "TEST DESCRIPTION";
            tempPark.Establish_Date = new DateTime(2000, 1, 1).Date;
            tempPark.Location = "TEST LOCATION";
            tempPark.Name = "TEST PARK";
            tempPark.Visitors = 0;
            int tempPark_parkId = ParkSqlDALTests.InsertFakePark(tempPark);

            // Put a temporary campground into the database
            // Record the id into a class scope int variable
            testCamp.Daily_Fee = 5.00M;
            testCamp.Name = "Slimy Valley";
            testCamp.Open_From_MM = 1;
            testCamp.Open_To_MM = 12;
            testCamp.Park_Id = tempPark_parkId;

            // put a temporary site into the database
            // Record the id into a class scope int variable
            testCamp.Campground_Id = CampgroundSqlDALTests.InsertFakeCampground(testCamp);
            testSite.Campground_Id = testCamp.Campground_Id;
            testSite.Site_Number = 1;
            siteId = SiteSqlDALTests.InsertFakeSite(testSite);
        }

        [TestMethod]
        [TestCategory("Reservation SQL DAL")]
        public void AddReservation_Succeeds()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                // Arrange
                ReservationSqlDAL testClass = new ReservationSqlDAL(connectionString);
                ManualInitialize();
                DateTime testStartDate = new DateTime(2000, 6, 10).Date;
                DateTime testEndDate = new DateTime(2000, 6, 20).Date;
                Reservation tempReservation = new Reservation();
                tempReservation.Site_Id = siteId;
                tempReservation.Name = "TEMP RESERVATION";
                tempReservation.From_Date = testStartDate;
                tempReservation.To_Date = testEndDate;

                // Act
                int reservationId = testClass.AddReservation(tempReservation);

                // Assert
                Assert.IsTrue(reservationId > 0);
            }
        }

        static string connectionString = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;
    }
}
