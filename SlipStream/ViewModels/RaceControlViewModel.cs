using SlipStream.Core;
using SlipStream.Models;
using SlipStream.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using static SlipStream.Core.Appendeces;

namespace SlipStream.ViewModels
{
    public class RaceControlViewModel : BaseViewModel
    {
        // === BEGINING OF MODULE SETUP ===
        // === Singleton Instance with Thread Saftey ===
        private static RaceControlViewModel _instance = null;
        private static object _singletonLock = new object();
        public static RaceControlViewModel GetInstance()
        {
            lock (_singletonLock)
            {
                if (_instance == null) { _instance = new RaceControlViewModel(); }
                return _instance;
            }
        }
        // === END OF MODULE SETUP ===

        public RelayCommand RaceControlHomeSubViewCommand { get; set; }
        public RaceControlDefaultView RCDV { get; set; }

        public RelayCommand LeaderboardSubViewCommand { get; set; }
        public RaceControlLeaderboardView RCLV { get; set; }

        public RelayCommand ExportLeaderboardCommand { get; set; }

        private object _currentSubView;
        public object CurrentSubView
        {
            get { return _currentSubView; }
            set
            { _currentSubView = value;
              OnPropertyChanged("CurrentSubView");
            }
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

        private string _SessionTimeRemaining;
        public string SessionTimeRemaining
        {
            get { return _SessionTimeRemaining; }
            set { SetField(ref _SessionTimeRemaining, value, nameof(SessionTimeRemaining)); }
        }

        private string _Formula;
        public string Formula
        {
            get { return _Formula; }
            set { SetField(ref _Formula, value, nameof(Formula)); }
        }

        private string _Circuit;
        public string Circuit
        {
            get { return _Circuit; }
            set { SetField(ref _Circuit, value, nameof(Circuit)); }
        }

        private string _CurrentSession;
        public string CurrentSession
        {
            get { return _CurrentSession; }
            set { SetField(ref _CurrentSession, value, nameof(CurrentSession)); }
        }

        private int _networkGame;
        public int NetworkGame
        {
            get { return _networkGame; }
            set { SetField(ref _networkGame, value, nameof(NetworkGame)); }
        }

        private string _CurrentWeather;
        public string CurrentWeather
        {
            get { return _CurrentWeather; }
            set { SetField(ref _CurrentWeather, value, nameof(CurrentWeather)); }
        }

        private string _SessionDuration;
        public string SessionDuration
        {
            get { return _SessionDuration; }
            set { SetField(ref _SessionDuration, value, nameof(SessionDuration)); }
        }

        private string _NumOfActiveCars;
        public string NumOfActiveCars
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

        private float _speedTrap;
        public float SpeedTrap
        {
            get { return _speedTrap; }
            set { SetField(ref _speedTrap, value, nameof(SpeedTrap)); }
        }

        private TimeSpan _flap;
        public TimeSpan Flap
        {
            get { return _flap; }
            set { SetField(ref _flap, value, nameof(Flap)); }
        }

        private string _eventStringCode;
        public string EventStringCode
        {
            get { return _eventStringCode; }
            set { SetField(ref _eventStringCode, value, nameof(EventStringCode)); }
        }

        // Create a observable collection of DriverData
        public ObservableCollection<DriverData> DriverArr { get; set; }
        private object _driverArrLock = new object();

        private RaceControlViewModel() : base()
        {
            // Set Views
            RCDV = new RaceControlDefaultView();
            RCLV = new RaceControlLeaderboardView();

            CurrentSubView = RCDV;

            RaceControlHomeSubViewCommand = new RelayCommand(o =>
            {
                CurrentSubView = RCDV;
            });

            LeaderboardSubViewCommand = new RelayCommand(o =>
            {
                CurrentSubView = RCLV;
            });

            ExportLeaderboardCommand = new RelayCommand(o =>
            {
                RCLV.Leaderboard.SelectAllCells();
                RCLV.Leaderboard.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
                ApplicationCommands.Copy.Execute(null, RCLV.Leaderboard);
                RCLV.Leaderboard.UnselectAllCells();

                String result = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
                try
                {
                    StreamWriter sw = new StreamWriter("slipstream_leaderboard.csv");
                    sw.WriteLine(result);
                    sw.Close();
                    Process.Start("slipstream_leaderboard.csv");
                }
                catch (Exception ex)
                { }
            });

            for(int i = 0; i < 22; i++)
            {
                //DriverArr[i].DriverName = "Player";
            }
            //RCLV.Leaderboard.Items.IsLiveSorting = true;
            //RCLV.Leaderboard.Items.SortDescriptions.Add(new SortDescription("CarPosition", ListSortDirection.Ascending));

            // Set New Observable Collection
            DriverArr = new ObservableCollection<DriverData>();

            // thread safety
            BindingOperations.EnableCollectionSynchronization(DriverArr, _driverArrLock);

            for (int i = 0; i < 22; i++)
            {
                // Add a new Default Driver
                DriverArr.Add(new DriverData());
            }
            UDPC.OnLapDataReceive += UDPC_OnLapDataReceive;
            UDPC.OnParticipantsDataReceive += UDPC_OnParticipantDataReceive;
            UDPC.OnCarStatusDataReceive += UDPC_OnCarStatusDataReceive;
            UDPC.OnEventDataReceive += UDPC_OnEventDataReceive;
            UDPC.OnFinalClassificationDataReceive += UDPC_OnFinalClassificationDataReceive;
            UDPC.OnSessionDataReceive += UDPC_OnSessionDataReceive;
        }

        private void UDPC_OnCarStatusDataReceive(PacketCarStatusData packet)
        {
            // Loop through the participants the game is giving us
            for (int i = 0; i < packet.m_carStatusData.Length; i++)
            {
                var carStatusData = packet.m_carStatusData[i];
                // Update it in the array
                DriverArr[i].VisualTireCompound = (VisualTireCompounds)carStatusData.m_visualTyreCompound;
                
                if(DriverArr[i].VisualTireCompound == VisualTireCompounds.Soft)
                {
                    DriverArr[i].TireIconSource = "/Core/Images/CustomSoft.png";
                }
                if (DriverArr[i].VisualTireCompound == VisualTireCompounds.Medium)
                {
                    DriverArr[i].TireIconSource = "/Core/Images/CustomMedium.png";
                }
                if (DriverArr[i].VisualTireCompound == VisualTireCompounds.Hard)
                {
                    DriverArr[i].TireIconSource = "/Core/Images/CustomHard.png";
                }

            }
        }

        private void UDPC_OnSessionDataReceive(PacketSessionData packet)
        {
            Formula = ("Formula: " + Regex.Replace(packet.formula.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim());
            Circuit = ("Circuit: " + Regex.Replace(packet.trackId.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim());
            CurrentSession = ("Session Type: " + Regex.Replace(packet.sessionType.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim());
            CurrentWeather = ("Weather: " + Regex.Replace(packet.weather.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim());
            SessionDuration = $"Session Duration: {TimeSpan.FromSeconds(packet.sessionDuration)}";
            SessionTimeRemaining = $"Time Remaining: {TimeSpan.FromSeconds(packet.sessionTimeLeft)}";
            NetworkGame = packet.networkGame;
        }

        private void UDPC_OnFinalClassificationDataReceive(PacketFinalClassificationData packet)
        {
            // Loop through the participants the game is giving us
            for (int i = 0; i < packet.m_classificationData.Length; i++)
            {
                var finalData = packet.m_classificationData[i];
                // Update it in the array
                DriverArr[i].CarPosition = (byte)finalData.m_position;
                DriverArr[i].CurrentLapNum = (int)finalData.m_numLaps;
                DriverArr[i].BestLapTime = TimeSpan.FromMilliseconds(finalData.m_bestLapTimeInMS);
                DriverArr[i].NumTireStints = finalData.m_numTyreStints.ToString();
            }
        }

        private void UDPC_OnEventDataReceive(PacketEventData packet)
        {
            //SessionFastestLap = TimeSpan.FromSeconds(packet.m_eventDetails.fastestLap.lapTime);

            string s = new string(packet.m_eventStringCode);

            if (s == "FTLP")
            {
                EventStringCode = "New Fastest Lap";
                Flap = TimeSpan.FromSeconds(packet.m_eventDetails.fastestLap.lapTime);
            }
            if (s == "SPTP")
            {
                EventStringCode = "New Speed Trap Speed Set";
                SpeedTrap = packet.m_eventDetails.speedTrap.speed;
            }
            if (s == "SEND")
            {
                EventStringCode = "Session End";

                // Loop through the participants the game is giving us
                for (int i = 0; i < NumOfParticipants; i++)
                {
                    if (DriverArr[i].CarPosition == 1)
                    {
                        SessionFastestLap = DriverArr[i].BestLapTime;
                    }

                    DriverArr[i].DriverStatus = "";
                    DriverArr[i].BestLapGap = SessionFastestLap - DriverArr[i].BestLapTime;
                }
            }
            if (s == "PENA")
            {
                EventStringCode = "Penalty Issued";
            }




        }

        private void UDPC_OnLapDataReceive(PacketLapData packet)
        {

            // Loop through the participants the game is giving us
            for (int i = 0; i < packet.lapData.Length; i++)
            {
                var lapData = packet.lapData[i];

                // Update it in the array

                DriverArr[i].CarPosition = lapData.carPosition;
                DriverArr[i].LastLapTime = TimeSpan.FromMilliseconds(lapData.lastLapTimeInMS);
                DriverArr[i].Sector1Time = TimeSpan.FromMilliseconds(lapData.sector1TimeInMS);
                DriverArr[i].Sector2Time = TimeSpan.FromMilliseconds(lapData.sector2TimeInMS);
                DriverArr[i].Sector3Time = (TimeSpan.FromMilliseconds(lapData.lastLapTimeInMS - lapData.sector2TimeInMS - lapData.lastLapTimeInMS));
                DriverArr[i].CurrentLapNum = lapData.currentLapNum -1;
                DriverArr[i].DriverStatus = Regex.Replace(lapData.driverStatus.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
                DriverArr[i].DriverStatusUpdate = lapData.driverStatus;

                //if(CurrentSession != "Race")
                //{
                if (DriverArr[i].CurrentLapNum > 0)
                    {
                        DriverArr[i].BestLapGap = TimeSpan.FromMilliseconds(lapData.lastLapTimeInMS) - SessionFastestLap;
                    }
                //}

                if (DriverArr[i].CarPosition == 1)
                {
                    SessionFastestLap = DriverArr[i].BestLapTime;
                }

                if (DriverArr[i].DriverStatusUpdate == DriverStatus.InGarage)
                {
                    DriverArr[i].DriverStatusSource = "/Core/Images/Garage.png";
                }
                if (DriverArr[i].DriverStatusUpdate == DriverStatus.OutLap)
                {
                    DriverArr[i].DriverStatusSource = "/Core/Images/out.png";
                }
                if (DriverArr[i].DriverStatusUpdate == DriverStatus.InLap)
                {
                    DriverArr[i].DriverStatusSource = "/Core/Images/redbox.png";
                }
                if (DriverArr[i].DriverStatusUpdate == DriverStatus.FlyingLap)
                {
                    DriverArr[i].DriverStatusSource = "/Core/Images/fast.png";
                }

            }
        }

        private void UDPC_OnParticipantDataReceive(PacketParticipantsData packet)
        {
            //NumOfParticipants = $"Participants: {packet.m_numActiveCars}";

            // Loop through the participants the game is giving us
            for (int i = 0; i < packet.m_participants.Length; i++)
            {
                var participant = packet.m_participants[i];
                string s = new string(participant.m_name);

                // Update them in the array
                DriverArr[i].TeamID = participant.m_teamId;
                DriverArr[i].TeamName = Regex.Replace(participant.m_teamId.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
                DriverArr[i].raceNumber = participant.m_raceNumber;
                DriverArr[i].AI = participant.m_aiControlled;

                if(NetworkGame == 0)
                {
                    //DriverArr[i].DriverName = Regex.Replace(participant.m_driverId.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
                }
                if (NetworkGame == 1)
                {
                    if (DriverArr[i].AI != 0)
                    {
                        //DriverArr[i].DriverName = Regex.Replace(participant.m_driverId.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
                    }
                    if (DriverArr[i].AI == 0)
                    {
                        //DriverArr[i].DriverName = s;
                    }
                }
                if (DriverArr[i].TeamID == Teams.Mercedes)
                {
                    DriverArr[i].TeamRect = "#FF2F4F4F";
                }
                if (DriverArr[i].TeamID == Teams.RedBullRacing)
                {
                    DriverArr[i].TeamRect = "#FF2F4F4F";
                }
                if (DriverArr[i].TeamID == Teams.Ferrari)
                {
                    DriverArr[i].TeamRect = "#FF2F4F4F";
                }
                if (DriverArr[i].TeamID == Teams.Mclaren)
                {
                    DriverArr[i].TeamRect = "#FF2F4F4F";
                }
                if (DriverArr[i].TeamID == Teams.Haas)
                {
                    DriverArr[i].TeamRect = "#FF2F4F4F";
                }
                if (DriverArr[i].TeamID == Teams.Williams)
                {
                    DriverArr[i].TeamRect = "#FF2F4F4F";
                }
                if (DriverArr[i].TeamID == Teams.AlphaTauri)
                {
                    DriverArr[i].TeamRect = "#FF2F4F4F";
                }
                if (DriverArr[i].TeamID == Teams.Alpine)
                {
                    DriverArr[i].TeamRect = "#FF2F4F4F";
                }
                if (DriverArr[i].TeamID == Teams.AlfaRomeo)
                {
                    DriverArr[i].TeamRect = "#FF2F4F4F";
                }
                if (DriverArr[i].TeamID == Teams.AstonMartin)
                {
                    DriverArr[i].TeamRect = "#FF2F4F4F";
                }

            }
        }

        // MODEL 
        public class DriverData : ObservableObject
        {
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
}
