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

            switch ((PersonalMinimumVerificationResult)value)
            {
                case PersonalMinimumVerificationResult.Pass:
                    result = new SolidColorBrush(Colors.Green);
                    break;
                case PersonalMinimumVerificationResult.Fail:
                    result = new SolidColorBrush(Colors.Red);
                    break;
                case PersonalMinimumVerificationResult.Unknown:
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
