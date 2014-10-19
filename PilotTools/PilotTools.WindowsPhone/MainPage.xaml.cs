using AirportData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace PilotTools
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        IAirportDirectory directory;

        public MainPage()
        {
            this.InitializeComponent();
            
            directory = new AirportData.OurAirports.AirportDirectory();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private async void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            this.progress.Visibility = Windows.UI.Xaml.Visibility.Visible;
            await directory.DownloadAndSaveAsync();
            this.progress.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private async void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            this.progress.Visibility = Windows.UI.Xaml.Visibility.Visible;
            await directory.LoadAsync();
            this.progress.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void btnLookup_Click(object sender, RoutedEventArgs e)
        {
            this.progress.Visibility = Windows.UI.Xaml.Visibility.Visible;
            try
            {
                var airport = directory.GetAirportData(tbLookup.Text);

                map.Center = new Geopoint(airport.Position);
                map.ZoomLevel = 14;
            }
            catch(AirportDirectoryException ex)
            {
                Debug.WriteLine(ex.Message);
            }

            this.progress.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }
    }
}
