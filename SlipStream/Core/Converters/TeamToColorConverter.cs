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
    public class TeamToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Teams val = (Teams)value;

            switch (val)
            {
                case Teams.Mercedes:
                    return "#00D2BE";
                case Teams.RedBullRacing:
                    return "#0600EF";
                case Teams.Ferrari:
                    return "#C00000";
                case Teams.Mclaren:
                    return "#FF8700";
                case Teams.Haas:
                    return "#FFFFFFFF";
                case Teams.Williams:
                    return "#0082FA";
                case Teams.AlphaTauri:
                    return "#C8C8C8";
                case Teams.Alpine:
                    return "#FF00D1FF";
                case Teams.AlfaRomeo:
                    return "#FF870000";
                case Teams.AstonMartin:
                    return "#FF2F4F4F";
                default:
                    return "#ff00ff";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
