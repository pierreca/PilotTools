using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace AirportData
{
    public interface IAirportDirectory
    {
        IAirport GetAirportData(string ICAO);

        Task<IEnumerable<IAirport>> GetAirportsAroundAsync(BasicGeoposition position, int searchRadius);
    }
}
