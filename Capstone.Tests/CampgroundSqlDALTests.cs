using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Configuration;
using System.Transactions;
using Capstone.Models;
using Capstone.DAL;
using System.Linq;

namespace Capstone.Tests
{
    [TestClass]
    public class CampgroundSqlDALTests
    {
        [TestMethod]
        public void GetCampgroundsAtPark_InsertedCampgroundExists()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                CampgroundSqlDAL testClass = new CampgroundSqlDAL(connectionString);

                Park tempPark = new Park();
                tempPark.Area = 0;
                tempPark.Description = "TEST DESCRIPTION";
                tempPark.Establish_Date = new DateTime(2000, 1, 1).Date;
                tempPark.Location = "TEST LOCATION";
                tempPark.Name = "TEST PARK";
                tempPark.Visitors = 0;
                int tempPark_parkId = ParkSqlDALTests.InsertFakePark(tempPark);

                Campground testCamp = new Campground();
                testCamp.Daily_Fee = 5.00M;
                testCamp.Name = "Slimy Valley";
                testCamp.Open_From_MM = 5;
                testCamp.Open_To_MM = 6;
                testCamp.Park_Id = tempPark_parkId;
                testCamp.Campground_Id = InsertFakeCampground(testCamp);

                Campground insertedCampground = testClass.GetCampgroundsAtPark(tempPark_parkId).First();

                Assert.IsNotNull(insertedCampground);
            }
            

        }

        static string connectionString = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;

        public static int InsertFakeCampground(Campground camp)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO campground (park_id, name, open_from_mm, open_to_mm, daily_fee) " +
                                                "VALUES (@park_id, @name, @open_from_mm, @open_to_mm, @daily_fee)", conn);
                cmd.Parameters.AddWithValue("@park_id", camp.Park_Id);
                cmd.Parameters.AddWithValue("@name", camp.Name);
                cmd.Parameters.AddWithValue("@open_from_mm", camp.Open_From_MM);
                cmd.Parameters.AddWithValue("@open_to_mm", camp.Open_To_MM);
                cmd.Parameters.AddWithValue("@daily_fee", camp.Daily_Fee);

                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("SELECT campground_id FROM campground WHERE park_id = @park_id AND name = @name AND open_from_mm = @open_from_mm " +
                    "AND open_to_mm = @open_to_mm AND daily_fee = @daily_fee;", conn);
                cmd.Parameters.AddWithValue("@park_id", camp.Park_Id);
                cmd.Parameters.AddWithValue("@name", camp.Name);
                cmd.Parameters.AddWithValue("@open_from_mm", camp.Open_From_MM);
                cmd.Parameters.AddWithValue("@open_to_mm", camp.Open_To_MM);
                cmd.Parameters.AddWithValue("@daily_fee", camp.Daily_Fee);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}
