using AirportData;
using PilotTools.DataSources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace PilotTools.ViewModels
{
    public class AirportViewModel : ViewModelBase
    {
        private IAirport airport;
        private Geopoint mapCenter;
        private string metar;

        public AirportViewModel(IDataSourceManager sourceManager)
            : base(sourceManager)
        {
            this.AddFavorite = new RelayCommand(async arg =>
            {
                var favs = (await Helpers.Favorites.LoadAsync()).ToList();
                if(!favs.Contains(this.airport.ICAO))
                {
                    favs.Add(this.airport.ICAO);
                    await Helpers.Favorites.SaveAsync(favs);
                }
            });
        }

        public IAirport Airport
        {
            get { return this.airport; }
            set { this.SetProperty<IAirport>(ref this.airport, value); }
        }

        public Geopoint MapCenter
        {
            get { return this.mapCenter; }
            set { this.SetProperty<Geopoint>(ref this.mapCenter, value); }
        }

        public string Metar
        {
            get { return this.metar; }
            set { this.SetProperty<string>(ref this.metar, value); }
        }

        public RelayCommand AddFavorite { get; set; }

        public async Task LoadAirportDataAsync(string airportCode)
        {
            try
            {
                var airportSource = this.SourceManager.DataSources[DataSourceContent.Airports] as AirportSource;
                this.Airport = airportSource.Directory.GetAirportData(airportCode);
                this.MapCenter = new Geopoint(this.Airport.Position);

                var decoder = new WeatherData.MetarDecoder();
                var weatherData = await decoder.GetMetarAsync(airportCode);
                this.Metar = weatherData.Raw;
            }
            catch(Exception ex)
            {
                // TODO: Surface this error to the UI differently
                Debug.WriteLine("Failed to query airport data source: " + ex.Message);
            }
        }
    }
}
