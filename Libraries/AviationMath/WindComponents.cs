using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AviationMath
{
    public enum CrosswindDirection
    {
        FromLeft,
        FromRight
    }

    public class CrossWindComponents
    {
        private CrossWindComponents() { }

        public int Headwind { get; set; }
        public int Tailwind { get; set; }
        public int Crosswind { get; set; }

        public CrosswindDirection CrossWindDirection { get; set; }

        public static CrossWindComponents CreateFromMetarData(int WindFrom, int WindSpeed, int RunwayNumber, double magneticDeviation)
        {
            var result = new CrossWindComponents();

            var trueRwyHdg = RunwayNumber * 10 + magneticDeviation;
            var windRelativeAngle = WindFrom - trueRwyHdg;

            if (windRelativeAngle < 0)
            {
                windRelativeAngle += 360;
            }

            if(windRelativeAngle < 180)
            {
                result.CrossWindDirection = CrosswindDirection.FromLeft;
            }
            else
            {
                result.CrossWindDirection = CrosswindDirection.FromRight;
            }

            if(windRelativeAngle < 90 || windRelativeAngle > 270)
            {
                result.Headwind = 0;
                result.Tailwind = (int)Math.Abs(Math.Round(Math.Cos(windRelativeAngle * Math.PI / 180.0) * WindSpeed));
            }
            else
            {
                result.Headwind = (int)Math.Abs(Math.Round(Math.Cos(windRelativeAngle * Math.PI / 180.0) * WindSpeed));
                result.Tailwind = 0;
            }

            result.Crosswind = (int)Math.Abs(Math.Round(Math.Sin(windRelativeAngle * Math.PI / 180.0) * WindSpeed));

            return result;
        }
    }
}
