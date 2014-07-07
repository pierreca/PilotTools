using System;
using System.Collections.Generic;
using System.Text;
using WeatherData;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace PilotTools.Converters
{
    public class FlightRulesToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = new SolidColorBrush(Colors.Gray);

            switch((FlightRules)value)
            {
                case FlightRules.VFR:
                    result = new SolidColorBrush(Colors.Green);
                    break;
                case FlightRules.MVFR:
                    result = new SolidColorBrush(Colors.Blue);
                    break;
                case FlightRules.IFR:
                    result = new SolidColorBrush(Colors.Red);
                    break;
                case FlightRules.LIFR:
                    result = new SolidColorBrush(Colors.Pink);
                    break;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
