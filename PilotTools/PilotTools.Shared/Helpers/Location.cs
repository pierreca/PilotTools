using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace PilotTools.Helpers
{
    public static class Location
    {
        public static async Task<BasicGeoposition> GetPosition()
        {
            BasicGeoposition result = new BasicGeoposition();
            try
            {
                Geolocator locator = new Geolocator();
                var pos = await locator.GetGeopositionAsync();
                
                result = pos.Coordinate.Point.Position;
            }
            catch(Exception)
            {

            }

            return result;
        }
    }
}
