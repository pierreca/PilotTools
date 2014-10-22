using System;
using System.Collections.Generic;
using System.Text;

namespace PilotTools.Common
{
    public class DataLoadedEventArgs : EventArgs
    {
        public string SourceName { get; set; }
    }
}
