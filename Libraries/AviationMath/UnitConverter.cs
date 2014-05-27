using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AviationMath
{
    public static class UnitConverter
    {
        public static double FeetToMeters(double feet)
        {
            return feet * 0.3048;
        }

        public static double MetersToFeet(double meters)
        {
            return meters * 3.28084;
        }

        public static double KilometersToNauticalMiles(double km)
        {
            return km * 0.539957;
        }
    }
}
