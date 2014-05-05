using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportData
{
    public interface IAirport
    {
        int Id { get; set; }
        int Elevation { get; set; }
        Dictionary<RadioFrequencyType, double> Frequencies { get; set; }

        string ICAO { get; set; }
        string LocalCode { get; set; }

        string Name { get; set; }

        double Latitude { get; set; }
        double Longitude { get; set; }

        IEnumerable<IRunway> Runways { get; set; }

    }
}