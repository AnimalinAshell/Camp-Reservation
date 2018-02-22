using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Campground
    {

        private const int NAME_PAD = 17;
        private const int DATE_PAD = 12;

        public int Campground_Id { get; set; }
        public int Park_Id { get; set; }
        public string Name { get; set; }
        public int Open_From_MM { get; set; }
        public int Open_To_MM { get; set; }
        public decimal Daily_Fee { get; set; }

        public override string ToString()
        {
            return $"{Name,-NAME_PAD}" +
                    $"{new DateTime(2000, Open_From_MM, 2).ToString("MMM"),-DATE_PAD}" +
                    $"{new DateTime(2000, Open_To_MM, 2).ToString("MMM"),-DATE_PAD}" +
                    $"{Daily_Fee}";
        }

        public static string Header => "Name".PadRight(NAME_PAD) + "Open".PadRight(DATE_PAD) + "Close".PadRight(DATE_PAD) + "Daily Fee";
    }
}
