using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using static SlipStream.Structs.Appendeces;

namespace SlipStream.Core.Converters
{
    /// <summary>
    /// Provides methods to convert from a <see cref="string"/> (fullpath) to a <see cref="BitmapImage"/> 
    /// </summary>
    public class SafetyCarStringToImageConverter : IValueConverter
    {
        private static string basePath = "/Core/Images/SafetyCar/";
        private static string activePath = basePath + "warning.png";
        private static string notActivePath = basePath + "check.png";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SafetyCarStatus val = (SafetyCarStatus)value;

            switch (val)
            {
                case SafetyCarStatus.Clear:
                    return notActivePath;
                case SafetyCarStatus.SafetyCarActive:
                case SafetyCarStatus.VirtualSafetyCar:
                    return activePath;
                default:
                    return notActivePath;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
} 
