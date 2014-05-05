using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportData
{
    public interface IRunway
    {
        RunwayEnd End1 { get; set; }
        RunwayEnd End2 { get; set; }

        int Id { get; set; }
        int AirportId { get; set; }
        int Length { get; set; }
        int Width { get; set; }

        bool Lighted { get; set; }
        bool Closed { get; set; }

        string Surface { get; set; }
    }
}
