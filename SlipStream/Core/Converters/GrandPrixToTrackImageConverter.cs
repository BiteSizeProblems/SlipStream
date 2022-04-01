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
    public class GrandPrixToTrackImageConverter : IValueConverter
    {
        private static string basePath = "/Core/Images/TrackVectors/";

        private static string unknownPath = basePath + "abu_dhabi.png";

        private static string abuDhabiPath = basePath + "abu_dhabi.png";
        private static string australianPath = basePath + "australia.png";
        private static string austrianPath = basePath + "austria.png";
        private static string azerbaijanPath = basePath + "azerbaijan.png";
        private static string bahrainPath = basePath + "bahrain.png";
        private static string belgianPath = basePath + "belgium.png";
        private static string brazilPath = basePath + "brazil.png";
        private static string britishPath = basePath + "britain.png";
        private static string canadianPath = basePath + "canada.png";
        private static string chinesePath = basePath + "china.png";
        private static string dutchPath = basePath + "holland.png";
        private static string frenchPath = basePath + "france.png";
        private static string germanPath = basePath + "germany_nurburgring.png";
        private static string hungarianPath = basePath + "hungary.png";
        private static string italianImolaPath = basePath + "italy_imola.png";
        private static string italianMonzaPath = basePath + "italy_monza.png";
        private static string japanesePath = basePath + "japan.png";
        private static string mexicanPath = basePath + "mexico.png";
        private static string monacoPath = basePath + "monaco.png";
        private static string portugesePath = basePath + "portugal.png";
        private static string russianPath = basePath + "russia.png";
        private static string saudiArabianPath = basePath + "saudi_arabia.png";
        private static string singaporePath = basePath + "singapore.png";
        private static string spanishPath = basePath + "spain.png";
        private static string unitedStatesAustinPath = basePath + "usa_austin.png";
        private static string unitedStatesMiamiPath = basePath + "usa_miami.png";
        private static string vietnamPath = basePath + "vietnam.png";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GrandPrix val = (GrandPrix)value;

            switch (val)
            {
                case GrandPrix.ABU_DHABI_GRAND_PRIX:
                    return abuDhabiPath;
                case GrandPrix.AUSTRALIAN_GRAND_PRIX:
                    return australianPath;
                case GrandPrix.AUSTRIAN_GRAND_PRIX:
                    return austrianPath;
                case GrandPrix.AZERBAIJAN_GRAND_PRIX:
                    return azerbaijanPath;
                case GrandPrix.BAHRAIN_GRAND_PRIX:
                case GrandPrix.BAHRAIN_SHORT_GRAND_PRIX:
                    return bahrainPath;
                case GrandPrix.BELGIAN_GRAND_PRIX:
                    return belgianPath;
                case GrandPrix.BRAZILIAN_GRAND_PRIX:
                    return brazilPath;
                case GrandPrix.BRITISH_GRAND_PRIX:
                case GrandPrix.BRITISH_SHORT_GRAND_PRIX:
                    return britishPath;
                case GrandPrix.CANADIAN_GRAND_PRIX:
                    return canadianPath;
                case GrandPrix.CHINESE_GRAND_PRIX:
                    return chinesePath;
                case GrandPrix.DUTCH_GRAND_PRIX:
                    return dutchPath;
                case GrandPrix.FRENCH_GRAND_PRIX:
                    return frenchPath;
                case GrandPrix.GERMAN_GRAND_PRIX:
                    return germanPath;
                case GrandPrix.HUNGARIAN_GRAND_PRIX:
                    return hungarianPath;
                case GrandPrix.ITALIAN_GRAND_PRIX_AT_IMOLA:
                    return italianImolaPath;
                case GrandPrix.ITALIAN_GRAND_PRIX_AT_MONZA:
                    return italianMonzaPath;
                case GrandPrix.JAPANESE_GRAND_PRIX:
                case GrandPrix.JAPANESE_SHORT_GRAND_PRIX:
                    return japanesePath;
                case GrandPrix.MEXICAN_GRAND_PRIX:
                    return mexicanPath;
                case GrandPrix.MONACO_GRAND_PRIX:
                    return monacoPath;
                case GrandPrix.PORTUGESE_GRAND_PRIX:
                    return portugesePath;
                case GrandPrix.RUSSIAN_GRAND_PRIX:
                    return russianPath;
                case GrandPrix.SAUDI_ARABIAN_GRAND_PRIX:
                    return saudiArabianPath;
                case GrandPrix.SINGAPORE_GRAND_PRIX:
                    return singaporePath;
                case GrandPrix.SPANISH_GRAND_PRIX:
                    return spanishPath;
                case GrandPrix.UNITED_STATES_GRAND_PRIX:
                case GrandPrix.UNITED_STATES_SHORT_GRAND_PRIX:
                    return unitedStatesAustinPath;
                case GrandPrix.VIETNAM_GRAND_PRIX:
                    return vietnamPath;
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
