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
    public class TeamToLogoConverter : IValueConverter
    {
        private static string basePath = "/Core/Images/TeamIcons/";

        private static string mercedesPath = basePath + "mercedes.png";
        private static string redbullPath = basePath + "red_bull.png";
        private static string ferrariPath = basePath + "ferrari.png";
        private static string mclarenPath = basePath + "mclaren.png";
        private static string haasPath = basePath + "haas.png";
        private static string williamsPath = basePath + "williams.png";
        private static string alphatauriPath = basePath + "alpha_tauri.png";
        private static string alpinePath = basePath + "alpine.png";
        private static string alfaromeoPath = basePath + "alfa_romeo.png";
        private static string astonmartinPath = basePath + "aston_martin.png";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Teams val = (Teams)value;

            switch (val)
            {
                case Teams.Mercedes:
                    return mercedesPath;
                case Teams.RedBullRacing:
                    return redbullPath;
                case Teams.Ferrari:
                    return ferrariPath;
                case Teams.Mclaren:
                    return mclarenPath;
                case Teams.Haas:
                    return haasPath;
                case Teams.Williams:
                    return williamsPath;
                case Teams.AlphaTauri:
                    return alphatauriPath;
                case Teams.Alpine:
                    return alpinePath;
                case Teams.AlfaRomeo:
                    return alfaromeoPath;
                case Teams.AstonMartin:
                    return astonmartinPath;
                default:
                    return mercedesPath;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
