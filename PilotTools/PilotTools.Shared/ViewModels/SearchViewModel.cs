using AirportData;
using PilotTools.DataSources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace PilotTools.ViewModels
{
    public class SearchViewModel : ViewModelBase
    {
        private string searchQuery;
        private AirportViewModel result;

        public SearchViewModel(IDataSourceManager sourceManager)
            : base(sourceManager)
        {
            this.Search = new RelayCommand(async arg => await this.StartSearch());
        }

        public string SearchQuery
        {
            get { return this.searchQuery; }
            set { this.SetProperty<string>(ref this.searchQuery, value); }
        }

        public AirportViewModel Result
        {
            get { return this.result; }
            set { this.SetProperty<AirportViewModel>(ref this.result, value); }
        }

        public RelayCommand Search { get; set; }

        private async Task StartSearch()
        {
            if (string.IsNullOrWhiteSpace(this.SearchQuery))
                return;

            var r = new AirportViewModel(this.SourceManager);
            await r.LoadAirportDataAsync(this.SearchQuery);

            this.Result = r;
        }
    }
}
