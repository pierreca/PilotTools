using System;
using System.Collections.Generic;
using System.Text;

namespace PilotTools.DataSources
{
    public class DataSourceManager : IDataSourceManager
    {
        public Dictionary<DataSourceContent, IDataSource> DataSources { get; set; }

        public DataSourceManager()
        {
            var sources = new Dictionary<DataSourceContent, IDataSource>();
            var airports = new AirportSource();
            sources.Add(DataSourceContent.Airports, airports);

            var flightPlans = new FlightPlanSource(airports.Directory);
            sources.Add(DataSourceContent.FlightPlans, flightPlans);

            this.DataSources = sources; 
        }
    }
}
