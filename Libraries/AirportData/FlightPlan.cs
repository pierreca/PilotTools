using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportData
{
    public class FlightPlan
    {
        private const char Separator = ',';
        public FlightPlan()
        {
            this.WayPoints = new List<IAirport>();
        }

        public IEnumerable<IAirport> WayPoints { get; set; }

        public static FlightPlan FromString(string s, IAirportDirectory directory)
        {
            var retval = new FlightPlan();

            if(string.IsNullOrWhiteSpace(s))
            {
                throw new ArgumentException("s");
            }

            var wpTable = s.Split(Separator);
            retval.WayPoints = wpTable.Select(wp => directory.GetAirportData(wp));

            return retval;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var wp in WayPoints)
            {
                sb.Append(wp.ICAO + Separator);
            }

            return sb.ToString().TrimEnd(Separator);
        }
    }
}
