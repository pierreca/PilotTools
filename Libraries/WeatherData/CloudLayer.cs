using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherData
{
    public enum CloudLayerType
    {
        FEW,
        SCT,
        BKN,
        OVC
    }

    public class CloudLayer
    {
        public int Altitude { get; set; }
        public CloudLayerType Type { get; set; }
        public bool IsCeiling { get; set; }
    }
}
