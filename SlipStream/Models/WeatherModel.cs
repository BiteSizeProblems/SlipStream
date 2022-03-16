using SlipStream.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SlipStream.Structs.Appendeces;

namespace SlipStream.Models
{
    public class WeatherModel : ObservableObject
    {

        private SessionTypes _sessionType;
        public SessionTypes SessionType
        {
            get { return _sessionType; }
            set { SetField(ref _sessionType, value, nameof(SessionType)); }
        }

        private byte _timeOffset;
        public byte TimeOffset
        {
            get { return _timeOffset; }
            set { SetField(ref _timeOffset, value, nameof(TimeOffset)); }
        }

        private WeatherTypes _weather;
        public WeatherTypes Weather
        {
            get { return _weather; }
            set { SetField(ref _weather, value, nameof(Weather)); }
        }

        private string _weatherHistoryIcon;
        public string WeatherHistoryIcon
        {
            get { return _weatherHistoryIcon; }
            set { SetField(ref _weatherHistoryIcon, value, nameof(WeatherHistoryIcon)); }
        }

        private string _trackTemperature;
        public string TrackTemperature
        {
            get { return _trackTemperature; }
            set { SetField(ref _trackTemperature, value, nameof(TrackTemperature)); }
        }

        private string _airTemperature;
        public string AirTemperature
        {
            get { return _airTemperature; }
            set { SetField(ref _airTemperature, value, nameof(AirTemperature)); }
        }

        private string _rainPercentage;
        public string RainPercentage
        {
            get { return _rainPercentage; }
            set { SetField(ref _rainPercentage, value, nameof(RainPercentage)); }
        }
    }
}
