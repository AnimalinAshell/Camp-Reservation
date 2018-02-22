using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class SiteSqlDAL
    {
        private string ConnectionString { get; set; }

        public SiteSqlDAL(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        string reservationQuery;

        
        public List<Site> GetAvailableSites(int campground_Id, DateTime from_Date, DateTime to_Date)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(reservationQuery, conn);
                    cmd.
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error something went wrong: " + ex.Message);
            }
        }
    }
}
