using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Campground
    {
        public int Campground_Id { get; set; }
        public int Park_Id { get; set; }
        public string Name { get; set; }
        public int Open_From_MM { get; set; }
        public int Open_To_MM { get; set; }
        public decimal Daily_Fee { get; set; }

        public override string ToString()
        {
            return $"{Name,-17} " +
                    $"{new DateTime(2000, Open_From_MM, 2),-12} " +
                    $"{new DateTime(2000, Open_To_MM, 2),-12} " +
                    $"{Daily_Fee}";
        }
    }
}
