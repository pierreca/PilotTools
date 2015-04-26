using System;
using System.Collections.Generic;
using System.Text;

using FlightPlanning;
using PilotTools.Common;
using WeatherData;
using PilotTools.Helpers;

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

            sources.Add(DataSourceContentType.Metar, new MetarSource());

            sources.Add(DataSourceContentType.PersonalMinimums, new PersonalMinimums());

            this.DataSources = sources; 
        }
    }
}
