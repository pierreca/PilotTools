using System;
using System.Collections.Generic;
using System.Text;

using AirportData;
using PilotTools.DataSources;
using System.Collections.ObjectModel;
using System.Linq;
using PilotTools.Common;
using FlightPlanning;
using System.Threading.Tasks;

namespace PilotTools.ViewModels
{
    public class EditFlightPlanViewModel : ViewModelBase
    {
        private FlightPlanViewModel flightPlan;
        private string newWayPointCode;
        private bool waypointNotFound = false;

        public EditFlightPlanViewModel(IDataSourceManager sourceManager)
            : base(sourceManager)
        {
            this.FlightPlan = flightPlan;

            this.AddWaypoint = new RelayCommand(async arg =>
            {
                if(string.IsNullOrWhiteSpace(this.newWayPointCode))
                {
                    this.WaypointNotFound = true;
                    return;
                }

                if(this.FlightPlan.FlightPlan == null)
                {
                    this.FlightPlan.FlightPlan = new FlightPlan();
                }

                try
                {
                    await this.FlightPlan.AddWaypoint(this.newWayPointCode);
                    this.WaypointNotFound = false;
                }
                catch (AirportData.AirportDirectoryException)
                {
                    this.WaypointNotFound = true;
                }
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

        public bool WaypointNotFound
        {
            get { return this.waypointNotFound; }
            set { this.SetProperty<bool>(ref this.waypointNotFound, value); }
        }

        public RelayCommand AddWaypoint { get; set; }
        
        public void DeleteWaypoints(IList<object> list)
        {
            // we need an intermediate list because as soon as we remove from list, the COMObject cannot be reevaluated.
            var toRemove = (from item in list
                            let avm = item as AirportViewModel
                            select avm).ToList();

            foreach (var airport in toRemove)
            {
                this.FlightPlan.Waypoints.Remove(airport as AirportViewModel);
            }
        }

        public async Task SaveWaypoints()
        {
            var locator = App.Current.Resources["ViewModelLocator"] as ViewModelLocator;
            await locator.AirportsPivotViewModel.SaveFlightPlans();
        }
    }
}
