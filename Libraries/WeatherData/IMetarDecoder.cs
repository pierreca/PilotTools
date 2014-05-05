using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherData
{
    public interface IMetarDecoder
    {
        Task<Metar> GetMetarAsync(string icao);
    }
}
