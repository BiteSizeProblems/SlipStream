using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SlipStream.Core.Converters
{
    public class TireWearToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            float val = (float)value;

            if ( val > 75f)
            {
                return "#FFB10000";
            }
            else if ( val > 50f)
            {
                return "#FFDDD803";
            }
            else
            {
                return "#FF2EB521";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
