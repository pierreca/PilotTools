using AviationMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

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
            int id, airportId, length, width, identifier, threshold;
            double lat, lng, alt;

            var fields = s.Split(',');
            int.TryParse(fields[0], out id);
            int.TryParse(fields[1], out airportId);
            int.TryParse(fields[3], out length);
            int.TryParse(fields[4], out width);

            var result = new Runway()
                {
                    Id = id,
                    AirportId = airportId,
                    Length = length,
                    Width = width,
                    Surface = fields[5].Trim('"'),
                    Lighted = parseBool(fields[6]),
                    Closed = parseBool(fields[7]),
                };


            int.TryParse(fields[8].Trim('"'), out identifier);
            int.TryParse(fields[12].Trim('"'), out threshold);
            double.TryParse(fields[9].Trim('"'), out lat);
            double.TryParse(fields[10].Trim('"'), out lng);
            double.TryParse(fields[11].Trim('"'), out alt);

            result.End1 = new RunwayEnd()
            {
                Identifier = identifier,
                DisplacedThreshold = threshold,
                Position = new BasicGeoposition()
                {
                    Altitude = UnitConverter.FeetToMeters(alt),
                    Latitude = lat,
                    Longitude = lng
                }
            };

            int.TryParse(fields[14].Trim('"'), out identifier);
            int.TryParse(fields[18].Trim('"'), out threshold);
            double.TryParse(fields[15].Trim('"'), out lat);
            double.TryParse(fields[16].Trim('"'), out lng);
            double.TryParse(fields[17].Trim('"'), out alt);

            result.End2 = new RunwayEnd()
            {
                Identifier = identifier,
                DisplacedThreshold = threshold,
                Position = new BasicGeoposition()
                {
                    Altitude = UnitConverter.FeetToMeters(alt),
                    Latitude = lat,
                    Longitude = lng
                }
            };

            return result;
        }

        private static bool parseBool(string boolStr)
        {
                int tmp = int.MinValue;
                int.TryParse(boolStr, out tmp);

                return tmp == 1;
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