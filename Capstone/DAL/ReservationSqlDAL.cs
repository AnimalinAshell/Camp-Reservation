using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class ReservationSqlDAL
    {
        private string ConnectionString { get; set; }

        public ReservationSqlDAL(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public int AddReservation(Reservation reservation)//Site site, string name, DateTime arrival_date, DateTime departure_date)
        {
            int reservation_id = 0;

            try
            {
                using(SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (@site_id, @name, @from_date, @to_date)", conn);
                    cmd.Parameters.AddWithValue("@site_id", reservation.Site_Id);
                    cmd.Parameters.AddWithValue("@name", reservation.Name + " Reservation");
                    cmd.Parameters.AddWithValue("@from_date", reservation.From_Date.Date);
                    cmd.Parameters.AddWithValue("@to_date", reservation.To_Date.Date);

                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("SELECT reservation_id FROM reservation WHERE site_id = @site_id AND name = @name AND from_date = @from_date AND to_date = @to_date", conn);
                    cmd.Parameters.AddWithValue("@site_id", reservation.Site_Id);
                    cmd.Parameters.AddWithValue("@name", reservation.Name + " Reservation");
                    cmd.Parameters.AddWithValue("@from_date", reservation.From_Date.Date);
                    cmd.Parameters.AddWithValue("@to_date", reservation.To_Date.Date);

                    reservation_id = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Something went wrong with the SQL query.\n" + e.Message);
                throw;
            }


            return reservation_id;
        }
    }
}
