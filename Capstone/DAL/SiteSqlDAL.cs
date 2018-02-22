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


        public List<Site> GetAvailableSites(Campground campground, DateTime from_Date, DateTime to_Date)
        {
            List<Site> sites = new List<Site>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(AvailableSitesFromCampground, conn);
                    cmd.Parameters.AddWithValue("@input_campground_id", campground.Campground_Id);
                    cmd.Parameters.AddWithValue("@input_from_date", from_Date.Date);
                    cmd.Parameters.AddWithValue("@input_to_date", to_Date.Date);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Site s = new Site();
                        s.Accessible = Convert.ToBoolean(reader["accessible"]) ? "Yes" : "No";
                        s.Daily_Fee = Convert.ToDecimal(reader["daily_fee"]);
                        s.Max_Occupancy = Convert.ToInt32(reader["max_occupancy"]);
                        s.Max_Rv_Length = Convert.ToInt32(reader["max_rv_length"]);
                        s.Site_Number = Convert.ToInt32(reader["site_number"]);
                        s.Utilities = Convert.ToBoolean(reader["utilities"]) ? "Yes" : "N/A";
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error something went wrong: " + ex.Message);
            }

            return sites;
        }

        public List<Site> GetAvailableSites(Park park, DateTime from_Date, DateTime to_Date)
        {
            List<Site> sites = new List<Site>();

            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(AvailableSitesFromPark, conn);
                    cmd.Parameters.AddWithValue("@input_park_id", park.Park_id);
                    cmd.Parameters.AddWithValue("@input_from_date", from_Date);
                    cmd.Parameters.AddWithValue("@input_to_date", to_Date);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Site s = new Site();
                        s.Accessible = Convert.ToBoolean(reader["accessible"]) ? "Yes" : "No";
                        s.Daily_Fee = Convert.ToDecimal(reader["daily_fee"]);
                        s.Max_Occupancy = Convert.ToInt32(reader["max_occupancy"]);
                        s.Max_Rv_Length = Convert.ToInt32(reader["max_rv_length"]);
                        s.Site_Number = Convert.ToInt32(reader["site_number"]);
                        s.Utilities = Convert.ToBoolean(reader["utilities"]) ? "Yes" : "N/A";
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error something went wrong: " + ex.Message);
            }

            return sites;
        }






        static string AvailableSitesFromCampground =
            "SELECT" +
            "   s.site_number," + "\n" +
            "   s.max_occupancy," + "\n" +
            "   s.accessible," + "\n" +
            "   s.max_rv_length," + "\n" +
            "   s.utilities," + "\n" +
            "   c.daily_fee" + "\n" +
            "FROM site s" + "\n" +
            "LEFT JOIN reservation r ON r.site_id = s.site_id" + "\n" +
            "JOIN campground c ON c.campground_id = s.campground_id" + "\n" +
            "WHERE" + "\n" +
            "    c.campground_id = @input_campground_id AND" + "\n" +
            "    s.site_id NOT IN (" + "\n" +
            "       SELECT rs.site_id" + "\n" +
            "       FROM reservation rs" + "\n" +
            "       WHERE" + "\n" +
            "          (@input_to_date > rs.from_date AND @input_to_date < rs.to_date) OR" + "\n" +
            "          (@input_from_date > rs.from_date AND @input_from_date < rs.to_date)" + "\n" +
            "	)" + "\n" +
            "GROUP BY" + "\n" +
            "    s.site_number," + "\n" +
            "    s.max_occupancy," + "\n" +
            "    s.accessible," + "\n" +
            "    s.max_rv_length," + "\n" +
            "    s.utilities," + "\n" +
            "    c.daily_fee;";

        static string AvailableSitesFromPark =
            "SELECT" +
            "s.site_number," + "\n" +
            "s.max_occupancy," + "\n" +
            "s.accessible," + "\n" +
            "s.max_rv_length," + "\n" +
            "s.utilities," + "\n" +
            "c.daily_fee" + "\n" +
            "FROM site s" + "\n" +
            "LEFT JOIN reservation r ON r.site_id = s.site_id" + "\n" +
            "JOIN campground c ON c.campground_id = s.campground_id" + "\n" +
            "WHERE" + "\n" +
            "    c.park_id = @input_park_id AND" + "\n" +
            "    s.site_id NOT IN (" + "\n" +
            "    SELECT rs.site_id" + "\n" +
            "    FROM reservation rs" + "\n" +
            "    WHERE" + "\n" +
            "       (@input_to_date > rs.from_date AND @input_to_date < rs.to_date) OR" + "\n" +
            "       (@input_from_date > rs.from_date AND @input_from_date < rs.to_date)" + "\n" +
            "	)" + "\n" +
            "GROUP BY" + "\n" +
            "   s.site_number," + "\n" +
            "   s.max_occupancy," + "\n" +
            "   s.accessible," + "\n" +
            "   s.max_rv_length," + "\n" +
            "   s.utilities," + "\n" +
            "   c.daily_fee;";
    }
}
