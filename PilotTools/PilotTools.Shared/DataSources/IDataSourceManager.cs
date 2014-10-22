using PilotTools.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PilotTools.DataSources
{
    public interface IDataSourceManager
    {
        Dictionary<DataSourceContentType, IDataSource> DataSources { get; set; }

    }
}
