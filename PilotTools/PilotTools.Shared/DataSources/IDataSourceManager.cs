using System;
using System.Collections.Generic;
using System.Text;

namespace PilotTools.DataSources
{
    public interface IDataSourceManager
    {
        Dictionary<DataSourceContent, IDataSource> DataSources { get; set; }

    }
}
