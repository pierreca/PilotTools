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
        Task DownloadAndSaveAsync();
        IAirport GetAirportData(string ICAO);
        Task<IEnumerable<IAirport>> GetAirportsAroundAsync(BasicGeoposition position, int searchRadius);
        Task LoadAsync();

    }
}
