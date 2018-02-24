using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class ParkSqlDAL
    {
        private string ConnectionString { get; set; }

        public ParkSqlDAL (string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        /// <summary>
        /// Fetches all national parks from the database.
        /// </summary>
        /// <returns></returns>
        public List<Park> GetAllParks()
        {
            List<Park> parkList = new List<Park>();

            try
            {
                using(SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM park ORDER BY name;", conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Park p = new Park();

                        p.Area = Convert.ToInt32(reader["area"]);
                        p.Description = Convert.ToString(reader["description"]);
                        p.Establish_Date = Convert.ToDateTime(reader["establish_date"]);
                        p.Location = Convert.ToString(reader["location"]);
                        p.Name = Convert.ToString(reader["name"]);
                        p.Park_id = Convert.ToInt32(reader["park_id"]);
                        p.Visitors = Convert.ToInt32(reader["visitors"]);

                        parkList.Add(p);
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Something went wrong reading the database: \n" + e.Message);
                throw;
            }

            return parkList;
        }

    }
}
