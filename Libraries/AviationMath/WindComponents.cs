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

    public class WindComponents
    {
        public int Headwind { get; set; }
        public int Tailwind { get; set; }
        public int Crosswind { get; set; }

        public CrosswindDirection CrossWindDirection { get; set; }
    }
}
