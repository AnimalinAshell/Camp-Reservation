using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Site
    {
        public int Site_Id { get; set; }
        public int Campground_Id { get; set; }
        public int Site_Number { get; set; }
        public int Max_Occupancy { get; set; }
        public int Accessible { get; set; }
        public int Max_Rv_Length { get; set; }
        public int Utilities { get; set; }
        public int Daily_Fe { get; set; }
        
    }
}
