using SlipStream.Core;
using SlipStream.Models;
using SlipStream.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using static SlipStream.Core.Appendeces;

namespace SlipStream.ViewModels
{
    public class EngineerViewModel : BaseViewModel
    {
        // === BEGINING OF MODULE SETUP ===
        // === Singleton Instance with Thread Saftey ===
        private static EngineerViewModel _instance = null;
        private static object _singletonLock = new object();
        public static EngineerViewModel GetInstance()
        {
            lock (_singletonLock)
            {
                if (_instance == null) { _instance = new EngineerViewModel(); }
                return _instance;
            }
        }
        // === END OF MODULE SETUP ===

        public RelayCommand EngineerHomeSubViewCommand { get; set; }
        public EngineerDefaultView EDV { get; set; }

        public RelayCommand EngineerStrategySubViewCommand { get; set; }
        public EngineerStrategyView ESV { get; set; }

        private object _currentSubView;
        public object CurrentSubView
        {
            get { return _currentSubView; }
            set
            {
                _currentSubView = value;
                OnPropertyChanged("CurrentSubView");
            }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { SetField(ref _selectedIndex, value, nameof(SelectedIndex)); }
        }

        private string _indexedDriverName;
        public string IndexedDriverName
        {
            get { return _indexedDriverName; }
            set { SetField(ref _indexedDriverName, value, nameof(IndexedDriverName)); }
        }

        private string _indexedDriverName2;
        public string IndexedDriverName2
        {
            get { return _indexedDriverName2; }
            set { SetField(ref _indexedDriverName2, value, nameof(IndexedDriverName2)); }
        }

        private string _indexedCurrentTire;
        public string IndexedCurrentTire
        {
            get { return _indexedCurrentTire; }
            set { SetField(ref _indexedCurrentTire, value, nameof(IndexedCurrentTire)); }
        }

        private TimeSpan _indexedLastLap;
        public TimeSpan IndexedLastLap
        {
            get { return _indexedLastLap; }
            set { SetField(ref _indexedLastLap, value, nameof(IndexedLastLap)); }
        }

        private TimeSpan _indexedBestLap;
        public TimeSpan IndexedBestLap
        {
            get { return _indexedBestLap; }
            set { SetField(ref _indexedBestLap, value, nameof(IndexedBestLap)); }
        }

        private VisualTireCompounds _indexedVisualTireCompound;
        public VisualTireCompounds IndexedVisualTireCompound
        {
            get { return _indexedVisualTireCompound; }
            set { SetField(ref _indexedVisualTireCompound, value, nameof(IndexedVisualTireCompound)); }
        }

        private string _indexedTireIconSource;
        public string IndexedTireIconSource
        {
            get { return _indexedTireIconSource; }
            set { SetField(ref _indexedTireIconSource, value, nameof(IndexedTireIconSource)); }
        }

        private string _indexedTireAge;
        public string IndexedTireAge
        {
            get { return _indexedTireAge; }
            set { SetField(ref _indexedTireAge, value, nameof(IndexedTireAge)); }
        }

        /// <summary>
        /// 
        /// </summary>

        private string _fuelMix;
        public string FuelMix
        {
            get { return _fuelMix; }
            set { SetField(ref _fuelMix, value, nameof(FuelMix)); }
        }

        private string _vehicleFlag;
        public string VehicleFlag
        {
            get { return _vehicleFlag; }
            set { SetField(ref _vehicleFlag, value, nameof(VehicleFlag)); }
        }

        private string _ersRemaining;
        public string ErsRemaining
        {
            get { return _ersRemaining; }
            set { SetField(ref _ersRemaining, value, nameof(ErsRemaining)); }
        }

        private string _ersDeployMode;
        public string ErsDeployMode
        {
            get { return _ersDeployMode; }
            set { SetField(ref _ersDeployMode, value, nameof(ErsDeployMode)); }
        }

        private string _tireWear;
        public string TireWear
        {
            get { return _tireWear; }
            set { SetField(ref _tireWear, value, nameof(TireWear)); }
            
        }


        private string position;
        public string Position
        {
            get { return position; }
            set { SetField(ref position, value, nameof(Position)); }
        }

        private TimeSpan lastS1Time;
        public TimeSpan LastS1Time
        {
            get { return lastS1Time; }
            set { SetField(ref lastS1Time, value, nameof(LastS1Time)); }
        }

        private TimeSpan lastS2Time;
        public TimeSpan LastS2Time
        {
            get { return lastS2Time; }
            set { SetField(ref lastS2Time, value, nameof(LastS2Time)); }
        }

        private TimeSpan lastS3Time;
        public TimeSpan LastS3Time
        {
            get { return lastS3Time; }
            set { SetField(ref lastS3Time, value, nameof(LastS3Time)); }
        }

        private TimeSpan bestS1Time;
        public TimeSpan BestS1Time
        {
            get { return bestS1Time; }
            set { SetField(ref bestS1Time, value, nameof(BestS1Time)); }
        }

        private TimeSpan bestS2Time;
        public TimeSpan BestS2Time
        {
            get { return bestS2Time; }
            set { SetField(ref bestS2Time, value, nameof(BestS2Time)); }
        }

        private TimeSpan bestS3Time;
        public TimeSpan BestS3Time
        {
            get { return bestS3Time; }
            set { SetField(ref bestS3Time, value, nameof(BestS3Time)); }
        }

        private TimeSpan lastLapTime;
        public TimeSpan LastLapTime
        {
            get { return lastLapTime; }
            set { SetField(ref lastLapTime, value, nameof(LastLapTime)); }
        }

        private TimeSpan bestLapTime;
        public TimeSpan BestLapTime
        {
            get { return bestLapTime; }
            set { SetField(ref bestLapTime, value, nameof(BestLapTime)); }
        }

        private int currentLap;
        public int CurrentLap
        {
            get { return currentLap; }
            set { SetField(ref currentLap, value, nameof(CurrentLap)); }
        }

        private string status;
        public string Status
        {
            get { return status; }
            set { SetField(ref status, value, nameof(Status)); }
        }

        private TimeSpan timeRemaining;
        public TimeSpan TimeRemaining
        {
            get { return timeRemaining; }
            set { SetField(ref timeRemaining, value, nameof(TimeRemaining)); }
        }

        private string warnings;
        public string Warnings
        {
            get { return warnings; }
            set { SetField(ref warnings, value, nameof(Warnings)); }
        }

        private string penalties;
        public string Penalties
        {
            get { return penalties; }
            set { SetField(ref penalties, value, nameof(Penalties)); }
        }

        private string safetyCarStatus;
        public string SafetyCarStatus
        {
            get { return safetyCarStatus; }
            set { SetField(ref safetyCarStatus, value, nameof(SafetyCarStatus)); }
        }

        private string weather;
        public string Weather
        {
            get { return weather; }
            set { SetField(ref weather, value, nameof(Weather)); }
        }

        private int _totalParticipants;
        public int TotalParticipants
        {
            get { return _totalParticipants; }
            set { SetField(ref _totalParticipants, value, nameof(TotalParticipants)); }
        }

        // Create a observable collection of DriverData
        public ObservableCollection<WeatherData> WeatherArr { get; set; }
        private object _weatherArrLock = new object();

        // Create a observable collection of DriverData
        public ObservableCollection<DriverData> DriverArr { get; set; }
        private object _driverArrLock = new object();

        // Create a observable collection of DriverData
        public ObservableCollection<IndexedDriverData> IndexDriver { get; set; }
        private object _indexDriverLock = new object();

        private EngineerViewModel() : base()
        {
            // Set Views

            EDV = new EngineerDefaultView();
            ESV = new EngineerStrategyView();

            CurrentSubView = EDV;

            EngineerHomeSubViewCommand = new RelayCommand(o =>
            {
                CurrentSubView = EDV;
            });

            EngineerStrategySubViewCommand = new RelayCommand(o =>
            {
                CurrentSubView = ESV;
            });

            // Set New Observable Collection
            WeatherArr = new ObservableCollection<WeatherData>();
            // thread safety
            BindingOperations.EnableCollectionSynchronization(WeatherArr, _weatherArrLock);

            // Set New Observable Collection
            DriverArr = new ObservableCollection<DriverData>();
            // thread safety
            BindingOperations.EnableCollectionSynchronization(DriverArr, _driverArrLock);

            // Set New Observable Collection
            IndexDriver = new ObservableCollection<IndexedDriverData>();
            // thread safety
            BindingOperations.EnableCollectionSynchronization(IndexDriver, _indexDriverLock);

            for (int i = 0; i < 22; i++)
            {
                // Add a new Default Driver
                DriverArr.Add(new DriverData());
            }

            // Could be UPTO 22 participants
            // Fill the Array up
            for (int i = 0; i < 22; i++)
            {
                // Add a new Default Driver
                WeatherArr.Add(new WeatherData());
            }

            UDPC.OnLapDataReceive += UDPC_OnLapDataReceive;
            UDPC.OnParticipantsDataReceive += UDPC_OnParticipantsDataReceive;
            UDPC.OnSessionDataReceive += UDPC_OnSessionDataReceive;
            UDPC.OnCarStatusDataReceive += UDPC_OnCarStatusDataReceive;
            UDPC.OnCarDamageDataReceive += UDPC_OnCarDamageDataReceive; ;
        }

        private void UDPC_OnCarDamageDataReceive(PacketCarDamageData packet)
        {
            // Loop through the participants the game is giving us
            for (int i = 0; i < packet.m_carDamageData.Length; i++)
            {
                var indexdata = packet.m_carDamageData[SelectedIndex];

                TireWear = $"Current Tire Wear: {indexdata.m_tyresWear}";
            }
        }

        private void UDPC_OnSessionDataReceive(PacketSessionData packet)
        {
            TimeRemaining = TimeSpan.FromSeconds(packet.sessionTimeLeft);
            SafetyCarStatus = "Safety Car Status: " + Regex.Replace(packet.safetyCarStatus.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
            Weather = "Weather: " + Regex.Replace(packet.weather.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();

            for(int i = 0; i < packet.weatherForecastSamples.Length; i++)
            {

            }
        }

        private void UDPC_OnParticipantsDataReceive(PacketParticipantsData packet)
        {

            TotalParticipants = packet.m_participants.Length;

            // List of All Drivers
            for (int i = 0; i < packet.m_participants.Length; i++)
            {
                var participant = packet.m_participants[i];
                DriverArr[i].Index = i;
                //DriverArr[i].DriverName = Regex.Replace(participant.m_driverId.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
                DriverArr[i].raceNumber = participant.m_raceNumber;
            }

            int playerIndex = packet.m_header.playerCarIndex;
            var playerData = packet.m_participants[4];
            var indexdata = packet.m_participants[SelectedIndex];

            //IndexedDriverName = Regex.Replace(indexdata.m_driverId.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
        }

        private void UDPC_OnCarStatusDataReceive(PacketCarStatusData packet)
        {
            // Loop through the participants the game is giving us
            for (int i = 0; i < packet.m_carStatusData.Length; i++)
            {
                var indexdata = packet.m_carStatusData[SelectedIndex];

                var carStatusData = packet.m_carStatusData[i];
                // Update it in the array
                DriverArr[i].VisualTireCompound = (VisualTireCompounds)carStatusData.m_visualTyreCompound;

                IndexedTireAge = $"Laps on Current Tire: {indexdata.m_tyresAgeLaps}";

                IndexedVisualTireCompound = (VisualTireCompounds)indexdata.m_visualTyreCompound;
                
                //if(IndexDriverStatus != InGarage)

                if(indexdata.m_fuelMix == 0)
                {
                    FuelMix = $"Current Fuel Mix: Lean";
                }
                if (indexdata.m_fuelMix == 0)
                {
                    FuelMix = $"Current Fuel Mix: Standard";
                }
                if (indexdata.m_fuelMix == 0)
                {
                    FuelMix = $"Current Fuel Mix: Rich";
                }
                if (indexdata.m_fuelMix == 0)
                {
                    FuelMix = $"Current Fuel Mix: Max";
                }



                VehicleFlag = $"Zone Flags: {indexdata.m_vehicleFiaFlags}";
                
                ErsRemaining = $"ERS Remaining: {(indexdata.m_ersStoreEnergy / 40000)}%";
                
                if(indexdata.m_ersDeployMode == 0)
                {
                    ErsDeployMode = $"ERS Mode: None";
                }
                if (indexdata.m_ersDeployMode == 1)
                {
                    ErsDeployMode = $"ERS Mode: Medium";
                }
                if (indexdata.m_ersDeployMode == 2)
                {
                    ErsDeployMode = $"ERS Mode: Hotlap";
                }
                if (indexdata.m_ersDeployMode == 3)
                {
                    ErsDeployMode = $"ERS Mode: Overtake";
                }




                if (IndexedVisualTireCompound == VisualTireCompounds.Soft)
                {
                    IndexedTireIconSource = "/Core/Images/CustomSoft.png";
                }
                if (IndexedVisualTireCompound == VisualTireCompounds.Medium)
                {
                    IndexedTireIconSource = "/Core/Images/CustomMedium.png";
                }
                if (IndexedVisualTireCompound == VisualTireCompounds.Hard)
                {
                    IndexedTireIconSource = "/Core/Images/CustomHard.png";
                }

            }
        }

        private void UDPC_OnLapDataReceive(PacketLapData packet)
        {
            var indexdata = packet.lapData[SelectedIndex];

            Position = "Position: " + indexdata.carPosition;
            LastLapTime = TimeSpan.FromMilliseconds(indexdata.lastLapTimeInMS);
            CurrentLap = indexdata.currentLapNum - 1;
            Status = "Status: " + Regex.Replace(indexdata.driverStatus.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
            LastS1Time = TimeSpan.FromMilliseconds(indexdata.sector1TimeInMS);
            LastS2Time = TimeSpan.FromMilliseconds(indexdata.sector2TimeInMS);
            LastS3Time = (TimeSpan.FromMilliseconds(indexdata.lastLapTimeInMS - indexdata.sector2TimeInMS - indexdata.lastLapTimeInMS));
            Warnings = $"Warnings: {indexdata.warnings}";
            Penalties = $"Penalties: {TimeSpan.FromSeconds(indexdata.penalties)}";

            for (int i = 0; i < packet.lapData.Length; i++)
            {
                var lapData = packet.lapData[i];

                DriverArr[i].CarPosition = lapData.carPosition;
            }

            IndexedLastLap = TimeSpan.FromMilliseconds(indexdata.lastLapTimeInMS);

        }

        public class IndexedDriverData : ObservableObject
        {
            private int _indexCarNum;
            public int IndexCarNum
            {
                get { return _indexCarNum; }
                set { SetField(ref _indexCarNum, value, nameof(IndexCarNum)); }
            }
        }

        // MODEL 
        public class DriverData : ObservableObject
        {
            private int _index;
            public int Index
            {
                get { return _index; }
                set { SetField(ref _index, value, nameof(Index)); }
            }

            private Drivers _driverID;
            public Drivers DriverID
            {
                get { return _driverID; }
                set { SetField(ref _driverID, value, nameof(DriverID)); }
            }

            private string _driverName;
            public string DriverName
            {
                get { return _driverName; }
                set { SetField(ref _driverName, value, nameof(DriverName)); }
            }

            private string _driverTag;
            public string DriverTag
            {
                get { return _driverTag; }
                set { SetField(ref _driverTag, value, nameof(DriverTag)); }
            }

            private byte ai;
            public byte AI
            {
                get { return ai; }
                set { SetField(ref ai, value, nameof(AI)); }
            }

            private Teams _teamID;
            public Teams TeamID
            {
                get { return _teamID; }
                set
                {
                    SetField(ref _teamID, value, nameof(TeamID));

                    if (this.TeamID != value)
                    {
                        this._teamID = value;
                        switch (this._teamID)
                        {
                            default:
                                this.TeamColor = new SolidColorBrush(Color.FromRgb(255, 0, 255));
                                break;
                            case Teams.Mercedes:
                            case Teams.Mercedes2020:
                                this.TeamColor = new SolidColorBrush(Color.FromRgb(0, 210, 90));
                                break;
                            case Teams.Ferrari:
                            case Teams.Ferrari2020:
                                this.TeamColor = new SolidColorBrush(Color.FromRgb(220, 0, 0));
                                break;
                            case Teams.RedBullRacing:
                            case Teams.RedBull2020:
                                this.TeamColor = new SolidColorBrush(Color.FromRgb(6, 0, 239));
                                break;
                            case Teams.Alpine:
                            case Teams.Renault2020:
                                this.TeamColor = new SolidColorBrush(Color.FromRgb(0, 144, 255));
                                break;
                            case Teams.Haas:
                            case Teams.Haas2020:
                                this.TeamColor = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                                break;
                            case Teams.AstonMartin:
                            case Teams.RacingPoint2020:
                                this.TeamColor = new SolidColorBrush(Color.FromRgb(0, 111, 98));
                                break;
                            case Teams.AlphaTauri:
                            case Teams.AlphaTauri2020:
                                this.TeamColor = new SolidColorBrush(Color.FromRgb(43, 69, 98));
                                break;
                            case Teams.Mclaren:
                            case Teams.McLaren2020:
                                this.TeamColor = new SolidColorBrush(Color.FromRgb(255, 135, 0));
                                break;
                            case Teams.AlfaRomeo:
                            case Teams.AlfaRomeo2020:
                                this.TeamColor = new SolidColorBrush(Color.FromRgb(144, 0, 0));
                                break;
                            case Teams.Williams:
                            case Teams.Williams2020:
                                this.TeamColor = new SolidColorBrush(Color.FromRgb(0, 90, 255));
                                break;
                        }
                    }
                }
            }

            private string _teamName;
            public string TeamName
            {
                get { return _teamName; }
                set { SetField(ref _teamName, value, nameof(TeamName)); }
            }

            private SolidColorBrush _teamColor;
            public SolidColorBrush TeamColor
            {
                get
                {
                    return this._teamColor;
                }
                set
                {
                    if (value != this.TeamColor)
                    {
                        this._teamColor = value;
                        this.OnPropertyChanged("TeamColor");
                    }
                }
            }

            private string _teamRect;
            public string TeamRect
            {
                get
                {
                    return this._teamRect;
                }
                set
                {
                    if (value != this._teamRect)
                    {
                        this._teamRect = value;
                        this.OnPropertyChanged("TeamRect");
                    }
                }
            }

            private int _raceNumber;
            public int raceNumber
            {
                get { return _raceNumber; }
                set { SetField(ref _raceNumber, value, nameof(raceNumber)); }
            }

            private float _currentLapTime;
            public float CurrentLapTime
            {
                get { return _currentLapTime; }
                set { SetField(ref _currentLapTime, value, nameof(CurrentLapTime)); }
            }

            private string _tireIconSource;
            public string TireIconSource
            {
                get { return _tireIconSource; }
                set { SetField(ref _tireIconSource, value, nameof(TireIconSource)); }
            }

            private DriverStatus _driverStatusUpdate;
            public DriverStatus DriverStatusUpdate
            {
                get { return _driverStatusUpdate; }
                set { SetField(ref _driverStatusUpdate, value, nameof(DriverStatusUpdate)); }
            }

            private string _driverStatusSource;
            public string DriverStatusSource
            {
                get { return _driverStatusSource; }
                set { SetField(ref _driverStatusSource, value, nameof(DriverStatusSource)); }
            }

            private TimeSpan lastLapTime;
            public TimeSpan LastLapTime
            {
                get { return lastLapTime; }
                set
                {
                    SetField(ref lastLapTime, value, nameof(LastLapTime));

                    if (CurrentLapNum < 1)
                    {
                        BestLapTime = LastLapTime;
                    }
                    else if (LastLapTime < BestLapTime)
                    {
                        BestLapTime = value;
                    }
                }
            }

            private TimeSpan bestLapTime;
            public TimeSpan BestLapTime
            {
                get { return bestLapTime; }
                set { SetField(ref bestLapTime, value, nameof(BestLapTime)); }
            }

            private TimeSpan bestLapGap;
            public TimeSpan BestLapGap
            {
                get { return bestLapGap; }
                set { SetField(ref bestLapGap, value, nameof(BestLapGap)); }
            }

            private TimeSpan sector1Time;
            public TimeSpan Sector1Time
            {
                get { return sector1Time; }
                set { SetField(ref sector1Time, value, nameof(Sector1Time)); }
            }

            private TimeSpan sector2Time;
            public TimeSpan Sector2Time
            {
                get { return sector2Time; }
                set { SetField(ref sector2Time, value, nameof(Sector2Time)); }
            }

            private TimeSpan sector3Time;
            public TimeSpan Sector3Time
            {
                get { return sector3Time; }
                set { SetField(ref sector3Time, value, nameof(Sector3Time)); }
            }

            private byte carPosition;
            public byte CarPosition
            {
                get { return carPosition; }
                set
                {
                    SetField(ref carPosition, value, nameof(CarPosition));

                }
            }

            private int currentLapNum;
            public int CurrentLapNum
            {
                get { return currentLapNum; }
                set { SetField(ref currentLapNum, value, nameof(CurrentLapNum)); }
            }

            private string driverStatus;
            public string DriverStatus
            {
                get { return driverStatus; }
                set { SetField(ref driverStatus, value, nameof(DriverStatus)); }
            }

            private VisualTireCompounds visualTireCompound;
            public VisualTireCompounds VisualTireCompound
            {
                get { return visualTireCompound; }
                set { SetField(ref visualTireCompound, value, nameof(VisualTireCompound)); }
            }

            private string numTireStints;
            public string NumTireStints
            {
                get { return numTireStints; }
                set { SetField(ref numTireStints, value, nameof(NumTireStints)); }
            }

            // Args CTOR
            public DriverData(Drivers d, Teams t, int r)
            {
                this.DriverID = d;
                this.TeamID = t;
                this.raceNumber = r;
            }

            // NoArgs CTOR
            public DriverData()
            {
                this.DriverID = Drivers.Unknown;
                this.TeamID = Teams.Unknown;
                this.CurrentLapTime = 0;
            }
        }

    }

    

    // MODEL 
    public class WeatherData : ObservableObject
    {
        private WeatherTypes weather;
        public WeatherTypes Weather
        {
            get { return weather; }
            set { SetField(ref weather, value, nameof(Weather)); }
        }

        // Args CTOR
        public WeatherData(WeatherTypes d)
        {
            this.Weather = d;
        }

        // NoArgs CTOR
        public WeatherData()
        {
            //this.Weather = WeatherTypes.Unknown;
        }
    }
}
