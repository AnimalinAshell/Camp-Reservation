using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Reservation
    {
        public int Reservation_Id { get; set; }
        public int Site_Id { get; set; }
        public string Name { get; set; }
        public DateTime From_Date { get; set; }
        public DateTime To_Date { get; set; }
        public DateTime Create_Date { get; set; }

    }
}
