using System;
using System.Collections.Generic;
using System.Text;

using AirportData;
using System.Threading.Tasks;

namespace PilotTools.DataSources
{
    public class AirportSource : IDataSource
    {
        private const string name = "Airports";

        public AirportData.OurAirports.AirportDirectory Directory { get; set; }

        public AirportSource()
        {
            this.Directory = new AirportData.OurAirports.AirportDirectory();
        }

        public string Name
        {
            get
            {
                return AirportSource.name;
            }
        }

        public DataSourceType Type
        {
            get { return DataSourceType.OnlineWithCache; }
        }

        public async Task LoadAsync()
        {
            await this.Directory.LoadAsync();
        }

        public async Task DownloadAsync()
        {
            await this.Directory.DownloadAndSaveAsync();
        }
    }
}
