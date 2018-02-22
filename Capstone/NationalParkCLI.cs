using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using Capstone.DAL;


namespace Capstone
{
    public class NationalParkCLI
    {

        public void Run(string connectionString)
        {
            ParkSqlDAL p = new ParkSqlDAL(connectionString);
            List<Park> parks = p.GetAllParks();
            parks.Insert(0, null);
            while (true)
            {
                Console.WriteLine("Select a park for further details..");

                for (int i = 1; i < parks.Count; i++)
                {
                    Console.WriteLine("  " + i + ")  " + parks[i].Name);
                }
                Console.WriteLine("  " + parks.Count + ")  Quit ");
                CLIHelper.GetInteger(">>");
            }
        }
    }
}
