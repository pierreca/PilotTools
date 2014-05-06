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
        public int Identifier;
        public int Heading;

        public BasicGeoposition Position;
        
        public int DisplacedThreshold;
    }
}
