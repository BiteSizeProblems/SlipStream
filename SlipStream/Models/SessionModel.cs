using SlipStream.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SlipStream.Structs.Appendeces;

namespace SlipStream.Models
{
    // MODEL
    public class SessionModel : ObservableObject
    {

        // PARTICIPANTS
        private int _totalParticipants;
        public int TotalParticipants
        {
            get { return _totalParticipants; }
            set 
            { 
                SetField(ref _totalParticipants, value, nameof(TotalParticipants));
                MaxIndex = TotalParticipants - 1;
                MinIndex = 0;
            }
        }

        private int _NumOfActiveCars;
        public int NumOfActiveCars
        {
            get { return _NumOfActiveCars; }
            set { SetField(ref _NumOfActiveCars, value, nameof(NumOfActiveCars)); }
        }

        private int _NumOfParticipants;
        public int NumOfParticipants
        {
            get { return _NumOfParticipants; }
            set { SetField(ref _NumOfParticipants, value, nameof(NumOfParticipants)); }
        }

        // INDEXING
        private int _maxIndex;
        public int MaxIndex
        {
            get { return _maxIndex; }
            set { SetField(ref _maxIndex, value, nameof(MaxIndex)); }
        }

        private int _minIndex;
        public int MinIndex
        {
            get { return _minIndex; }
            set { SetField(ref _minIndex, value, nameof(MinIndex)); }
        }

        // DRIVER STATISTICAL DATA

        private int _NumSoftTires;
        public int NumSoftTires
        {
            get { return _NumSoftTires; }
            set 
            { 
                SetField(ref _NumSoftTires, value, nameof(NumSoftTires)); 
            }
        }

        private int _NumMediumTires;
        public int NumMediumTires
        {
            get { return _NumMediumTires; }
            set { SetField(ref _NumMediumTires, value, nameof(NumMediumTires)); }
        }

        private int _NumHardTires;
        public int NumHardTires
        {
            get { return _NumHardTires; }
            set { SetField(ref _NumHardTires, value, nameof(NumHardTires)); }
        }

        private int _NumInterTires;
        public int NumInterTires
        {
            get { return _NumInterTires; }
            set { SetField(ref _NumInterTires, value, nameof(NumInterTires)); }
        }

        private int _NumWetTires;
        public int NumWetTires
        {
            get { return _NumWetTires; }
            set { SetField(ref _NumWetTires, value, nameof(NumWetTires)); }
        }

        // LAP & TIME DATA

        private TimeSpan _averageSoftTime;
        public TimeSpan AverageSoftTime
        {
            get { return _averageSoftTime; }
            set { SetField(ref _averageSoftTime, value, nameof(AverageSoftTime)); }
        }

        private int _totalLaps;
        public int TotalLaps
        {
            get { return _totalLaps; }
            set { SetField(ref _totalLaps, value, nameof(TotalLaps)); }
        }

        private string _lapsRemaining;
        public string LapsRemaining
        {
            get { return _lapsRemaining; }
            set { SetField(ref _lapsRemaining, value, nameof(LapsRemaining)); }
        }

        private string _SessionTimeRemaining;
        public string SessionTimeRemaining
        {
            get { return _SessionTimeRemaining; }
            set { SetField(ref _SessionTimeRemaining, value, nameof(SessionTimeRemaining)); }
        }

        private string _SessionDuration;
        public string SessionDuration
        {
            get { return _SessionDuration; }
            set { SetField(ref _SessionDuration, value, nameof(SessionDuration)); }
        }

        // TRACK DATA

        private string _circuit;
        public string Circuit
        {
            get { return _circuit; }
            set { SetField(ref _circuit, value, nameof(Circuit)); }
        }

        private Tracks _track;
        public Tracks Track
        {
            get { return _track; }
            set 
            { 
                SetField(ref _track, value, nameof(Track));

                switch (Track)
                {
                    case Tracks.YasMarina:
                        TrackIcon = "/Core/Images/Tracks/abudhabi.png";
                        break;
                    case Tracks.RedBullRing:
                        TrackIcon = "/Core/Images/Tracks/austria.png";
                        break;
                    case Tracks.BakuCity:
                        TrackIcon = "/Core/Images/Tracks/azerbaijan.png";
                        break;
                    case Tracks.Interlagos:
                        TrackIcon = "/Core/Images/Tracks/brazil.png";
                        break;
                    case Tracks.Catalunya:
                        TrackIcon = "/Core/Images/Tracks/spain.png";
                        break;
                    case Tracks.Hanoi:
                        TrackIcon = "/Core/Images/Tracks/vietnam.png";
                        break;
                    case Tracks.Hockenheim:
                        TrackIcon = "/Core/Images/Tracks/germany.png";
                        break;
                    case Tracks.Hungaroring:
                        TrackIcon = "/Core/Images/Tracks/hungary.png";
                        break;
                    case Tracks.Imola:
                        TrackIcon = "/Core/Images/Tracks/italy_imola.png";
                        break;
                    case Tracks.JeddahCorniche:
                        TrackIcon = "/Core/Images/Tracks/saudiarabia.png";
                        break;
                    case Tracks.AlbertPark:
                        TrackIcon = "/Core/Images/Tracks/australia.png";
                        break;
                    case Tracks.AutódromoHermanosRodríguez:
                        TrackIcon = "/Core/Images/Tracks/mexico.png";
                        break;
                    case Tracks.Monaco:
                        TrackIcon = "/Core/Images/Tracks/monaco.png";
                        break;
                    case Tracks.CircuitGillesVilleneuve:
                        TrackIcon = "/Core/Images/Tracks/canada.png";
                        break;
                    case Tracks.Monza:
                        TrackIcon = "/Core/Images/Tracks/italy_monza.png";
                        break;
                    case Tracks.PaulRicard:
                        TrackIcon = "/Core/Images/Tracks/france.png";
                        break;
                    case Tracks.AlgarveInternationalCircuit:
                        TrackIcon = "/Core/Images/Tracks/portugal.png";
                        break;
                    case Tracks.Sakhir:
                        TrackIcon = "/Core/Images/Tracks/bahrain.png";
                        break;
                    case Tracks.SakhirShort:
                        TrackIcon = "/Core/Images/Tracks/bahrain_short.png";
                        break;
                    case Tracks.ShanghaiInternational:
                        TrackIcon = "/Core/Images/Tracks/china.png";
                        break;
                    case Tracks.Silverstone:
                        TrackIcon = "/Core/Images/Tracks/britain.png";
                        break;
                    case Tracks.MarinaBay:
                        TrackIcon = "/Core/Images/Tracks/singapore.png";
                        break;
                    case Tracks.SochiAutodrom:
                        TrackIcon = "/Core/Images/Tracks/russia.png";
                        break;
                    case Tracks.SpaFrancorchamps:
                        TrackIcon = "/Core/Images/Tracks/belgium.png";
                        break;
                    case Tracks.Suzuka:
                        TrackIcon = "/Core/Images/Tracks/japan.png";
                        break;
                    case Tracks.SuzukaShort:
                        TrackIcon = "/Core/Images/Tracks/japan_short.png";
                        break;
                    case Tracks.CircuitOfTheAmericas:
                        TrackIcon = "/Core/Images/Tracks/usa.png";
                        break;
                    case Tracks.CircuitOfTheAmericasShort:
                        TrackIcon = "/Core/Images/Tracks/usa_short.png";
                        break;
                    case Tracks.Zandvoort:
                        TrackIcon = "/Core/Images/Tracks/holland.png";
                        break;
                }
            }
        }

        private string _trackIcon;
        public string TrackIcon
        {
            get { return _trackIcon; }
            set { SetField(ref _trackIcon, value, nameof(TrackIcon)); }
        }


        // SINGLE DRIVER DATA

        private int _leadLap;
        public int LeadLap
        {
            get { return _leadLap; }
            set { SetField(ref _leadLap, value, nameof(LeadLap)); }
        }

        private TimeSpan _leadLapTime;
        public TimeSpan LeadLapTime
        {
            get { return _leadLapTime; }
            set { SetField(ref _leadLapTime, value, nameof(LeadLapTime)); }
        }

        private TimeSpan sessionFastestLap;
        public TimeSpan SessionFastestLap
        {
            get { return sessionFastestLap; }
            set { SetField(ref sessionFastestLap, value, nameof(SessionFastestLap)); }
        }

        private string sessionFastestLapID;
        public string SessionFastestLapID
        {
            get { return sessionFastestLapID; }
            set { SetField(ref sessionFastestLapID, value, nameof(SessionFastestLapID)); }
        }

        // SESSION DATA

        private string _Formula;
        public string Formula
        {
            get { return _Formula; }
            set { SetField(ref _Formula, value, nameof(Formula)); }
        }

        private string _CurrentSession;
        public string CurrentSession
        {
            get { return _CurrentSession; }
            set { SetField(ref _CurrentSession, value, nameof(CurrentSession)); }
        }

        private SessionTypes _sessionType;
        public SessionTypes SessionType
        {
            get { return _sessionType; }
            set { SetField(ref _sessionType, value, nameof(SessionType)); }
        }

        private NetworkTypes _networkGame;
        public NetworkTypes NetworkGame
        {
            get { return _networkGame; }
            set { SetField(ref _networkGame, value, nameof(NetworkGame)); }
        }

        // WEATHER DATA
        private string _CurrentWeather;
        public string CurrentWeather
        {
            get { return _CurrentWeather; }
            set { SetField(ref _CurrentWeather, value, nameof(CurrentWeather)); }
        }

        private string _CurrentWeatherIcon;
        public string CurrentWeatherIcon
        {
            get { return _CurrentWeatherIcon; }
            set { SetField(ref _CurrentWeatherIcon, value, nameof(CurrentWeatherIcon)); }
        }

        // EVENT STRING CODES
        private string _eventStringCode;
        public string EventStringCode
        {
            get { return _eventStringCode; }
            set { SetField(ref _eventStringCode, value, nameof(EventStringCode)); }
        }

        // SAFETY & HAZARDS
        private string _safetyCarStatus;
        public string SafetyCarStatus
        {
            get { return _safetyCarStatus; }
            set { SetField(ref _safetyCarStatus, value, nameof(SafetyCarStatus)); }
        }

        private string _safetyCarColor;
        public string SafetyCarColor
        {
            get { return _safetyCarColor; }
            set { SetField(ref _safetyCarColor, value, nameof(SafetyCarColor)); }
        }

        private string _safetyCarIcon;
        public string SafetyCarIcon
        {
            get { return _safetyCarIcon; }
            set { SetField(ref _safetyCarIcon, value, nameof(SafetyCarIcon)); }
        }
    }
}
