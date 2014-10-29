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

            switch ((PersonalMinimumVerificationResult)value)
            {
                case PersonalMinimumVerificationResult.Pass:
                    result = "Minimums OK";
                    break;
                case PersonalMinimumVerificationResult.Fail:
                    result = "Under Minimums";
                    break;
                case PersonalMinimumVerificationResult.Unknown:
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
