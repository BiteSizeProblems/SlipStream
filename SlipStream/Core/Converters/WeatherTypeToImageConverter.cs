using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using static SlipStream.Structs.Appendeces;

namespace SlipStream.Core.Converters
{
    public class WeatherTypeToImageConverter : IValueConverter
    {
        private static string basePath = "/Core/Images/WeatherIcons/";

        private static string clearPath = basePath + "clear.png";
        private static string overcastPath = basePath + "overcast.png";
        private static string cloudyPath = basePath + "light_cloud.png";
        private static string lightRainPath = basePath + "light_rain.png";
        private static string heavyRainPath = basePath + "heavy_rain.png";
        private static string stormPath = basePath + "storm.png";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            WeatherTypes val = (WeatherTypes)value;

            switch (val)
            {
                case WeatherTypes.Clear:
                    return clearPath;
                case WeatherTypes.Overcast:
                    return overcastPath;
                case WeatherTypes.LightCloud:
                    return cloudyPath;
                case WeatherTypes.LightRain:
                    return lightRainPath;
                case WeatherTypes.HeavyRain:
                    return heavyRainPath;
                case WeatherTypes.Storm:
                    return stormPath;
                default:
                    return clearPath;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
