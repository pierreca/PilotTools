using System;
using System.Collections.Generic;
using System.Text;

namespace PilotTools.Helpers
{
    public class SystemHelper
    {
        public static bool HasNetwork
        {
            get
            {
                return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            }
        }
    }
}
