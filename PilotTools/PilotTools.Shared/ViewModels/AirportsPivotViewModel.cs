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
using Windows.Devices.Geolocation;

namespace PilotTools.ViewModels
{
    public class AirportsPivotViewModel : ViewModelBase
    {
        private int aroundMeRadius;

        private ObservableCollection<AirportViewModel> favorites;
        private ObservableCollection<IAirport> aroundMe;

        private ObservableCollection<FlightPlan> flightPlans;

        private bool hasNetwork;
        private string searchCode;

        public AirportsPivotViewModel(IDataSourceManager sourceManager)
            : base (sourceManager)
        {
            this.aroundMeRadius = 50;

            this.Load = new RelayCommand(arg =>
            {
                this.LoadFavorites();
                this.LoadAroundMe();
                this.LoadFlightPlans();
            });
        }

        public ObservableCollection<IAirport> AroundMe
        {
            get { return this.aroundMe; }
            set { this.SetProperty<ObservableCollection<IAirport>>(ref this.aroundMe, value); }
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

        public ObservableCollection<FlightPlan> FlightPlans
        {
            get { return this.flightPlans; }
            set { this.SetProperty<ObservableCollection<FlightPlan>>(ref this.flightPlans, value); }
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

            this.AroundMe = new ObservableCollection<IAirport>(await airportsDB.GetAirportsAroundAsync(myPosition, this.AroundMeRadius));
        }

        private async void LoadFlightPlans()
        {
            var fpSource = this.SourceManager.DataSources[DataSourceContentType.FlightPlans] as FlightPlanSource;
            await fpSource.LoadAsync();
            this.FlightPlans = new ObservableCollection<FlightPlan>(fpSource.FlightPlans);
        }
    }
}
