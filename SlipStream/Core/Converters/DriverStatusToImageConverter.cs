using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SlipStream.Core.Converters
{
    public class DriverStatusToImageConverter : IValueConverter
    {
        private static string basePath = "/Core/Images/Lights/";

        private static string unknownPath = basePath + "blue_circle.png";

        // PRACTICE & QUALIFYING
        private static string yellowPath = basePath + "yellow_circle.png";
        private static string greenPath = basePath + "green_circle.png";
        private static string orangePath = basePath + "orange_circle.png";
        private static string blackPath = basePath + "black_circle.png";
        private static string redPath = basePath + "red_circle.png";
        private static string bluePath = basePath + "blue_circle.png";
        private static string whitePath = basePath + "white_circle.png";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string val = value.ToString();

            switch (val)
            {
                case "Flying_Lap":
                case "Active":
                    return greenPath;
                case "In_Lap":
                    return orangePath;
                case "Out_Lap":
                case "In_This_Lap":
                    return yellowPath;
                case "In_Pit_Lane":
                    return bluePath;
                case "In_Garage":
                case "Finished":
                    return whitePath;
                case "Inactive":
                case "DNF":
                case "Retired":
                    return blackPath;
                case "DSQ":
                    return redPath;
                default:
                    return unknownPath;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
