using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Configuration;
using System.Transactions;
using Capstone.Models;
using Capstone.DAL;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.Tests
{
    [TestClass]
    public class SiteSqlDALTests
    {
        Park tempPark = new Park();
        Campground testCamp = new Campground();
        Site testSite = new Site();
        int campgroundId;
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
            campgroundId = CampgroundSqlDALTests.InsertFakeCampground(testCamp);
            testCamp.Campground_Id = campgroundId;
            testSite.Campground_Id = campgroundId;
            testSite.Site_Number = 1;
            siteId = SiteSqlDALTests.InsertFakeSite(testSite);
        }


        [TestMethod]
        [TestCategory("Site SQL DAL")]
        public void GetAvailableSites_AdjacentToEarlier_IsAvailable()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                // Arrange
                ManualInitialize();
                SiteSqlDAL testClass = new SiteSqlDAL(connectionString);
                DateTime testStartDate = new DateTime(2000, 6, 10).Date;
                DateTime testEndDate = new DateTime(2000, 6, 20).Date;
                DateTime conflictStartDate = new DateTime(2000, 6, 1).Date;
                DateTime conflictEndDate = new DateTime(2000, 6, 10).Date;
                ReservationSqlDAL rDal = new ReservationSqlDAL(connectionString);
                Reservation tempReservation = new Reservation();
                tempReservation.Site_Id = siteId;
                tempReservation.Name = "TEMP RESERVATION";
                tempReservation.From_Date = conflictStartDate;
                tempReservation.To_Date = conflictEndDate;
                rDal.AddReservation(tempReservation);

                // Act
                List<Site> availableSites = testClass.GetAvailableSites(testCamp, testStartDate, testEndDate);

                // Assert
                Assert.AreEqual(1, availableSites.Count);
            }
        }

        [TestMethod]
        [TestCategory("Site SQL DAL")]
        public void GetAvailableSites_ReturnsNothingIfStartDateConflicts()
        {
        }

        [TestMethod]
        [TestCategory("Site SQL DAL")]
        public void GetAvailableSites_ReturnsNothingIfEndDateConflicts()
        {
        }

        [TestMethod]
        [TestCategory("Site SQL DAL")]
        public void GetAvailableSites_ReturnsNothingIfMiddleConflicts()
        {
        }


        [TestMethod]
        [TestCategory("Site SQL DAL")]
        public void GetAvailableSites_ReturnsNothingIfParkClosed()
        {
        }

        static string connectionString = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;

        /// <summary>
        /// Inserts a site into the database.
        /// </summary>
        /// <param name="site"></param>
        /// <returns>Park_id of the new park.</returns>
        public static int InsertFakeSite(Site site)
        {
            int siteId = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO site (campground_id, site_number) " +
                                                "VALUES (@campground_id, @site_number)", conn);
                cmd.Parameters.AddWithValue("@campground_id", site.Campground_Id);
                cmd.Parameters.AddWithValue("@site_number", site.Site_Number);

                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("SELECT site_id FROM site WHERE campground_id = @campground_id AND " +
                                     "site_number = @site_number", conn);
                cmd.Parameters.AddWithValue("@campground_id", site.Campground_Id);
                cmd.Parameters.AddWithValue("@site_number", site.Site_Number);

                siteId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return siteId;
        }
    }
}
