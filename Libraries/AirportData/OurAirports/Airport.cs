using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace AirportData.OurAirports
{
    public class Airport : IAirport
    {
        public Airport()
        {
            this.Runways = new List<Runway>();
        }

        public int Id { get; set; }

        public Dictionary<RadioFrequencyType, double> Frequencies { get; set; }

        public string ICAO { get; set; }

        public string LocalCode { get; set; }

        public string Name { get; set; }

        public BasicGeoposition Position { get; set; }

        public IEnumerable<IRunway> Runways { get; set; }

        public static IAirport CreateFromString(string s)
        {
            int id, alt;
            double lat, lng;

            var fields = s.Split(','); 
            int.TryParse(fields[0], out id);
            double.TryParse(fields[4], out lat);
            double.TryParse(fields[5], out lng);
            int.TryParse(fields[6], out alt);
            
            var result = new Airport()
            {
                Id = id,
                Name = fields[3].Trim('"'),
                ICAO = fields[12].Trim('"'),
                LocalCode = fields[14].Trim('"'),
                Position = new BasicGeoposition()
                {
                    Altitude = alt,
                    Latitude = lat,
                    Longitude = lng
                }
            };

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