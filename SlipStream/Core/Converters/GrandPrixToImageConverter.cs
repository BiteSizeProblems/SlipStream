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
    public class GrandPrixToImageConverter : IValueConverter
    {
        private static string basePath = "/Core/Images/Flags/";

        private static string unknownPath = basePath + "Unknown.png";

        private static string abuDhabiPath = basePath + "Emirian.png";
        private static string australianPath = basePath + "Australian.png";
        private static string austrianPath = basePath + "Austrian.png";
        private static string azerbaijanPath = basePath + "Azerbaijani.png";
        private static string bahrainPath = basePath + "Bahraini.png";
        private static string belgianPath = basePath + "Belgian.png";
        private static string brazilPath = basePath + "Brazilian.png";
        private static string britishPath = basePath + "British.png";
        private static string canadianPath = basePath + "Canadian.png";
        private static string chinesePath = basePath + "Chinese.png";
        private static string dutchPath = basePath + "Dutch.png";
        private static string frenchPath = basePath + "French.png";
        private static string germanPath = basePath + "German.png";
        private static string hungarianPath = basePath + "Hungarian.png";
        private static string italianPath = basePath + "Italian.png";
        private static string japanesePath = basePath + "Japanese.png";
        private static string mexicanPath = basePath + "Mexican.png";
        private static string monacoPath = basePath + "Monegasque.png";
        private static string portugesePath = basePath + "Portugese.png";
        private static string russianPath = basePath + "Russian.png";
        private static string saudiArabianPath = basePath + "Saudi.png";
        private static string singaporePath = basePath + "Singaporean.png";
        private static string spanishPath = basePath + "Spanish.png";
        private static string unitedStatesPath = basePath + "American.png";
        private static string vietnamPath = basePath + "Vietnamese.png";

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
                case GrandPrix.ITALIAN_GRAND_PRIX_AT_MONZA:
                    return italianPath;
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
                    return unitedStatesPath;
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
