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
    public class PersonalMinimumsResultToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = string.Empty;

            switch ((PersonalMinimumsResult)value)
            {
                case PersonalMinimumsResult.Pass:
                    result = "Minimums OK";
                    break;
                case PersonalMinimumsResult.Fail:
                    result = "Under Minimums";
                    break;
                case PersonalMinimumsResult.Unknown:
                    result = "Cannot Verify Minimums";
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
