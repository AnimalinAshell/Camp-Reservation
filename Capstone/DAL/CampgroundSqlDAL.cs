using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class CampgroundSqlDAL
    {
        private string ConnectionString { get; set; }

        public CampgroundSqlDAL(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public List<Campground> GetCampgroundsAtPark(int selectedParkId)
        {
            List<Campground> campgrounds = new List<Campground>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {

                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM campground WHERE park_id = @selectedParkId", conn);
                    cmd.Parameters.AddWithValue("@selectedParkId", selectedParkId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Campground c = new Campground();
                        c.Campground_Id = Convert.ToInt32(reader["campground_id"]);
                        c.Park_Id = Convert.ToInt32(reader["park_id"]);
                        c.Name = Convert.ToString(reader["name"]);
                        c.Open_From_MM = Convert.ToInt32(reader["open_from_mm"]);
                        c.Open_To_MM = Convert.ToInt32(reader["open_to_mm"]);
                        c.Daily_Fee = Convert.ToDecimal(reader["daily_fee"]);

                        campgrounds.Add(c);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error something went wrong " + ex.Message);
            }

            return campgrounds;
        }
    }
}
