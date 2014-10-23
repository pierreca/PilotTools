using PilotTools.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using WeatherData;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace PilotTools.Converters
{
    public class PersonalMinimumsResultToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = new SolidColorBrush(Colors.Gray);

            switch ((PersonalMinimumsResult)value)
            {
                case PersonalMinimumsResult.Pass:
                    result = new SolidColorBrush(Colors.Green);
                    break;
                case PersonalMinimumsResult.Fail:
                    result = new SolidColorBrush(Colors.Red);
                    break;
                case PersonalMinimumsResult.Unknown:
                    result = new SolidColorBrush(Colors.SlateGray);
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
