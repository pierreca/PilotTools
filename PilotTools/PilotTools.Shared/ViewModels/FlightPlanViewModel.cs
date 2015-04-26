using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FlightPlanning;
using PilotTools.DataSources;

namespace PilotTools.ViewModels
{
    public class FlightPlanViewModel : ViewModelBase
    {
        private FlightPlan flightPlan;
        private string name;
        private ObservableCollection<AirportViewModel> waypoints;

        public FlightPlanViewModel(IDataSourceManager sourceManager)
            : base(sourceManager)
        { }

        public string Name
        {
            get { return this.name; }
            set { this.SetProperty<string>(ref this.name, value); }
        }

        public FlightPlan FlightPlan
        {
            get { return this.flightPlan; }
            set { this.SetProperty<FlightPlan>(ref this.flightPlan, value); }
        }

        public ObservableCollection<AirportViewModel> Waypoints
        {
            get { return this.waypoints; }
            set { this.SetProperty<ObservableCollection<AirportViewModel>>(ref this.waypoints, value); }
        }

        public async static Task<FlightPlanViewModel> CreateAsync(FlightPlan flightPlan)
        {
            var fpvm = new FlightPlanViewModel(App.DataSourceManager);
            fpvm.flightPlan = flightPlan;
            await fpvm.LoadWaypoints();
            
            return fpvm;
        }

        public async Task AddWaypoint(string ICAO)
        {
            var avm = new AirportViewModel(this.SourceManager);
            await avm.LoadAirportDataAsync(ICAO);
            this.FlightPlan.AddWaypoint(avm.Airport);
            await this.LoadWaypoints();
        }

        private async Task LoadWaypoints()
        {
            if (this.Waypoints == null)
            {
                this.Waypoints = new ObservableCollection<AirportViewModel>();
            }
            else
            {
                this.Waypoints.Clear();
            }

            foreach (var wp in flightPlan.Waypoints)
            {
                var avm = new AirportViewModel(App.DataSourceManager);
                await avm.LoadAirportDataAsync(wp.ICAO);
                this.Waypoints.Add(avm);
            }

            this.Name = this.Waypoints.First().Airport.ICAO + " -> " + this.Waypoints.Last().Airport.ICAO;
        }
    }
}
