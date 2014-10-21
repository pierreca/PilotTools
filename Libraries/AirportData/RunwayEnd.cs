using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace AirportData
{
    public class RunwayEnd
    {
        public string Identifier { get; set; }

        public BasicGeoposition Position { get; set; }

        public int DisplacedThreshold { get; set; }
    }
}
