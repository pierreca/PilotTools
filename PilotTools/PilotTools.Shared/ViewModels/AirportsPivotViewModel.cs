using AirportData;
using FlightPlanning;
using PilotTools.Common;
using PilotTools.DataSources;
using PilotTools.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace PilotTools.ViewModels
{
    public class AirportsPivotViewModel : ViewModelBase
    {
        private int aroundMeRadius;

        private ObservableCollection<AirportViewModel> favorites;
        private ObservableCollection<AirportViewModel> nearest;
        private ObservableCollection<FlightPlanViewModel> flightPlans;
        private bool hasNetwork;
        private string searchCode;

        public AirportsPivotViewModel(IDataSourceManager sourceManager)
            : base (sourceManager)
        {
            this.aroundMeRadius = 20;

            this.Load = new RelayCommand(arg =>
            {
                this.LoadFavorites();
                this.LoadAroundMe();
                this.LoadFlightPlans();
            });

            this.RefreshFavorites = new RelayCommand(async arg =>
            {
                foreach (var avm in this.favorites)
                {
                    await avm.LoadAirportWeatherAsync(true);
                }
            });
        }

        public ObservableCollection<AirportViewModel> Nearest
        {
            get { return this.nearest; }
            set { this.SetProperty<ObservableCollection<AirportViewModel>>(ref this.nearest, value); }
        }

        public int AroundMeRadius
        {
            get { return this.aroundMeRadius; }
            set { this.SetProperty<int>(ref this.aroundMeRadius, value); }
        }

        public ObservableCollection<AirportViewModel> Favorites
        {
            get { return this.favorites; }
            set { this.SetProperty<ObservableCollection<AirportViewModel>>(ref this.favorites, value); }
        }

        public ObservableCollection<FlightPlanViewModel> FlightPlans
        {
            get { return this.flightPlans; }
            set { this.SetProperty<ObservableCollection<FlightPlanViewModel>>(ref this.flightPlans, value); }
        }

        public bool HasNetwork
        {
            get { return this.hasNetwork; }
            set { this.SetProperty<bool>(ref this.hasNetwork, value); }
        }

        public string SearchCode
        {
            get { return this.searchCode; }
            set { this.SetProperty<string>(ref this.searchCode, value); }
        }

        public RelayCommand Load { get; set; }

        public RelayCommand RefreshFavorites { get; set; }

        private async void LoadFavorites()
        {
            var favs = await Helpers.Favorites.LoadAsync();

            try
            {
                var airportsDB = this.SourceManager.DataSources[DataSourceContentType.Airports] as IAirportDirectory; 
                
                if (favs.Count() > 0)
                {
                    this.Favorites = new ObservableCollection<AirportViewModel>();
                    var airports = favs.Select(f => airportsDB.GetAirportData(f));
                    foreach (var airport in airports)
                    {
                        var vm = new AirportViewModel(this.SourceManager);
                        this.HasNetwork = SystemHelper.HasNetwork;
                        await vm.LoadAirportDataAsync(airport.ICAO);
                        this.Favorites.Add(vm);
                    }
                }
            } 
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async void LoadAroundMe()
        {
            var airportsDB = this.SourceManager.DataSources[DataSourceContentType.Airports] as IAirportDirectory;
            var myPosition = await Helpers.Location.GetPosition();
            var airportsNearMe = await airportsDB.GetAirportsAroundAsync(myPosition, this.AroundMeRadius);

            this.Nearest = new ObservableCollection<AirportViewModel>();
            foreach (var airport in airportsNearMe)
            {
                var vm = new AirportViewModel(this.SourceManager);
                this.HasNetwork = SystemHelper.HasNetwork;
                await vm.LoadAirportDataAsync(airport.ICAO);
                this.Nearest.Add(vm);
            }
        }

        private async void LoadFlightPlans()
        {
            var fpSource = this.SourceManager.DataSources[DataSourceContentType.FlightPlans] as FlightPlanSource;
            await fpSource.LoadAsync();
            this.FlightPlans = new ObservableCollection<FlightPlanViewModel>();
            
            foreach (var fp in fpSource.FlightPlans)
            {
                this.FlightPlans.Add(await FlightPlanViewModel.CreateAsync(fp));
            }
        }

        public async Task DeleteFavorites(IList<object> list)
        {
            // we need an intermediate list because as soon as we remove from list, the COMObject cannot be reevaluated.
            var toRemove = (from item in list
                            let avm = item as AirportViewModel
                            select avm).ToList();

            foreach (var airport in toRemove)
            {
                this.Favorites.Remove(airport as AirportViewModel);
            }

            var newFavs = this.Favorites.Select<AirportViewModel, string>(avm => avm.Airport.ICAO);
            await Helpers.Favorites.SaveAsync(newFavs);
        }

        public async Task DeleteFlightPlans(IList<object> list)
        {
            // we need an intermediate list because as soon as we remove from list, the COMObject cannot be reevaluated.
            var toRemove = (from item in list
                            let fpvm = item as FlightPlanViewModel
                            select fpvm).ToList();

            foreach (var flightplan in toRemove)
            {
                this.FlightPlans.Remove(flightplan as FlightPlanViewModel);
            }

            var fpSource = this.SourceManager.DataSources[DataSourceContentType.FlightPlans] as FlightPlanSource;
            fpSource.FlightPlans = from fpvm in this.FlightPlans
                                   select fpvm.FlightPlan;

            await fpSource.SaveAsync();
        }

        public async Task SaveFlightPlans()
        {
            var flightPlans = this.FlightPlans.Select(fp => fp.FlightPlan);
            var fpSource = this.SourceManager.DataSources[DataSourceContentType.FlightPlans] as FlightPlanSource;
            fpSource.FlightPlans = flightPlans;
            await fpSource.SaveAsync();
        }
    }
}
