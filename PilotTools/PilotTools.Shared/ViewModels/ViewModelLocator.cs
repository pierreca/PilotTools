using PilotTools.DataSources;
using System;
using System.Collections.Generic;
using System.Text;

namespace PilotTools.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            this.AirportsPivotViewModel = new AirportsPivotViewModel(App.DataSourceManager);
            this.EditFlightPlanViewModel = new EditFlightPlanViewModel(App.DataSourceManager);
            this.SelectedAirportViewModel = new AirportViewModel(App.DataSourceManager);
            this.SearchViewModel = new SearchViewModel(App.DataSourceManager);
            this.SettingsViewModel = new SettingsViewModel(App.DataSourceManager);
        }

        public AirportsPivotViewModel AirportsPivotViewModel { get; set; }
        public EditFlightPlanViewModel EditFlightPlanViewModel { get; set; }
        public AirportViewModel SelectedAirportViewModel { get; set; }
        public SearchViewModel SearchViewModel { get; set; }
        public SettingsViewModel SettingsViewModel { get; set; }
    }
}
