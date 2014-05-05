using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportData
{
    public class AirportDirectoryException : Exception
    {
        string Message { get; set; }

        public AirportDirectoryException(string message)
        {
            this.Message = message;   
        }
    }
}
