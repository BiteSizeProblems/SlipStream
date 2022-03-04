using SlipStream.Core;
using SlipStream.Models;
using SlipStream.Structs;
using SlipStream.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using static SlipStream.Structs.Appendeces;

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

        public SessionModel model { get; set; }
        public DriverModel driver { get; set; }
        public WeatherModel w_model { get; set; }

        public RelayCommand RaceControlHomeSubViewCommand { get; set; }
        public RaceControlDefaultView RCDV { get; set; }

        public RelayCommand LeaderboardSubViewCommand { get; set; }
        public RaceControlLeaderboardView RCLV { get; set; }

        public RelayCommand WeatherSubViewCommand { get; set; }
        public RaceControlWeatherView RCWV { get; set; }

        public RelayCommand DetailSubViewCommand { get; set; }
        public RaceControlDetailView RCDetV { get; set; }

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

        private int _numActiveCars;
        public int NumActiveCars
        {
            get { return _numActiveCars; }
            set { SetField(ref _numActiveCars, value, nameof(NumActiveCars)); }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                SetField(ref _selectedIndex, value, nameof(SelectedIndex));
                CurrentSelectedDriver = Driver[value];
            }
        }

        private DriverModel _currSelectedDriver;
        public DriverModel CurrentSelectedDriver
        {
            get { return _currSelectedDriver; }
            set { SetField(ref _currSelectedDriver, value, nameof(CurrentSelectedDriver)); }
        }

        public TimeSpan SectorMin = TimeSpan.FromSeconds(0);

        // Create a observable collection of DriverModel
        public ObservableCollection<DriverModel> Driver { get; set; }
        private object _driverLock = new object();

        // Create a observable collection of DriverModel
        public ObservableCollection<WeatherModel> W_Model { get; set; }
        private object _w_modelLock = new object();

        private RaceControlViewModel() : base()
        {
            // Set Views
            RCDV = new RaceControlDefaultView();
            RCLV = new RaceControlLeaderboardView();
            RCWV = new RaceControlWeatherView();
            RCDetV = new RaceControlDetailView();


            CurrentSubView = RCDV;

            RaceControlHomeSubViewCommand = new RelayCommand(o =>
            {
                CurrentSubView = RCDV;
            });

            LeaderboardSubViewCommand = new RelayCommand(o =>
            {
                CurrentSubView = RCLV;
            });

            WeatherSubViewCommand = new RelayCommand(o =>
            {
                CurrentSubView = RCWV;
            });

            DetailSubViewCommand = new RelayCommand(o =>
            {
                CurrentSubView = RCDetV;
            });

            this.model = new SessionModel();

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

            // Set New Observable Collection
            Driver = new ObservableCollection<DriverModel>();

            // thread safety
            BindingOperations.EnableCollectionSynchronization(Driver, _driverLock);

            for (int i = 0; i < 22; i++)
            {
                // Add a new Default Driver
                Driver.Add(new DriverModel());
                Driver[i].DriverName = "Placeholder";
                Driver[i].DriverIndex = i + 1;
            }

            // Set New Observable Collection
            W_Model = new ObservableCollection<WeatherModel>();
            BindingOperations.EnableCollectionSynchronization(W_Model, _w_modelLock);

            for (int i = 0; i < 56; i++)
            {
                W_Model.Add(new WeatherModel());
            }

            UDPC.OnLapDataReceive += UDPC_OnLapDataReceive;
            UDPC.OnParticipantsDataReceive += UDPC_OnParticipantDataReceive;
            UDPC.OnCarStatusDataReceive += UDPC_OnCarStatusDataReceive;
            UDPC.OnEventDataReceive += UDPC_OnEventDataReceive;
            UDPC.OnFinalClassificationDataReceive += UDPC_OnFinalClassificationDataReceive;
            UDPC.OnSessionDataReceive += UDPC_OnSessionDataReceive;
            UDPC.OnSessionHistoryDataReceive += UDPC_OnSessionHistoryDataReceive;
        }

        private void UDPC_OnSessionHistoryDataReceive(PacketSessionHistoryData packet)
        {
            for(int i = 0; i < packet.m_lapHistoryData.Length; i++)
            {
                var lapHistory = packet.m_lapHistoryData[i];
                var tireHistory = packet.m_tyreStintsHistoryData[i];

                Driver[i].lapNum = Driver[i].lapNum + 1;
                Driver[i].CarIdx = packet.m_carIdx;
                Driver[i].LapTime = lapHistory.m_lapTimeInMS;

                if(Driver[i].CurrentLapTime > SectorMin)
                {
                    //Driver[i].S1Time = TimeSpan.FromMilliseconds(lapHistory.m_sector1TimeInMS);
                    //Driver[i].S2Time = TimeSpan.FromMilliseconds(lapHistory.m_sector2TimeInMS);
                    //Driver[i].S3Time = TimeSpan.FromMilliseconds(lapHistory.m_sector3TimeInMS);
                }

                if(Driver[i].S1Time < SectorMin)
                {

                }
                
            }
        }

        private void UDPC_OnCarStatusDataReceive(PacketCarStatusData packet)
        {
            // Loop through the participants the game is giving us
            for (int i = 0; i < packet.m_carStatusData.Length; i++)
            {
                var carStatusData = packet.m_carStatusData[i];
                // Update it in the array
                Driver[i].VisualTireCompound = (VisualTireCompounds)carStatusData.m_visualTyreCompound;
               
                if(Driver[i].VisualTireCompound == VisualTireCompounds.Soft)
                {
                    Driver[i].TireIconSource = "/Core/Images/CustomSoft.png";
                }
                if (Driver[i].VisualTireCompound == VisualTireCompounds.Medium)
                {
                    Driver[i].TireIconSource = "/Core/Images/CustomMedium.png";
                }
                if (Driver[i].VisualTireCompound == VisualTireCompounds.Hard)
                {
                    Driver[i].TireIconSource = "/Core/Images/CustomHard.png";
                }
            }
        }

        private void UDPC_OnSessionDataReceive(PacketSessionData packet)
        {
            model.Formula = ("Formula: " + Regex.Replace(packet.formula.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim());
            model.Circuit = ("Circuit: " + Regex.Replace(packet.trackId.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim());
            model.CurrentSession = ("Session Type: " + Regex.Replace(packet.sessionType.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim());
            model.CurrentWeather = ("Weather: " + Regex.Replace(packet.weather.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim());

            for (int i = 0; i < packet.weatherForecastSamples.Length; i++)
            {
                var forecast = packet.weatherForecastSamples[i];

                W_Model[i].SessionType = forecast.m_sessionType;
                W_Model[i].TimeOffset = forecast.m_timeOffset;
                W_Model[i].Weather = forecast.m_weather;
                W_Model[i].TrackTemperature = $"{(sbyte)((forecast.m_trackTemperature * 1.8) + 32)} F";
                W_Model[i].AirTemperature = $"{(sbyte)((forecast.m_airTemperature * 1.8) + 32)} F";
                W_Model[i].RainPercentage = $"{forecast.m_rainPercentage}%";
            }
            
            if (packet.networkGame == 0)
            {
                model.NetworkGame = "Network Type: Offline";
            }
            else
            {
                model.NetworkGame = "Network Type: Online";
            }

            if(packet.sessionType != SessionTypes.Race)
            {
                model.SessionTimeRemaining = $"Time Remaining: {TimeSpan.FromSeconds(packet.sessionTimeLeft)}";
                model.SessionDuration = $"Session Duration: {TimeSpan.FromSeconds(packet.sessionDuration)}";
            }
            else
            {
                model.SessionTimeRemaining = $"{model.LeadLap} / {packet.totalLaps}";
                model.SessionDuration = $"# of Laps: {packet.totalLaps.ToString()}";
            }
        }

        private void UDPC_OnFinalClassificationDataReceive(PacketFinalClassificationData packet)
        {
            // Loop through the participants the game is giving us
            for (int i = 0; i < packet.m_classificationData.Length; i++)
            {
                var finalData = packet.m_classificationData[i];

                Driver[i].CarPosition = (byte)finalData.m_position;
                Driver[i].CurrentLapNum = (int)finalData.m_numLaps;
                Driver[i].BestLapTime = TimeSpan.FromMilliseconds(finalData.m_bestLapTimeInMS);
                //Driver[i].NumTireStints = finalData.m_numTyreStints.ToString();
            }
        }

        private void UDPC_OnEventDataReceive(PacketEventData packet)
        {
            string s = new string(packet.m_eventStringCode);

            if (s == "FTLP")
            {
                model.EventStringCode = "New Fastest Lap";
                Flap = TimeSpan.FromSeconds(packet.m_eventDetails.fastestLap.lapTime);
            }
            if (s == "SPTP")
            {
                model.EventStringCode = "New Speed Trap Speed Set";
                SpeedTrap = packet.m_eventDetails.speedTrap.speed;
            }
            if (s == "SEND")
            {
                model.EventStringCode = "Session End";
                model.SessionTimeRemaining = "Session Complete";
                // Loop through the participants the game is giving us
                for (int i = 0; i < model.NumOfParticipants; i++)
                {
                    if (Driver[i].CarPosition == 1)
                    {
                        model.SessionFastestLap = Driver[i].BestLapTime;
                    }

                    Driver[i].DriverStatus = "";
                    Driver[i].BestLapGap = model.SessionFastestLap - Driver[i].BestLapTime;
                }
            }
            if (s == "PENA")
            {
                model.EventStringCode = "Penalty Issued";
            }
        }

        private void UDPC_OnLapDataReceive(PacketLapData packet)
        {
            // Loop through the participants the game is giving us
            for (int i = 0; i < packet.lapData.Length; i++)
            {
                var lapData = packet.lapData[i];
                // Update it in the array
                Driver[i].CurrentLapTime = TimeSpan.FromMilliseconds(lapData.currentLapTimeInMS);
                Driver[i].CarPosition = lapData.carPosition;
                Driver[i].GridPosition = lapData.gridPosition;
                Driver[i].LastLapTime = TimeSpan.FromMilliseconds(lapData.lastLapTimeInMS);
                Driver[i].Sector1Time = TimeSpan.FromMilliseconds(lapData.sector1TimeInMS);
                Driver[i].Sector2Time = TimeSpan.FromMilliseconds(lapData.sector2TimeInMS);
                Driver[i].Sector3Time = (TimeSpan.FromMilliseconds(lapData.lastLapTimeInMS - lapData.sector2TimeInMS - lapData.lastLapTimeInMS));
                Driver[i].CurrentLapNum = lapData.currentLapNum -1;
                Driver[i].DriverStatus = Regex.Replace(lapData.driverStatus.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
                Driver[i].DriverStatusUpdate = lapData.driverStatus;

                // POSITION CHANGE
                if(model.CurrentSession == "Race")
                {   
                    Driver[i].PositionChange = (sbyte)(Driver[i].CarPosition - Driver[i].GridPosition);

                    if(Driver[i].PositionChange > 0)
                    {
                        Driver[i].PositionChangeIcon = "/Core/Images/up_arrow.png";
                    }
                    else if(Driver[i].PositionChange < 0)
                    {
                        Driver[i].PositionChangeIcon = "/Core/Images/down_arrow.png";
                    }
                }
                

                // BEST LAP GAP
                if(model.CurrentSession != "Race")
                {
                    if (Driver[i].CurrentLapNum > 0)
                    {
                        Driver[i].BestLapGap = TimeSpan.FromMilliseconds(lapData.lastLapTimeInMS) - model.SessionFastestLap;
                    }
                }

                // SESSION FASTEST LAP & LEAD LAP NUM
                if (Driver[i].CarPosition == 1)
                {
                    model.SessionFastestLap = Driver[i].BestLapTime;
                    model.LeadLap = Driver[i].CurrentLapNum + 1;
                }

                // SECTOR TIMES

                Driver[i].S1Time = TimeSpan.FromMilliseconds(lapData.sector1TimeInMS);
                Driver[i].S2Time = TimeSpan.FromMilliseconds(lapData.sector2TimeInMS);

                if (Driver[i].DriverStatusUpdate != DriverStatus.InGarage && Driver[i].S1Time != SectorMin && Driver[i].S2Time != SectorMin)
                {
                    Driver[i].S3Time = TimeSpan.FromMilliseconds(lapData.currentLapTimeInMS - lapData.sector1TimeInMS - lapData.sector2TimeInMS);
                }
                else Driver[i].S3Time = SectorMin;

                
                
                // DRIVER STATUS ICON & BEST SECTOR TIMES
                if (Driver[i].DriverStatusUpdate == DriverStatus.InGarage)
                {
                    Driver[i].DriverStatusSource = "/Core/Images/Garage.png";
                }
                if (Driver[i].DriverStatusUpdate == DriverStatus.OutLap)
                {
                    Driver[i].DriverStatusSource = "/Core/Images/out.png";
                }
                if (Driver[i].DriverStatusUpdate == DriverStatus.InLap)
                {
                    Driver[i].DriverStatusSource = "/Core/Images/redbox.png";
                }
                if (Driver[i].DriverStatusUpdate == DriverStatus.FlyingLap)
                {
                    Driver[i].DriverStatusSource = "/Core/Images/fast.png";
                }
            }
        }

        private void UDPC_OnParticipantDataReceive(PacketParticipantsData packet)
        {

            model.NumOfActiveCars = $"Active Cars: {packet.m_numActiveCars.ToString()}";
            model.TotalParticipants = packet.m_participants.Length;

            // Loop through the participants the game is giving us
            for (int i = 0; i < packet.m_participants.Length; i++)
            {
                var participant = packet.m_participants[i];
                string s = new string(participant.m_name);

                // Update them in the array
                Driver[i].TeamID = participant.m_teamId;
                Driver[i].TeamName = Regex.Replace(participant.m_teamId.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
                Driver[i].raceNumber = participant.m_raceNumber;
                Driver[i].AI = participant.m_aiControlled;

                if(model.NetworkGame == "Network Type: Offline")
                {
                    Driver[i].DriverName = Regex.Replace(participant.m_driverId.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
                }
                
                if (Driver[i].TeamID == Teams.Mercedes)
                {
                    Driver[i].TeamRect = "#00D2BE";
                }
                if (Driver[i].TeamID == Teams.RedBullRacing)
                {
                    Driver[i].TeamRect = "#0600EF";
                }
                if (Driver[i].TeamID == Teams.Ferrari)
                {
                    Driver[i].TeamRect = "#C00000";
                }
                if (Driver[i].TeamID == Teams.Mclaren)
                {
                    Driver[i].TeamRect = "#FF8700";
                }
                if (Driver[i].TeamID == Teams.Haas)
                {
                    Driver[i].TeamRect = "#0600EF";
                }
                if (Driver[i].TeamID == Teams.Williams)
                {
                    Driver[i].TeamRect = "#0082FA";
                }
                if (Driver[i].TeamID == Teams.AlphaTauri)
                {
                    Driver[i].TeamRect = "#C8C8C8";
                }
                if (Driver[i].TeamID == Teams.Alpine)
                {
                    Driver[i].TeamRect = "#FF2F4F4F";
                }
                if (Driver[i].TeamID == Teams.AlfaRomeo)
                {
                    Driver[i].TeamRect = "#960000";
                }
                if (Driver[i].TeamID == Teams.AstonMartin)
                {
                    Driver[i].TeamRect = "#FF2F4F4F";
                }
            }
        }
    }
}
