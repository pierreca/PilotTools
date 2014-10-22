using System;
using System.Collections.Generic;
using System.Text;

using AirportData;
using PilotTools.DataSources;
using System.Collections.ObjectModel;
using System.Linq;
using PilotTools.Common;
using FlightPlanning;

namespace PilotTools.ViewModels
{
    public class EditFlightPlanViewModel : ViewModelBase
    {
        private FlightPlan flightPlan;
        private string newWayPointCode;

        public EditFlightPlanViewModel(IDataSourceManager sourceManager)
            : base(sourceManager)
        {
            this.FlightPlan = new FlightPlan();

            this.AddWaypoint = new RelayCommand(arg =>
            {
                if(string.IsNullOrWhiteSpace(this.newWayPointCode))
                {
                    return;
                }

                var airportDirectory = sourceManager.DataSources[DataSourceContentType.Airports] as IAirportDirectory;
                var airport = airportDirectory.GetAirportData(this.newWayPointCode);
                var waypoints = this.FlightPlan.WayPoints.ToList();
                waypoints.Add(airport);
                this.FlightPlan.WayPoints = waypoints;
            });

            this.Clear = new RelayCommand(arg => this.FlightPlan.WayPoints = new ObservableCollection<IAirport>());

            this.Save = new RelayCommand(async arg =>
            {
                var fpDataSource = this.SourceManager.DataSources[DataSourceContentType.FlightPlans] as FlightPlanSource;
                await fpDataSource.LoadAsync();
                var flightPlans = fpDataSource.FlightPlans.ToList();
                flightPlans.Add(this.FlightPlan);
                fpDataSource.FlightPlans = flightPlans;
                await fpDataSource.SaveAsync();
            });
        }

        public FlightPlan FlightPlan
        {
            get { return this.flightPlan; }
            set { this.SetProperty<FlightPlan>(ref this.flightPlan, value); }
        }

        public string NewWayPointCode
        {
            get { return this.newWayPointCode; }
            set { this.SetProperty<string>(ref this.newWayPointCode, value); }
        }

        public RelayCommand AddWaypoint { get; set; }

        public RelayCommand Clear { get; set; }

        public RelayCommand Save { get; set; }
        

    }
}
