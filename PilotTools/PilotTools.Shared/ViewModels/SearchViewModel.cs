using AirportData;
using PilotTools.DataSources;
using PilotTools.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotTools.ViewModels
{
    public class SearchViewModel : ViewModelBase
    {
        private string searchQuery;
        private ObservableCollection<AirportViewModel> selectedResults;
        private bool showError;
        private ObservableCollection<AirportViewModel> results;
        public bool hasResultsSelected;

        public SearchViewModel(IDataSourceManager sourceManager)
            : base(sourceManager)
        {
            this.ShowError = false;
            this.Search = new RelayCommand(async arg => await this.StartSearch());
            this.AddFavorites = new RelayCommand(async arg => await this.AddSelectedToFavorites());
            this.HasResultsSelected = false;
        }

        public string SearchQuery
        {
            get { return this.searchQuery; }
            set { this.SetProperty<string>(ref this.searchQuery, value); }
        }

        public ObservableCollection<AirportViewModel> SelectedResults
        {
            get { return this.selectedResults; }
            set { this.SetProperty<ObservableCollection<AirportViewModel>>(ref this.selectedResults, value); }
        }

        public bool ShowError
        {
            get { return this.showError; }
            set { this.SetProperty<bool>(ref this.showError, value); }
        }

        public ObservableCollection<AirportViewModel> Results
        {
            get { return this.results; }
            set { this.SetProperty<ObservableCollection<AirportViewModel>>(ref this.results, value); }
        }

        public bool HasResultsSelected
        {
            get { return this.hasResultsSelected; }
            set { this.SetProperty<bool>(ref this.hasResultsSelected, value); }
        }

        public RelayCommand AddFavorites { get; set; }
        public RelayCommand Search { get; set; }

        private async Task AddSelectedToFavorites()
        {
            if(this.SelectedResults != null && this.SelectedResults.Count > 0)
            {
                var favs = (await Helpers.Favorites.LoadAsync()).ToList();

                foreach(var avm in this.SelectedResults)
                {
                    if(!favs.Contains(avm.Airport.ICAO))
                    {
                        favs.Add(avm.Airport.ICAO);
                    }
                }

                await Helpers.Favorites.SaveAsync(favs);
            }
        }

        private async Task StartSearch()
        {
            // Display error message instead?
            if (string.IsNullOrWhiteSpace(this.SearchQuery))
            {
                this.ShowError = true;
            }
            else
            {
                var codes = this.SearchQuery.Split(' ');
                this.Results = new ObservableCollection<AirportViewModel>();

                foreach (var airportCode in codes)
                {
                    var r = new AirportViewModel(this.SourceManager);
                    await r.LoadAirportDataAsync(airportCode);
                    this.Results.Add(r);
                }
            }
        }
    }
}
