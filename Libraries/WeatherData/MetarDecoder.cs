using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherData
{
    public class MetarDecoder : IMetarDecoder
    {
        public async Task<Metar> GetMetarAsync(string icao)
        {
            throw new NotImplementedException();
        }
    }
}
