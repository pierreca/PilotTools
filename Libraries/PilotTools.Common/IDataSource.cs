using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PilotTools.Common
{
    public interface IDataSource
    {
        string Name { get; }

        DataSourceOrigin Type { get; }

        Task LoadAsync();

        Task DownloadAsync(); 
    }
}
