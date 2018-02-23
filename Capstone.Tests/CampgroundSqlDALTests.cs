using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Configuration;
using System.Transactions;

namespace Capstone.Tests
{
    [TestClass]
    public class CampgroundSqlDALTests
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        string connectionString = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;

        public int InsertFakeCampground()
        {
            int campgroundId = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO campground ", conn);

            }



            return campgroundId;
        }
    }
}
