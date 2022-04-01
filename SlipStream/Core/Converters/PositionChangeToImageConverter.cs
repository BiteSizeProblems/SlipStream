using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SlipStream.Core.Converters
{
    public class PositionChangeToImageConverter : IValueConverter
    {
        private static string basePath = "/Core/Images/Leaderboard/";
        private static string gainedPath = basePath + "green_arrow.png";
        private static string neutralPath = basePath + "dash.png";
        private static string lostPath = basePath + "red_arrow.png";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            sbyte val = (sbyte)value;

            if (val > 0)
            {
                return gainedPath;
            }
            else if ( val < 0)
            {
                return lostPath;
            }
            else
            {
                return neutralPath;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
