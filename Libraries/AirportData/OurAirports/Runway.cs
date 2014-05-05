using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportData.OurAirports
{
    public class Runway : IRunway
    {
        public RunwayEnd End1 { get; set; }
        public RunwayEnd End2 { get; set; }

        public int Id { get; set; }
        public int AirportId { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }

        public bool Lighted { get; set; }
        public bool Closed { get; set; }

        public string Surface { get; set; }

        public static IRunway CreateFromString(string s)
        {
            var result = new Runway();

            var fields = s.Split(',');
            result.Id = int.Parse(fields[0]);
            result.AirportId = int.Parse(fields[1]);
            result.Length = int.Parse(fields[3]);
            result.Width = int.Parse(fields[4]);
            result.Surface = fields[5];
            result.Lighted = (int.Parse(fields[6]) == 1);
            result.Closed = (int.Parse(fields[7]) == 1);

            result.End1 = new RunwayEnd();
            result.End1.Identifier = int.Parse(fields[8]);
            result.End1.Latitude = int.Parse(fields[9]);
            result.End1.Longitude = int.Parse(fields[10]);
            result.End1.Elevation = int.Parse(fields[11]);
            result.End1.DisplacedThreshold = int.Parse(fields[12]);

            result.End2 = new RunwayEnd();
            result.End2.Identifier = int.Parse(fields[13]);
            result.End2.Latitude = int.Parse(fields[14]);
            result.End2.Longitude = int.Parse(fields[15]);
            result.End2.Elevation = int.Parse(fields[16]);
            result.End2.DisplacedThreshold = int.Parse(fields[17]);

            return result;
        }
    }
}

/* Runway data from OurAirports.com uses the following schema in the CSV file:
 * "id",
 * "airport_ref",
 * "airport_ident",
 * "length_ft",
 * "width_ft",
 * "surface",
 * "lighted",
 * "closed",
 * "le_ident",
 * "le_latitude_deg",
 * "le_longitude_deg",
 * "le_elevation_ft",
 * "le_heading_degT",
 * "le_displaced_threshold_ft",
 * "he_ident",
 * "he_latitude_deg",
 * "he_longitude_deg",
 * "he_elevation_ft",
 * "he_heading_degT",
 * "he_displaced_threshold_ft"
 * 
 * For example at Renton Municipal airport: 
 * 244585,20947,"KRNT",5382,200,"ASPH-CONC-G",1,0,"16",47.5005,-122.217,24,174,300,"34",47.4858,-122.215,32,354,340
 * 
 */