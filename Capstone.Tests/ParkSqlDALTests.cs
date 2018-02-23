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
    public class ParkSqlDALTests
    {
        [TestMethod]
        [TestCategory("Park SQL DAL")]
        public void GetAllParks_ReturnsNewlyAddedPark ()
        {
            using(TransactionScope transaction = new TransactionScope())
            {
                // Arrange
                ParkSqlDAL testClass = new ParkSqlDAL(connectionString);
                Park tempPark = new Park();
                tempPark.Area = 0;
                tempPark.Description = "TEST DESCRIPTION";
                tempPark.Establish_Date = new DateTime(2000, 1, 1).Date;
                tempPark.Location = "TEST LOCATION";
                tempPark.Name = "TEST PARK";
                tempPark.Visitors = 0;
                int tempPark_parkId = InsertFakePark(tempPark);

                // Act
                List<Park> parks = testClass.GetAllParks();

                // Assert
                Assert.IsTrue(parks.Select(p => p.Park_id).Contains(tempPark_parkId));
            }
        }

        static string connectionString = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;

        /// <summary>
        /// Inserts a park into the database.
        /// </summary>
        /// <param name="park"></param>
        /// <returns>Park_id of the new park.</returns>
        public static int InsertFakePark(Park park)
        {
            int campgroundId = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO park (name, location, establish_date, area, visitors, description)" +
                                                "VALUES (@name, @location, @establish_date, @area, @visitors, @description)", conn);
                cmd.Parameters.AddWithValue("@name", park.Name);
                cmd.Parameters.AddWithValue("@location", park.Location);
                cmd.Parameters.AddWithValue("@establish_date", park.Establish_Date);
                cmd.Parameters.AddWithValue("@area", park.Area);
                cmd.Parameters.AddWithValue("@visitors", park.Visitors);
                cmd.Parameters.AddWithValue("@description", park.Description);

                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("SELECT park_id FROM park WHERE name = @name AND location = @location AND establish_date = @establish_date AND " +
                                     "area = @area AND visitors = @visitors AND description = @description", conn);
                cmd.Parameters.AddWithValue("@name", park.Name);
                cmd.Parameters.AddWithValue("@location", park.Location);
                cmd.Parameters.AddWithValue("@establish_date", park.Establish_Date);
                cmd.Parameters.AddWithValue("@area", park.Area);
                cmd.Parameters.AddWithValue("@visitors", park.Visitors);
                cmd.Parameters.AddWithValue("@description", park.Description);

                campgroundId = Convert.ToInt32(cmd.ExecuteScalar());
            }
            
            return campgroundId;
        }
    }
}
