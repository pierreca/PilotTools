using System;
using System.Collections.Generic;
using System.Text;

using FlightPlanning;
using PilotTools.Common;

namespace PilotTools.DataSources
{
    public class DataSourceManager : IDataSourceManager
    {
        public Dictionary<DataSourceContentType, IDataSource> DataSources { get; set; }

        public DataSourceManager()
        {
            var sources = new Dictionary<DataSourceContentType, IDataSource>();
            var airports = new AirportData.OurAirports.AirportDirectory();
            sources.Add(DataSourceContentType.Airports, airports);

            var flightPlans = new FlightPlanSource(airports);
            sources.Add(DataSourceContentType.FlightPlans, flightPlans);

            this.DataSources = sources; 
        }
    }
}
