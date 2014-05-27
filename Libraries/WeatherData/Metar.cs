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
    }
}
