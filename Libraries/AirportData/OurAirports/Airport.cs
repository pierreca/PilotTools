using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportData.OurAirports
{
    public class Airport : IAirport
    {
        public Airport()
        {
            this.Runways = new List<Runway>();
        }

        public int Id { get; set; }
        public int Elevation { get; set; }
        public Dictionary<RadioFrequencyType, double> Frequencies { get; set; }

        public string ICAO { get; set; }

        public string LocalCode { get; set; }

        public string Name { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public IEnumerable<IRunway> Runways { get; set; }

        public static Airport CreateFromString(string s)
        {
            var result = new Airport();
            
            var fields = s.Split(','); 
            result.Id = int.Parse(fields[0]);
            result.Latitude = double.Parse(fields[4]);
            result.Longitude = double.Parse(fields[5]);
            result.Elevation = int.Parse(fields[6]);
            result.Name = fields[3];
            result.ICAO = fields[12];
            result.LocalCode = fields[14];

            return result;
        }
    }
}


/* Airports data from OurAirports.com uses the following schema in the CSV file:
 * "id"
 * "ident"
 * "type"
 * "name"
 * "latitude_deg"
 * "longitude_deg"
 * "elevation_ft"
 * "continent"
 * "iso_country"
 * "iso_region"
 * "municipality"
 * "scheduled_service"
 * "gps_code"
 * "iata_code"
 * "local_code"
 * "home_link"
 * "wikipedia_link"
 * "keywords"
 * 
 * for example with Renton Municipal Airport:
 * 20947,"KRNT","small_airport","Renton Municipal Airport",47.4930992126465,-122.216003417969,32,"NA","US","US-WA","Renton","no","KRNT",,"RNT",,,
 */