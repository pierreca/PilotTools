using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PilotTools.DataSources
{
    public interface IDataSource
    {
        string Name { get; }

        DataSourceType Type { get; }

        Task LoadAsync();

        Task DownloadAsync(); 
    }
}
