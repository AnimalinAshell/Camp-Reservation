using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Park
    {
        public int Park_id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime Establish_Date { get; set; }
        public int Area { get; set; }
        public int Visitors { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return Name + " National Park" + "\n" +
                   "Locations:".PadRight(17) + Location + "\n" +
                   "Established:".PadRight(17) + Establish_Date.ToShortDateString() + "\n" +
                   "Area:".PadRight(17) + Area.ToString() + " sq km" + "\n" +
                   "Annual Visitors:".PadRight(17) + $"{Visitors:###,###,###,##0}" + "\n\n" +
                   Description;
        }
    }
}
