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
        private FlightPlanViewModel flightPlan;
        private string newWayPointCode;

        public EditFlightPlanViewModel(IDataSourceManager sourceManager)
            : base(sourceManager)
        {
            this.FlightPlan = flightPlan;

            this.AddWaypoint = new RelayCommand(arg =>
            {
                if(string.IsNullOrWhiteSpace(this.newWayPointCode))
                {
                    return;
                }

                this.FlightPlan.AddWaypoint(this.newWayPointCode);
            });

            this.Clear = new RelayCommand(arg => this.FlightPlan.Waypoints.Clear());

            this.Save = new RelayCommand(async arg =>
            {
                var fpDataSource = this.SourceManager.DataSources[DataSourceContentType.FlightPlans] as FlightPlanSource;
                await fpDataSource.LoadAsync();
                var flightPlans = fpDataSource.FlightPlans.ToList();
                flightPlans.Add(this.FlightPlan.FlightPlan);
                fpDataSource.FlightPlans = flightPlans;
                await fpDataSource.SaveAsync();
            });
        }

        public FlightPlanViewModel FlightPlan
        {
            get { return this.flightPlan; }
            set { this.SetProperty<FlightPlanViewModel>(ref this.flightPlan, value); }
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
