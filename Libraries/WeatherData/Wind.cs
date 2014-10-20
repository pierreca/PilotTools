using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherData
{
    public class Wind
    {
        public int Direction { get; set; }
        public int Speed { get; set; }
        public int GustSpeed { get; set; }

        public bool IsVariable { get; set; }
        public int VrbDirectionLow { get; set; }
        public int VrbDirectionHigh { get; set; }

        public override string ToString()
        {
            var result = string.Empty;

            if(this.Speed == 0)
            {
                result = "Calm";
            }
            else
            {
                var sb = new StringBuilder();
                if(IsVariable)
                {
                    sb.Append(string.Format("Variable @ {0} kts", this.Speed));
                }
                else
                {
                    sb.Append(string.Format("From {0} @ {1} kts", this.Direction, this.Speed));
                    if(this.GustSpeed > 0)
                    {
                        sb.Append(string.Format(", gusts at {0}", this.GustSpeed));
                    }
                }

                result = sb.ToString();
            }

            return result;
        }
    }
}
