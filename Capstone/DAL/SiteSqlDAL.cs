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

                        sites.Add(s);
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

                        sites.Add(s);
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
            "SELECT TOP 5                                                                   " + "\n" +
            "   s.site_number,                                                              " + "\n" +
            "	s.max_occupancy,                                                            " + "\n" +
            "	s.accessible,                                                               " + "\n" +
            "	s.max_rv_length,                                                            " + "\n" +
            "	s.utilities,                                                                " + "\n" +
            "	c.daily_fee                                                                 " + "\n" +
            "FROM site s                                                                    " + "\n" +
            "JOIN campground c ON c.campground_id = s.campground_id                         " + "\n" +
            "WHERE                                                                          " + "\n" +
            "    c.campground_id = @input_campground_id AND                                 " + "\n" +
            "    c.open_from_mm <= MONTH(@input_from_date) AND                              " + "\n" +
            "    c.open_to_mm >= MONTH(@input_to_date) AND                                  " + "\n" +
            "    ((c.open_from_mm = 1 AND c.open_to_mm = 12) OR                             " + "\n" +
            "    (YEAR(@input_from_date) = YEAR(@input_to_date))) AND                       " + "\n" +
            "    s.site_id NOT IN (                                                         " + "\n" +
            "        SELECT r.site_id                                                       " + "\n" +
            "        FROM reservation r                                                     " + "\n" +
            "        WHERE                                                                  " + "\n" +
            "            (@input_to_date > from_date AND @input_to_date <= to_date) OR      " + "\n" +
            "            (@input_from_date >= from_date AND @input_from_date<to_date) OR    " + "\n" +
            "            (@input_from_date <= from_date AND @input_to_date > to_date)       " + "\n" +
            "	);                                                                          ";

        static string AvailableSitesFromPark =
            "SELECT                                                                         " + "\n" +
            "   s.site_number,                                                              " + "\n" +
            "	s.max_occupancy,                                                            " + "\n" +
            "	s.accessible,                                                               " + "\n" +
            "	s.max_rv_length,                                                            " + "\n" +
            "	s.utilities,                                                                " + "\n" +
            "	c.daily_fee                                                                 " + "\n" +
            "FROM site s                                                                    " + "\n" +
            "JOIN campground c ON c.campground_id = s.campground_id                         " + "\n" +
            "WHERE                                                                          " + "\n" +
            "    c.park_id = @input_park_id AND                                             " + "\n" +
            "    c.open_from_mm <= MONTH(@input_from_date) AND                              " + "\n" +
            "    c.open_to_mm >= MONTH(@input_to_date) AND                                  " + "\n" +
            "    ((c.open_from_mm = 1 AND c.open_to_mm = 12) OR                             " + "\n" +
            "    (YEAR(@input_from_date) = YEAR(@input_to_date))) AND                       " + "\n" +
            "    s.site_id NOT IN (                                                         " + "\n" +
            "        SELECT r.site_id                                                       " + "\n" +
            "        FROM reservation r                                                     " + "\n" +
            "        WHERE                                                                  " + "\n" +
            "            (@input_to_date > from_date AND @input_to_date <= to_date) OR      " + "\n" +
            "            (@input_from_date >= from_date AND @input_from_date<to_date) OR    " + "\n" +
            "            (@input_from_date <= from_date AND @input_to_date > to_date)       " + "\n" +
            "	);                                                                          ";
    }
}
