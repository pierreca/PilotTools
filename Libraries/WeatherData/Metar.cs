using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherData
{
    public class Metar
    {
        public bool IsValid { get; set; }
        public string Raw { get; set; }

        public dynamic MetarObj { get; set; }

        public string Wind { get; set; }
        public int Visibility { get; set; }
        public int Temperature { get; set; }
        public int DewPoint { get; set; }
        public IEnumerable<CloudLayer> Clouds { get; set; }

        private FlightRules applicableFlightRules = FlightRules.UNKNOWN;
        public FlightRules ApplicableFlightRules 
        { 
            get
            {
                if(applicableFlightRules == FlightRules.UNKNOWN)
                {
                    applicableFlightRules = this.ComputeFlightRules();
                }

                return this.applicableFlightRules;
            }

        }

        public FlightRules ComputeFlightRules()
        {
            FlightRules result = FlightRules.UNKNOWN;

            var ceiling = this.Clouds.OrderBy(layer => layer.Altitude)
                                     .Where(layer => layer.IsCeiling)
                                     .First();

            if (this.Visibility < 3 || (ceiling != null && ceiling.Altitude < 1000))
            {
                result = FlightRules.IFR;
            }
            else
            {
                if (ceiling != null && ceiling.Altitude <= 3000)
                {
                    result = FlightRules.MVFR;
                }
                else
                {
                    result = FlightRules.VFR;
                }
            }

            return result;
        }
    }
}
