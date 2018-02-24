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

        private const int PADDING_SMALL = 11;
        private const int PADDING_BIG = 15;
        private const int PADDING_BIGGER = 35;

        private string Max_Rv_Length_String => Max_Rv_Length > 0 ? Max_Rv_Length.ToString() : "N/A";

        public string InformationString(int numOfDays) =>
            $"{Site_Number,-PADDING_SMALL}" +
            $"{Max_Occupancy,-PADDING_SMALL}" +
            $"{Accessible,-PADDING_BIG}" +
            $"{Max_Rv_Length_String,-PADDING_BIG}" +
            $"{Utilities,-PADDING_SMALL}" + 
            $"{(numOfDays * Daily_Fee).ToString("C")}";

        public string InformationStringWithCg(int numOfDays, int paddingBigger) =>
            Campground_Name.PadRight(paddingBigger) +
            $"{Site_Number,-PADDING_SMALL}" +
            $"{Max_Occupancy,-PADDING_SMALL}" +
            $"{Accessible,-PADDING_BIG}" +
            $"{Max_Rv_Length_String,-PADDING_BIG}" +
            $"{Utilities,-PADDING_SMALL}" +
            $"{(numOfDays * Daily_Fee).ToString("C")}";

        public static string Header => 
            "Site No".PadRight(PADDING_SMALL) + 
            "Max Occup.".PadRight(PADDING_SMALL) + 
            "Accessible?".PadRight(PADDING_BIG) + 
            "Max RV Length".PadRight(PADDING_BIG) + 
            "Utility".PadRight(PADDING_SMALL) + "Cost";

        public static string HeaderWithCg(int paddingBigger) =>
            "Campground".PadRight(paddingBigger) +
            "Site No".PadRight(PADDING_SMALL) +
            "Max Occup.".PadRight(PADDING_SMALL) +
            "Accessible?".PadRight(PADDING_BIG) +
            "Max RV Length".PadRight(PADDING_BIG) +
            "Utility".PadRight(PADDING_SMALL) + "Cost";
    }
}
