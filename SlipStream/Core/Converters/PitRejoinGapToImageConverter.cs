using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SlipStream.Core.Converters
{
    public class PitRejoinGapToImageConverter : IValueConverter
    {
        private static string basePath = "/Core/Images/Lights/";
        private static string greenPath = basePath + "green_circle.png";
        private static string yellowPath = basePath + "yellow_circle.png";
        private static string redPath = basePath + "red_circle.png";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan val = (TimeSpan)value;

            if (val < TimeSpan.FromSeconds(2))
            {
                return redPath;
            }
            else if (val < TimeSpan.FromSeconds(5))
            {
                return yellowPath;
            }
            else
            {
                return greenPath;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
