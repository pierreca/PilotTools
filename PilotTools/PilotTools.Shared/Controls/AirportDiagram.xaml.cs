using AirportData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace PilotTools.Controls
{
    public sealed partial class AirportDiagram : UserControl
    {
        public static DependencyProperty AirportProperty = DependencyProperty.Register(
            "Airport", 
            typeof(AirportData.OurAirports.Airport), 
            typeof(AirportDiagram),
            null
            );

        public AirportData.OurAirports.Airport Airport
        {
            get { return (AirportData.OurAirports.Airport)this.GetValue(AirportProperty); }
            set { this.SetValue(AirportProperty, value); }
        }
        public AirportDiagram()
        {
            this.InitializeComponent();
        }

        public void DrawCompass()
        {
            var circle = new Ellipse();
            circle.Width = this.drawingSurface.Width - 10;
            circle.Height = circle.Width;
            circle.StrokeThickness = 2;
            circle.Stroke = new SolidColorBrush(Colors.Goldenrod);
            circle.Margin = new Thickness(5);

            this.drawingSurface.Children.Add(circle);
        }


        public void DrawRunways()
        {
            // find the outer coordinates

            var westmost = this.Airport.Position;
            var eastmost = this.Airport.Position;
            var northmost = this.Airport.Position;
            var southmost = this.Airport.Position;

            foreach (var r in this.Airport.Runways)
            {
                if (r.End1.Position.Latitude < southmost.Latitude)
                {
                    southmost = r.End1.Position;
                }

                if (r.End2.Position.Latitude < southmost.Latitude)
                {
                    southmost = r.End2.Position;
                }

                if (r.End1.Position.Latitude > northmost.Latitude)
                {
                    northmost = r.End1.Position;
                }

                if (r.End2.Position.Latitude > northmost.Latitude)
                {
                    northmost = r.End2.Position;
                }

                if (r.End1.Position.Longitude < eastmost.Longitude)
                {
                    eastmost = r.End1.Position;
                }

                if (r.End2.Position.Longitude < eastmost.Longitude)
                {
                    eastmost = r.End2.Position;
                }

                if (r.End1.Position.Longitude > westmost.Longitude)
                {
                    westmost = r.End1.Position;
                }

                if (r.End2.Position.Longitude > westmost.Longitude)
                {
                    westmost = r.End2.Position;
                }
            }

            var eastward = Math.Abs(eastmost.Longitude - this.Airport.Position.Longitude);
            var westward = Math.Abs(westmost.Longitude - this.Airport.Position.Longitude);

            var northward = Math.Abs(northmost.Latitude - this.Airport.Position.Latitude);
            var southward = Math.Abs(southmost.Latitude - this.Airport.Position.Latitude);

            var eastwest = (eastward > westward) ? eastward * 2 : westward * 2;
            var northsouth = (northward > southward) ? northward * 2 : southward * 2;

            // add some margin
            northsouth += northsouth / 10;
            eastwest += eastwest / 10;

            // find the gps coordinates corresponding to the angles and add some margin

            BasicGeoposition topleft, topright, bottomleft, bottomright;

            topleft = new BasicGeoposition();
            topleft.Latitude = this.Airport.Position.Latitude + northsouth / 2;
            topleft.Longitude = this.Airport.Position.Longitude - eastwest / 2;

            topright = new BasicGeoposition();
            topright.Latitude = this.Airport.Position.Latitude + northsouth / 2;
            topright.Longitude = this.Airport.Position.Longitude + eastwest / 2;

            bottomleft = new BasicGeoposition();
            bottomleft.Latitude = this.Airport.Position.Latitude - northsouth / 2;
            bottomleft.Longitude = this.Airport.Position.Longitude - eastwest / 2;

            bottomright = new BasicGeoposition();
            bottomright.Latitude = this.Airport.Position.Latitude - northsouth / 2;
            bottomright.Longitude = this.Airport.Position.Longitude + eastwest / 2;

            // compute the corresponding canvas coordinates
            this.drawingSurface.Width = ((Windows.UI.Xaml.Controls.Grid)(this.Parent)).ActualWidth;
            this.drawingSurface.Height = this.drawingSurface.Width;

            double scale, topMargin, leftMargin;

            if (eastwest > northsouth)
            {
                scale = this.drawingSurface.Width / eastwest;
                topMargin = (this.drawingSurface.Height - (northsouth * scale)) / 2;
                leftMargin = 0;
            }
            else
            {
                scale = this.drawingSurface.Height / northsouth;
                topMargin = 0;
                leftMargin = (this.drawingSurface.Width - (eastwest * scale)) / 2; 
            }

            this.drawingSurface.Children.Clear();
            this.drawingSurface.Background = new SolidColorBrush(Colors.Black);

            foreach (var rwy in this.Airport.Runways)
            {
                var line = new Line();
                line.X1 = Math.Abs(rwy.End1.Position.Longitude - topleft.Longitude) * scale + leftMargin;
                line.Y1 = Math.Abs(rwy.End1.Position.Latitude - topleft.Latitude) * scale + topMargin;
                line.X2 = Math.Abs(rwy.End2.Position.Longitude - topleft.Longitude) * scale + leftMargin;
                line.Y2 = Math.Abs(rwy.End2.Position.Latitude - topleft.Latitude) * scale + topMargin;

                line.Stroke = new SolidColorBrush(Colors.White);
                line.Opacity = 0.8;
                line.StrokeThickness = 10;

                this.drawingSurface.Children.Add(line);
            }
        }

        public void SetKBVS()
        {

            this.Airport = new AirportData.OurAirports.Airport();
            this.Airport.Position = new BasicGeoposition()
            {
                Latitude = 48.4709014893,
                Longitude = -122.42099762
            };


            //48.4641,-122.424    48.4686,-122.414
            IRunway rwy1 = new AirportData.OurAirports.Runway()
            {
                End1 = new RunwayEnd()
                {
                    Position = new BasicGeoposition()
                    {
                        Latitude = 48.4641,
                        Longitude = -122.424
                    }
                },

                End2 = new RunwayEnd()
                {
                    Position = new BasicGeoposition()
                    {
                        Latitude = 48.4686,
                        Longitude = -122.414
                    }
                }
            };

            //48.4778,-122.431    48.4689,-122.413
            IRunway rwy2 = new AirportData.OurAirports.Runway()
            {
                End1 = new RunwayEnd()
                {
                    Position = new BasicGeoposition()
                    {
                        Latitude = 48.4778,
                        Longitude = -122.431
                    }
                },

                End2 = new RunwayEnd()
                {
                    Position = new BasicGeoposition()
                    {
                        Latitude = 48.4689,
                        Longitude = -122.413
                    }
                }
            };

            var runways = new List<IRunway>(2);
            runways.Add(rwy1);
            runways.Add(rwy2);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(this.Airport != null)
            {
                this.DrawRunways();
                this.DrawCompass();
            }
        }
    }
}
