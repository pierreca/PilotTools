using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace AirportData
{
    public interface IAirport
    {
        int Id { get; set; }

        Dictionary<RadioFrequencyType, double> Frequencies { get; set; }

        string ICAO { get; set; }
        string LocalCode { get; set; }

        string Name { get; set; }

        BasicGeoposition Position { get; set; }

        IEnumerable<IRunway> Runways { get; set; }

    }
}