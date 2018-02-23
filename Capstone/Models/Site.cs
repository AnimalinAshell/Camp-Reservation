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
        public string Campground_Name { get; set; }
        public int Site_Number { get; set; }
        public int Max_Occupancy { get; set; }
        public string Accessible { get; set; }
        public int Max_Rv_Length { get; set; }
        public string Utilities { get; set; }
        public decimal Daily_Fee { get; set; }

        private const int PADDING = 15;
        private string Max_Rv_Length_String => Max_Rv_Length > 0 ? Max_Rv_Length.ToString() : "N/A";
        public override string ToString()
        {
            return $"{Site_Number,-PADDING}" +
                    $"{Max_Occupancy,-PADDING}" +
                    $"{Accessible,-PADDING}" +
                    $"{Max_Rv_Length_String,-PADDING}" +
                    $"{Utilities,-PADDING}" + 
                    $"{Daily_Fee.ToString("C")}";
        }

        public static string Header => "Site No".PadRight(PADDING) + "Max Occup.".PadRight(PADDING) + "Accessible?".PadRight(PADDING) + 
            "Max RV Length".PadRight(PADDING) + "Utility".PadRight(PADDING) + "Cost";
    }
}
