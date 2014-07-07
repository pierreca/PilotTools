using AirportData;
using PilotTools.DataSources;
using PilotTools.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherData;
using Windows.Devices.Geolocation;

namespace PilotTools.ViewModels
{
    public class AirportViewModel : ViewModelBase
    {
        private IAirport airport;
        private bool hasNetwork;
        private bool hasMetar;
        private Geopoint mapCenter;
        private Metar metar;
        private bool meetsPersonalMinimums;

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

        public bool HasMetar
        {
            get { return this.hasMetar; }
            set { this.SetProperty<bool>(ref this.hasMetar, value); }
        }


        public bool HasNetwork
        {
            get { return this.hasNetwork; }
            set { this.SetProperty<bool>(ref this.hasNetwork, value); }
        }

        public Geopoint MapCenter
        {
            get { return this.mapCenter; }
            set { this.SetProperty<Geopoint>(ref this.mapCenter, value); }
        }

        public bool MeetsPersonalMinimums
        {
            get { return this.meetsPersonalMinimums; }
            set { this.SetProperty<bool>(ref this.meetsPersonalMinimums, value); }
        }

        public Metar Metar
        {
            get { return this.metar; }
            set { this.SetProperty<Metar>(ref this.metar, value); }
        }

        public RelayCommand AddFavorite { get; set; }

        public async Task LoadAirportDataAsync(string airportCode)
        {
            try
            {
                var airportSource = this.SourceManager.DataSources[DataSourceContent.Airports] as AirportSource;
                this.Airport = airportSource.Directory.GetAirportData(airportCode);
                this.MapCenter = new Geopoint(this.Airport.Position);
                
                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    this.HasNetwork = true;
                    var decoder = new WeatherData.MetarDecoder();
                    this.Metar = await decoder.GetMetarAsync(airportCode);
                    if(this.Metar.IsValid)
                    {
                        this.HasMetar = true;
                    }
                }

                this.MeetsPersonalMinimums = this.CheckPersonalMinimums();
            }
            catch(Exception ex)
            {
                // TODO: Surface this error to the UI differently
                Debug.WriteLine("Failed to query airport data source: " + ex.Message);
            }
        }

        public async Task<bool> CheckPersonalMinimums()
        {
            var minimums = await PersonalMinimums.LoadAsync();
            var OK = false;

            foreach(var rwy in this.airport.Runways)
            {
                if(!rwy.Closed && rwy.Length > minimums.RunwayLength && rwy.Width > minimums.RunwayWidth)
                {
                    OK = true;
                    break;
                }
            }

            if(OK)
            {
                OK = false;
            }
            else
            {
                return false;
            }

            if (this.HasMetar)
            {

            }
        }
    }
}
