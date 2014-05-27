using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace AviationMath
{
    public static class BasicGeopositionExtensions
    {
        /* Haversine formula code from http://damien.dennehy.me/blog/2011/01/15/haversine-algorithm-in-csharp/ */

        /// <summary>
        /// Radius of the Earth in Kilometers.
        /// </summary>
        private static double EARTH_RADIUS_KM = 6371;

        /// <summary>
        /// Converts an angle to a radian.
        /// </summary>
        /// <param name="input">The angle that is to be converted.</param>
        /// <returns>The angle in radians.</returns>
        private static double ToRad(double input)
        {
            return input * (Math.PI / 180);
        }

        /// <summary>
        /// Calculates the distance between two geo-points in kilometers using the Haversine algorithm.
        /// </summary>
        /// <param name="point1">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>A double indicating the distance between the points in KM.</returns>
        public static double GetDistanceToKM(this BasicGeoposition origin, BasicGeoposition position)
        {
            double dLat = ToRad(position.Latitude - origin.Latitude);
            double dLon = ToRad(position.Longitude - origin.Longitude);

            double a = Math.Pow(Math.Sin(dLat / 2), 2) +
                       Math.Cos(ToRad(origin.Latitude)) * Math.Cos(ToRad(position.Latitude)) *
                       Math.Pow(Math.Sin(dLon / 2), 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double distance = EARTH_RADIUS_KM * c;
            return distance;
        }

        public static double GetDistanceToNM(this BasicGeoposition origin, BasicGeoposition position)
        {
            if (origin.IsValid() && position.IsValid())
            {
                return UnitConverter.KilometersToNauticalMiles(origin.GetDistanceToKM(position));
            }
            else
            {
                return double.NaN;
            }
        }


        public static bool IsValid(this BasicGeoposition position)
        {
            return position.Latitude != 0.0 && position.Longitude != 0.0;
        }
    }
}
