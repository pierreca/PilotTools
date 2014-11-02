using AirportData;
using PilotTools.Common;
using PilotTools.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace PilotTools.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private NavigationHelper navigationHelper;
        
        private CommandBar favoritesCommandBar;
        private CommandBar flightPlansCommandBar;

        private AppBarButton deleteFavoritesButton;
        private AppBarButton deleteFlightPlansButton;

        public MainPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            this.CreateCommandBars();
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            var vm = this.DataContext as AirportsPivotViewModel;
            vm.Load.Execute(null);
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        #region Favorites

        private async void BtnDeleteFavorite_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as AirportsPivotViewModel;
            await vm.DeleteFavorites(this.lvFavorites.SelectedItems);
        }

        private void BtnSelectFavorites_Checked(object sender, RoutedEventArgs e)
        {
            this.lvFavorites.SelectionMode = ListViewSelectionMode.Multiple;
            this.deleteFavoritesButton.Visibility = Visibility.Visible;
        }

        private void BtnSelectFavorites_Unchecked(object sender, RoutedEventArgs e)
        {
            this.lvFavorites.SelectionMode = ListViewSelectionMode.Single;
            this.deleteFavoritesButton.Visibility = Visibility.Collapsed;
        }

        private void BtnAddFavorite_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SearchAirport));
        }

        private void lvAirports_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ListView).SelectionMode == ListViewSelectionMode.Single
                && (sender as ListView).SelectedIndex != -1)
            {
                var vml = App.Current.Resources["ViewModelLocator"] as ViewModelLocator;
                vml.SelectedAirportViewModel = ((sender as ListView).SelectedItem as AirportViewModel);
                this.Frame.Navigate(typeof(Views.AirportDetails));
            }
        }

        #endregion Favorites

        #region FlightPlans


        private void BtnAddFlightPlan_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(EditFlightPlan));
        }

        private void lvFlightPlans_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ListView).SelectionMode == ListViewSelectionMode.Single
                && (sender as ListView).SelectedIndex != -1)
            {
                var vml = App.Current.Resources["ViewModelLocator"] as ViewModelLocator;
                vml.EditFlightPlanViewModel.FlightPlan = ((sender as ListView).SelectedItem as FlightPlanViewModel);
                this.Frame.Navigate(typeof(Views.EditFlightPlan));
            }
        }

        private async void BtnDeleteFlightPlan_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as AirportsPivotViewModel;
            await vm.DeleteFlightPlans(this.lvFlightPlans.SelectedItems);
        }

        private void BtnSelectFlightPlans_Checked(object sender, RoutedEventArgs e)
        {
            this.lvFlightPlans.SelectionMode = ListViewSelectionMode.Multiple;
            this.deleteFlightPlansButton.Visibility = Visibility.Visible;
        }

        private void BtnSelectFlightPlans_Unchecked(object sender, RoutedEventArgs e)
        {
            this.lvFlightPlans.SelectionMode = ListViewSelectionMode.Single;
            this.deleteFlightPlansButton.Visibility = Visibility.Collapsed;
        }

        #endregion FlightPlans


        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Settings));
        }

        private void LayoutRoot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((Pivot)sender).SelectedIndex)
            {
                case 0:
                    this.BottomAppBar = this.favoritesCommandBar;
                    break;
                case 1:
                    this.BottomAppBar = this.flightPlansCommandBar;
                    break;
            }
        }

        private void CreateCommandBars()
        {
            var locator = App.Current.Resources["ViewModelLocator"] as ViewModelLocator;
            var vm = locator.AirportsPivotViewModel;

            this.favoritesCommandBar = new CommandBar();

            var selectFavoriteButton = new AppBarToggleButton()
                {
                    Icon = new SymbolIcon(Symbol.List),
                    Label = "select",
                    Name = "BtnSelectFavorites"
                };
            selectFavoriteButton.Checked += this.BtnSelectFavorites_Checked;
            selectFavoriteButton.Unchecked += this.BtnSelectFavorites_Unchecked;

            var refreshFavoriteButton = new AppBarButton()
            {
                Icon = new SymbolIcon(Symbol.Refresh),
                Label = "refresh",
                Command = vm.RefreshFavorites
            };

            var addFavoriteButton = new AppBarButton()
            {
                Icon = new SymbolIcon(Symbol.Add),
                Label = "add"
            };
            addFavoriteButton.Click += this.BtnAddFavorite_Click;

            this.deleteFavoritesButton = new AppBarButton()
            {
                Icon = new SymbolIcon(Symbol.Delete),
                Label = "delete",
                Visibility = Visibility.Collapsed
            };
            this.deleteFavoritesButton.Click += this.BtnDeleteFavorite_Click;

            var settingsButton = new AppBarButton()
            {
                Label = "settings"
            };
            settingsButton.Click += this.BtnSettings_Click;

            this.favoritesCommandBar.PrimaryCommands.Add(selectFavoriteButton);
            this.favoritesCommandBar.PrimaryCommands.Add(refreshFavoriteButton);
            this.favoritesCommandBar.PrimaryCommands.Add(addFavoriteButton);
            this.favoritesCommandBar.PrimaryCommands.Add(this.deleteFavoritesButton);

            this.favoritesCommandBar.SecondaryCommands.Add(settingsButton);

            this.flightPlansCommandBar = new CommandBar();

            var selectFlightPlanButton = new AppBarToggleButton()
            {
                Icon = new SymbolIcon(Symbol.List),
                Label = "select",
                Name = "BtnSelectFlightPlan"
            };
            selectFlightPlanButton.Checked += this.BtnSelectFlightPlans_Checked;
            selectFlightPlanButton.Unchecked += this.BtnSelectFlightPlans_Unchecked;

            var refreshFlightPlanButton = new AppBarButton()
             {
                 Icon = new SymbolIcon(Symbol.Refresh),
                 Label = "refresh"
             };

            var addFlightPlanButton = new AppBarButton()
            {
                Icon = new SymbolIcon(Symbol.Add),
                Label = "add"
            };
            addFlightPlanButton.Click += this.BtnAddFlightPlan_Click;

            this.deleteFlightPlansButton = new AppBarButton()
            {
                Icon = new SymbolIcon(Symbol.Delete),
                Label = "delete",
                Visibility = Visibility.Collapsed
            };
            this.deleteFlightPlansButton.Click += this.BtnDeleteFlightPlan_Click;


            this.flightPlansCommandBar.PrimaryCommands.Add(selectFlightPlanButton);
            this.flightPlansCommandBar.PrimaryCommands.Add(refreshFlightPlanButton);
            this.flightPlansCommandBar.PrimaryCommands.Add(addFlightPlanButton);
            this.flightPlansCommandBar.PrimaryCommands.Add(this.deleteFlightPlansButton);

            //this.flightPlansCommandBar.SecondaryCommands.Add(settingsButton);
        }
    }
}
