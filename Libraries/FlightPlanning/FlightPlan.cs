using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AirportData;

namespace FlightPlanning
{
    public class FlightPlan
    {
        private const char Separator = ',';
        public FlightPlan()
        {
            this.Waypoints = new List<IAirport>();
        }

        public IEnumerable<IAirport> Waypoints { get; set; }

        public void AddWaypoint(IAirport airport)
        {
            var wps = this.Waypoints.ToList();
            wps.Add(airport);
            this.Waypoints = wps;
        }

        public static FlightPlan FromString(string s, IAirportDirectory directory)
        {
            var retval = new FlightPlan();

            if(string.IsNullOrWhiteSpace(s))
            {
                throw new ArgumentException("s");
            }

            var wpTable = s.Split(Separator);
            retval.Waypoints = wpTable.Select(wp => directory.GetAirportData(wp));

            return retval;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var wp in Waypoints)
            {
                sb.Append(wp.ICAO + Separator);
            }

            return sb.ToString().TrimEnd(Separator);
        }
    }
}
