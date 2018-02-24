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
        [TestMethod]
        [TestCategory("Reservation SQL DAL")]
        public void AddReservation_Succeeds()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                // Arrange
                ReservationSqlDAL testClass = new ReservationSqlDAL(connectionString);
                Reservation testReservation = new Reservation();
                

                // Act

                // Assert
            }
        }

        static string connectionString = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;
    }
}
