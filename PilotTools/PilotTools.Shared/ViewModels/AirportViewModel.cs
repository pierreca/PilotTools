using AirportData;
using PilotTools.DataSources;
using PilotTools.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AviationMath;
using WeatherData;
using Windows.Devices.Geolocation;
using PilotTools.Common;

namespace PilotTools.ViewModels
{
    public class AirportViewModel : ViewModelBase
    {
        private IAirport airport;
        private bool hasMetar;
        private Geopoint mapCenter;
        private Metar metar;
        private PersonalMinimumsResult meetsPersonalMinimums;

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

        public Geopoint MapCenter
        {
            get { return this.mapCenter; }
            set { this.SetProperty<Geopoint>(ref this.mapCenter, value); }
        }

        public PersonalMinimumsResult MeetsPersonalMinimums
        {
            get { return this.meetsPersonalMinimums; }
            set { this.SetProperty<PersonalMinimumsResult>(ref this.meetsPersonalMinimums, value); }
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
                var airportDirectory = this.SourceManager.DataSources[DataSourceContentType.Airports] as IAirportDirectory;
                this.Airport = airportDirectory.GetAirportData(airportCode);
                this.MapCenter = new Geopoint(this.Airport.Position);
                
                if (SystemHelper.HasNetwork)
                {
                    var decoder = new WeatherData.MetarDecoder();
                    this.Metar = await decoder.GetMetarAsync(airportCode);
                    this.HasMetar = true;
                }

                this.MeetsPersonalMinimums = await this.CheckPersonalMinimums();
            }
            catch(Exception ex)
            {
                this.HasMetar = false;
            }
        }

        public async Task<PersonalMinimumsResult> CheckPersonalMinimums()
        {
            var minimums = await PersonalMinimums.LoadAsync();
            var OK = false;

            foreach (var rwy in this.airport.Runways)
            {
                if (!rwy.Closed && rwy.Length > minimums.RunwayLength && rwy.Width > minimums.RunwayWidth)
                {
                    OK = true;
                    break;
                }
            }

            if (OK)
            {
                OK = false;
            }
            else
            {
                return PersonalMinimumsResult.Fail;
            }

            if (this.HasMetar)
            {
                // TODO: fix magnetic deviation and speed vs gust speeds, variability, and multiple runways. 
                var windComponents = CrossWindComponents.CreateFromMetarData(this.Metar.Wind.Direction, this.Metar.Wind.Speed, this.airport.Runways.First().Id, 0.0);
                
            }

            return PersonalMinimumsResult.Unknown;
        }
    }
}
