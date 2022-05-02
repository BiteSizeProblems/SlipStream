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

        private int _fastestDriver;
        public int FastestDriver
        {
            get { return _fastestDriver; }
            set { SetField(ref _fastestDriver, value, nameof(FastestDriver)); }
        }

        // PLAYER ONLY DATA
        private uint _pitWindowIdeal;
        public uint PitWindowIdeal
        {
            get { return _pitWindowIdeal; }
            set { SetField(ref _pitWindowIdeal, value, nameof(PitWindowIdeal)); }
        }

        private uint _pitWindowLate;
        public uint PitWindowLate
        {
            get { return _pitWindowLate; }
            set { SetField(ref _pitWindowLate, value, nameof(PitWindowLate)); }
        }

        private uint _pitRejoin;
        public uint PitRejoin
        {
            get { return _pitRejoin; }
            set { SetField(ref _pitRejoin, value, nameof(PitRejoin)); }
        }

        public float[] AllPitlaneTimes = new float[22];

        private TimeSpan _averageTimeInPitlane;
        public TimeSpan AverageTimeInPitlane
        {
            get { return _averageTimeInPitlane; }
            set { SetField(ref _averageTimeInPitlane, value, nameof(AverageTimeInPitlane)); }
        }

        private int _numDriversStopped;
        public int NumDriversStopped
        {
            get { return _numDriversStopped; }
            set { SetField(ref _numDriversStopped, value, nameof(NumDriversStopped)); }
        }

        private TimeSpan _gapToCarAheadOnRejoin;
        public TimeSpan GapToCarAheadOnRejoin
        {
            get { return _gapToCarAheadOnRejoin; }
            set { SetField(ref _gapToCarAheadOnRejoin, value, nameof(GapToCarAheadOnRejoin)); }
        }

        private TimeSpan gapToCarBehindOnRejoin;
        public TimeSpan GapToCarBehindOnRejoin
        {
            get { return gapToCarBehindOnRejoin; }
            set { SetField(ref gapToCarBehindOnRejoin, value, nameof(GapToCarBehindOnRejoin)); }
        }

        // PARTICIPANTS
        private int _totalParticipants;
        public int TotalParticipants
        {
            get { return _totalParticipants; }
            set 
            { 
                SetField(ref _totalParticipants, value, nameof(TotalParticipants));
                //MaxIndex = TotalParticipants - 1;
                //MinIndex = 0;
            }
        }

        private int _NumOfActiveCars;
        public int NumOfActiveCars
        {
            get { return _NumOfActiveCars; }
            set 
            { 
                SetField(ref _NumOfActiveCars, value, nameof(NumOfActiveCars));
                MaxIndex = NumOfActiveCars - 1;
                MinIndex = 0;
            }
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
            set { SetField(ref _NumSoftTires, value, nameof(NumSoftTires)); }
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

        // AVERAGE LAP TIMES

        private TimeSpan _averageLapTime;
        public TimeSpan AverageLapTime
        {
            get { return _averageLapTime; }
            set { SetField(ref _averageLapTime, value, nameof(AverageLapTime)); }
        }

        private TimeSpan _averageSoftTime;
        public TimeSpan AverageSoftTime
        {
            get { return _averageSoftTime; }
            set { SetField(ref _averageSoftTime, value, nameof(AverageSoftTime)); }
        }

        private TimeSpan _averageMediumTime;
        public TimeSpan AverageMediumTime
        {
            get { return _averageMediumTime; }
            set { SetField(ref _averageMediumTime, value, nameof(AverageMediumTime)); }
        }

        private TimeSpan _averageHardTime;
        public TimeSpan AverageHardTime
        {
            get { return _averageHardTime; }
            set { SetField(ref _averageHardTime, value, nameof(AverageHardTime)); }
        }

        private TimeSpan _averageInterTime;
        public TimeSpan AverageInterTime
        {
            get { return _averageInterTime; }
            set { SetField(ref _averageInterTime, value, nameof(AverageInterTime)); }
        }

        private TimeSpan _averageWetTime;
        public TimeSpan AverageWetTime
        {
            get { return _averageWetTime; }
            set { SetField(ref _averageWetTime, value, nameof(AverageWetTime)); }
        }

        // AVERAGE TIRE WEAR

        private float _averageTireWear;
        public float AverageTireWear
        {
            get { return _averageTireWear; }
            set { SetField(ref _averageTireWear, value, nameof(AverageTireWear)); }
        }

        private float _averageSoftWear;
        public float AverageSoftWear
        {
            get { return _averageSoftWear; }
            set { SetField(ref _averageSoftWear, value, nameof(AverageSoftWear)); }
        }

        private float _averageSoftWearRate;
        public float AverageSoftWearRate
        {
            get { return _averageSoftWearRate; }
            set { SetField(ref _averageSoftWearRate, value, nameof(AverageSoftWearRate)); }
        }

        private float _averageMediumWear;
        public float AverageMediumWear
        {
            get { return _averageMediumWear; }
            set { SetField(ref _averageMediumWear, value, nameof(AverageMediumWear)); }
        }

        private float _averageMediumWearRate;
        public float AverageMediumWearRate
        {
            get { return _averageMediumWearRate; }
            set { SetField(ref _averageMediumWearRate, value, nameof(AverageMediumWearRate)); }
        }

        private float _averageHardWear;
        public float AverageHardWear
        {
            get { return _averageHardWear; }
            set { SetField(ref _averageHardWear, value, nameof(AverageHardWear)); }
        }

        private float _averageHardWearRate;
        public float AverageHardWearRate
        {
            get { return _averageHardWearRate; }
            set { SetField(ref _averageHardWearRate, value, nameof(AverageHardWearRate)); }
        }

        private float _averageInterWear;
        public float AverageInterWear
        {
            get { return _averageInterWear; }
            set { SetField(ref _averageInterWear, value, nameof(AverageInterWear)); }
        }

        private float _averageInterWearRate;
        public float AverageInterWearRate
        {
            get { return _averageInterWearRate; }
            set { SetField(ref _averageInterWearRate, value, nameof(AverageInterWearRate)); }
        }

        private float _averageWetWear;
        public float AverageWetWear
        {
            get { return _averageWetWear; }
            set { SetField(ref _averageWetWear, value, nameof(AverageWetWear)); }
        }

        private float _averageWetWearRate;
        public float AverageWetWearRate
        {
            get { return _averageWetWearRate; }
            set { SetField(ref _averageWetWearRate, value, nameof(AverageWetWearRate)); }
        }

        // Total Laps In Race
        // Total Time Duration of Session

        // Lead Lap Num
        // Session Time Left

        private int _totalLaps;
        public int TotalLaps
        {
            get { return _totalLaps; }
            set { SetField(ref _totalLaps, value, nameof(TotalLaps)); }
        }

        private int _lapsRemaining;
        public int LapsRemaining
        {
            get { return _lapsRemaining; }
            set { SetField(ref _lapsRemaining, value, nameof(LapsRemaining)); }
        }

        private TimeSpan _SessionTimeRemaining;
        public TimeSpan SessionTimeRemaining
        {
            get { return _SessionTimeRemaining; }
            set { SetField(ref _SessionTimeRemaining, value, nameof(SessionTimeRemaining)); }
        }

        private TimeSpan _SessionDuration;
        public TimeSpan SessionDuration
        {
            get { return _SessionDuration; }
            set { SetField(ref _SessionDuration, value, nameof(SessionDuration)); }
        }

        private string _raceLapCount;
        public string RaceLapCount
        {
            get { return _raceLapCount; }
            set { SetField(ref _raceLapCount, value, nameof(RaceLapCount)); }
        }

        private string _leftInSession;
        public string LeftInSession
        {
            get { return _leftInSession; }
            set { SetField(ref _leftInSession, value, nameof(LeftInSession)); }
        }

        // TRACK DATA

        private Tracks _circuit;
        public Tracks Circuit
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
                        GrandPrix = GrandPrix.ABU_DHABI_GRAND_PRIX;
                        break;
                    case Tracks.RedBullRing:
                        GrandPrix = GrandPrix.AUSTRIAN_GRAND_PRIX;
                        break;
                    case Tracks.BakuCity:
                        GrandPrix = GrandPrix.AZERBAIJAN_GRAND_PRIX;
                        break;
                    case Tracks.Interlagos:
                        GrandPrix = GrandPrix.BRAZILIAN_GRAND_PRIX;
                        break;
                    case Tracks.Catalunya:
                        GrandPrix = GrandPrix.SPANISH_GRAND_PRIX;
                        break;
                    case Tracks.Hanoi:
                        GrandPrix = GrandPrix.VIETNAM_GRAND_PRIX;
                        break;
                    case Tracks.Hockenheim:
                        GrandPrix = GrandPrix.GERMAN_GRAND_PRIX;
                        break;
                    case Tracks.Hungaroring:
                        GrandPrix = GrandPrix.HUNGARIAN_GRAND_PRIX;
                        break;
                    case Tracks.Imola:
                        GrandPrix = GrandPrix.ITALIAN_GRAND_PRIX_AT_IMOLA;
                        break;
                    case Tracks.JeddahCorniche:
                        GrandPrix = GrandPrix.SAUDI_ARABIAN_GRAND_PRIX;
                        break;
                    case Tracks.AlbertPark:
                        GrandPrix = GrandPrix.AUSTRALIAN_GRAND_PRIX;
                        break;
                    case Tracks.AutódromoHermanosRodríguez:
                        GrandPrix = GrandPrix.MEXICAN_GRAND_PRIX;
                        break;
                    case Tracks.Monaco:
                        GrandPrix = GrandPrix.MONACO_GRAND_PRIX;
                        break;
                    case Tracks.CircuitGillesVilleneuve:
                        GrandPrix = GrandPrix.CANADIAN_GRAND_PRIX;
                        break;
                    case Tracks.Monza:
                        GrandPrix = GrandPrix.ITALIAN_GRAND_PRIX_AT_MONZA;
                        break;
                    case Tracks.PaulRicard:
                        GrandPrix = GrandPrix.FRENCH_GRAND_PRIX;
                        break;
                    case Tracks.AlgarveInternationalCircuit:
                        GrandPrix = GrandPrix.PORTUGESE_GRAND_PRIX;
                        break;
                    case Tracks.Sakhir:
                        GrandPrix = GrandPrix.BAHRAIN_GRAND_PRIX;
                        break;
                    case Tracks.SakhirShort:
                        GrandPrix = GrandPrix.BAHRAIN_SHORT_GRAND_PRIX;
                        break;
                    case Tracks.ShanghaiInternational:
                        GrandPrix = GrandPrix.CHINESE_GRAND_PRIX;
                        break;
                    case Tracks.Silverstone:
                        GrandPrix = GrandPrix.BRITISH_GRAND_PRIX;
                        break;
                    case Tracks.MarinaBay:
                        GrandPrix = GrandPrix.SINGAPORE_GRAND_PRIX;
                        break;
                    case Tracks.SochiAutodrom:
                        GrandPrix = GrandPrix.RUSSIAN_GRAND_PRIX;
                        break;
                    case Tracks.SpaFrancorchamps:
                        GrandPrix = GrandPrix.BELGIAN_GRAND_PRIX;
                        break;
                    case Tracks.Suzuka:
                        GrandPrix = GrandPrix.JAPANESE_GRAND_PRIX;
                        break;
                    case Tracks.SuzukaShort:
                        GrandPrix = GrandPrix.JAPANESE_SHORT_GRAND_PRIX;
                        break;
                    case Tracks.CircuitOfTheAmericas:
                        GrandPrix = GrandPrix.UNITED_STATES_GRAND_PRIX;
                        break;
                    case Tracks.CircuitOfTheAmericasShort:
                        GrandPrix = GrandPrix.UNITED_STATES_SHORT_GRAND_PRIX;
                        break;
                    case Tracks.Zandvoort:
                        GrandPrix = GrandPrix.DUTCH_GRAND_PRIX;
                        break;
                }
            }
        }

        private GrandPrix _grandPrix;
        public GrandPrix GrandPrix
        {
            get { return _grandPrix; }
            set { SetField(ref _grandPrix, value, nameof(GrandPrix)); }
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

        public TimeSpan[] AllFastestLaptimes = new TimeSpan[22];

        private TimeSpan _fastestLap;
        public TimeSpan FastestLap
        {
            get { return _fastestLap; }
            set { SetField(ref _fastestLap, value, nameof(FastestLap)); }
        }

        private TimeSpan _fastestS1;
        public TimeSpan FastestS1
        {
            get { return _fastestS1; }
            set { SetField(ref _fastestS1, value, nameof(FastestS1)); }
        }

        private TimeSpan _fastestS2;
        public TimeSpan FastestS2
        {
            get { return _fastestS2; }
            set { SetField(ref _fastestS2, value, nameof(FastestS2)); }
        }

        private TimeSpan _fastestS3;
        public TimeSpan FastestS3
        {
            get { return _fastestS3; }
            set { SetField(ref _fastestS3, value, nameof(FastestS3)); }
        }

        private TimeSpan _fastestLastLap;
        public TimeSpan FastestLastLap
        {
            get { return _fastestLastLap; }
            set { SetField(ref _fastestLastLap, value, nameof(FastestLastLap)); }
        }

        // SESSION DATA

        private Formulas _Formula;
        public Formulas Formula
        {
            get { return _Formula; }
            set { SetField(ref _Formula, value, nameof(Formula)); }
        }

        private SessionTypes _CurrentSession;
        public SessionTypes CurrentSession
        {
            get { return _CurrentSession; }
            set { SetField(ref _CurrentSession, value, nameof(CurrentSession)); }
        }

        private Boolean _isRace;
        public Boolean IsRace
        {
            get { return _isRace; }
            set { SetField(ref _isRace, value, nameof(IsRace)); }
        }

        private NetworkTypes _networkGame;
        public NetworkTypes NetworkGame
        {
            get { return _networkGame; }
            set { SetField(ref _networkGame, value, nameof(NetworkGame)); }
        }

        // WEATHER DATA
        private WeatherTypes _CurrentWeather;
        public WeatherTypes CurrentWeather
        {
            get { return _CurrentWeather; }
            set { SetField(ref _CurrentWeather, value, nameof(CurrentWeather)); }
        }

        // EVENT STRING CODES
        private string _eventStringCode;
        public string EventStringCode
        {
            get { return _eventStringCode; }
            set { SetField(ref _eventStringCode, value, nameof(EventStringCode)); }
        }

        // SAFETY & HAZARDS
        private SafetyCarStatus _safetyCarStatus;
        public SafetyCarStatus SafetyCarStatus
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

        // LEADERBOARD LIGHTS

        private string _lightOne;
        public string LightOne
        {
            get { return _lightOne; }
            set { SetField(ref _lightOne, value, nameof(LightOne)); }
        }

        private string _lightTwo;
        public string LightTwo
        {
            get { return _lightTwo; }
            set { SetField(ref _lightTwo, value, nameof(LightTwo)); }
        }

        private string _lightThree;
        public string LightThree
        {
            get { return _lightThree; }
            set { SetField(ref _lightThree, value, nameof(LightThree)); }
        }

        private string _lightFour;
        public string LightFour
        {
            get { return _lightFour; }
            set { SetField(ref _lightFour, value, nameof(LightFour)); }
        }

        private string _lightFive;
        public string LightFive
        {
            get { return _lightFive; }
            set { SetField(ref _lightFive, value, nameof(LightFive)); }
        }

        private string _lightSix;
        public string LightSix
        {
            get { return _lightSix; }
            set { SetField(ref _lightSix, value, nameof(LightSix)); }
        }

        private string _lightSeven;
        public string LightSeven
        {
            get { return _lightSeven; }
            set { SetField(ref _lightSeven, value, nameof(LightSeven)); }
        }
    }
}
