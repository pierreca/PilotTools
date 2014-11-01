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

        public DateTime TimePublished { get; set; }

        public Wind Wind { get; set; }
        public int Visibility { get; set; }
        public int Temperature { get; set; }
        public int DewPoint { get; set; }
        public IEnumerable<CloudLayer> Clouds { get; set; }

        private FlightRules applicableFlightRules = FlightRules.UNKNOWN;
        public FlightRules ApplicableFlightRules 
        { 
            get
            {
                if(applicableFlightRules == FlightRules.UNKNOWN && this.IsValid)
                {
                    applicableFlightRules = this.ComputeFlightRules();
                }

                return this.applicableFlightRules;
            }

        }

        public FlightRules ComputeFlightRules()
        {
            FlightRules result = FlightRules.UNKNOWN;

            var ceilings = this.Clouds.OrderBy(layer => layer.Altitude)
                                     .Where(layer => layer.IsCeiling);

            var hasCeiling = ceilings.Any();

            if (this.Visibility < 3 || (hasCeiling && ceilings.First().Altitude < 1000))
            {
                result = FlightRules.IFR;
            }
            else
            {
                if (hasCeiling && ceilings.First().Altitude <= 3000)
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
