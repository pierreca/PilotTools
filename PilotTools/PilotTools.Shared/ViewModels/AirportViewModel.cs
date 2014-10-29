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
        private Dictionary<string, PersonalMinimumVerificationResult> personalMinimumResults;

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

        public Metar Metar
        {
            get { return this.metar; }
            set { this.SetProperty<Metar>(ref this.metar, value); }
        }

        public Dictionary<string, PersonalMinimumVerificationResult> PersonalMinimumResults
        {
            get { return this.personalMinimumResults; }
            set { this.SetProperty<Dictionary<string, PersonalMinimumVerificationResult>>(ref this.personalMinimumResults, value); }
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

                this.PersonalMinimumResults = await this.CheckPersonalMinimums();
            }
            catch(Exception)
            {
                this.HasMetar = false;
            }
        }

        public async Task<Dictionary<string, PersonalMinimumVerificationResult>> CheckPersonalMinimums()
        {
            Dictionary<string, PersonalMinimumVerificationResult> results = new Dictionary<string, PersonalMinimumVerificationResult>();
            results.Add("Visibility", PersonalMinimumVerificationResult.Unknown);
            results.Add("Ceiling", PersonalMinimumVerificationResult.Unknown);
            results.Add("Runway", PersonalMinimumVerificationResult.Unknown);
            results.Add("Crosswind", PersonalMinimumVerificationResult.Unknown);


            var minimums = await PersonalMinimums.LoadAsync();

            results["Visibility"] = this.Metar.Visibility.IsOver(minimums.Visibility);

            if(this.Metar.Clouds.Any(cl => cl.IsCeiling))
            {
                var ceiling = this.Metar.Clouds.Where(cl => cl.IsCeiling)
                                               .OrderBy(cl => cl.Altitude)
                                               .First();

                results["Ceiling"] = ceiling.Altitude.IsOver(minimums.Ceiling);
            }
            else
            {
                results["Ceiling"] = PersonalMinimumVerificationResult.Pass;
            }

            var passRunway = this.Airport.Runways.Where(rwy => rwy.Length.IsOver(minimums.RunwayLength) == PersonalMinimumVerificationResult.Pass
                                                               && rwy.Width.IsOver(minimums.RunwayWidth) == PersonalMinimumVerificationResult.Pass
                                                               && !rwy.Closed);
            var warningRunway = this.Airport.Runways.Where(rwy => rwy.Length.IsOver(minimums.RunwayLength) != PersonalMinimumVerificationResult.Fail
                                                                  && rwy.Width.IsOver(minimums.RunwayWidth) != PersonalMinimumVerificationResult.Fail
                                                                  && !rwy.Closed);
            if(passRunway.Any())
            {
                results["Runway"] = PersonalMinimumVerificationResult.Pass;
            }
            else if(warningRunway.Any())
            {
                results["Runway"] = PersonalMinimumVerificationResult.Warning;
            }
            else
            {
                results["Runway"] = PersonalMinimumVerificationResult.Fail;
            }

            if (this.HasMetar)
            {
                foreach (var rwy in passRunway)
                {
                    var windComponents = CrossWindComponents.CreateFromMetarData(this.Metar.Wind.Direction, this.Metar.Wind.Speed, rwy.End1.Heading);
                    results["Crosswind"] = minimums.Crosswind.IsOver(windComponents.Crosswind);

                    if (results["Crosswind"] == PersonalMinimumVerificationResult.Pass)
                    {
                        break;
                    }
                }
            }

            return results;
        }
    }
}
