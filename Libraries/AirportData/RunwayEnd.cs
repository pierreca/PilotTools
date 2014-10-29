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

        public int Heading
        {
            get
            {
                int heading;
                var gotHeading = int.TryParse(this.Identifier, out heading);

                if(!gotHeading)
                {
                    heading = int.Parse(this.Identifier.Substring(0, this.Identifier.Length - 1));
                }

                heading *= 10;
                return heading;
            }
        }
    }
}
