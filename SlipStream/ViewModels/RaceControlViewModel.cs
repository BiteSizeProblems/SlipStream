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
        public SessionHistoryModel s_history { get; set; }

        public RelayCommand RaceControlHomeSubViewCommand { get; set; }
        public RaceControlDefaultView RCDV { get; set; }

        public RelayCommand LeaderboardSubViewCommand { get; set; }
        public RaceControlLeaderboardView RCLV { get; set; }

        public RelayCommand WeatherSubViewCommand { get; set; }
        public RaceControlWeatherView RCWV { get; set; }

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

        public TimeSpan SectorMin = TimeSpan.FromMilliseconds(0);

        public TimeSpan SectorMin2 = TimeSpan.FromSeconds(15);

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

            this.model = new SessionModel();

            // Set New Observable Collection
            Driver = new ObservableCollection<DriverModel>();
            BindingOperations.EnableCollectionSynchronization(Driver, _driverLock);
            W_Model = new ObservableCollection<WeatherModel>();
            BindingOperations.EnableCollectionSynchronization(W_Model, _w_modelLock);

            for (int i = 0; i < 22; i++)
            {
                // Add a new Default Driver
                Driver.Add(new DriverModel());
                Driver[i].DriverName = "Placeholder";
                Driver[i].DriverIndex = i + 1;
            }

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
                Driver[i].LapTime = TimeSpan.FromMilliseconds(lapHistory.m_lapTimeInMS);
            }
        }

        private void UDPC_OnCarStatusDataReceive(PacketCarStatusData packet)
        {
            for (int i = 0; i < packet.m_carStatusData.Length; i++)
            {
                var carStatusData = packet.m_carStatusData[i];

                Driver[i].VisualTireCompound = (VisualTireCompounds)carStatusData.m_visualTyreCompound;

                switch (Driver[i].VisualTireCompound)
                {
                    case VisualTireCompounds.Soft:
                        Driver[i].TireIconSource = "/Core/Images/CustomSoft.png";
                        break;
                    case VisualTireCompounds.Medium:
                        Driver[i].TireIconSource = "/Core/Images/CustomMedium.png";
                        break;
                    case VisualTireCompounds.Hard:
                        Driver[i].TireIconSource = "/Core/Images/CustomHard.png";
                        break;
                    case VisualTireCompounds.Inter:
                        Driver[i].TireIconSource = "/Core/Images/CustomInt.png";
                        break;
                    case VisualTireCompounds.Wet:
                        Driver[i].TireIconSource = "/Core/Images/CustomWet.png";
                        break;
                }
            }
        }

        private void UDPC_OnSessionDataReceive(PacketSessionData packet)
        {
            model.Formula = Regex.Replace(packet.m_formula.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
            model.Circuit = Regex.Replace(packet.m_trackId.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
            model.CurrentSession = Regex.Replace(packet.m_sessionType.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
            model.CurrentWeather = Regex.Replace(packet.m_weather.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
            model.SafetyCarStatus = Regex.Replace(packet.m_safetyCarStatus.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
            model.NetworkGame = packet.m_networkGame.ToString();

            // NETWORK STATUS
            if (packet.m_networkGame == 0)
            {
                //model.NetworkGame = "Offline";
            }
            //else { model.NetworkGame = "Online"; }

            // SAFETY CAR STATUS
            if (model.SafetyCarStatus == "Clear")
            {
                model.SafetyCarIcon = "/Core/Images/check.png";
            }
            else if (model.SafetyCarStatus != "Clear")
            {
                model.SafetyCarIcon = "/Core/Images/warning.png";
            }

            // WEATHER ICON
            switch (model.CurrentWeather)
            {
                case "Clear":
                    model.CurrentWeatherIcon = "/Core/Images/clear.png";
                    break;
                case "Overcast":
                    model.CurrentWeatherIcon = "/Core/Images/overcast.png";
                    break;
                case "Light Rain":
                    model.CurrentWeatherIcon = "/Core/Images/light_rain.png";
                    break;
                case "Heavy Rain":
                    model.CurrentWeatherIcon = "/Core/Images/heavy_rain.png";
                    break;
                case "Storm":
                    model.CurrentWeatherIcon = "/Core/Images/storm.png";
                    break;
            }

            for (int i = 0; i < packet.m_numWeatherForecastSamples; i++)
            {
                var forecast = packet.m_weatherForecastSamples[i];

                W_Model[i].SessionType = (SessionTypes)forecast.m_sessionType;
                W_Model[i].TimeOffset = forecast.m_timeOffset;
                W_Model[i].Weather = (WeatherTypes)forecast.m_weather;
                W_Model[i].TrackTemperature = $"{(sbyte)((forecast.m_trackTemperature * 1.8) + 32)} F";
                W_Model[i].AirTemperature = $"{(sbyte)((forecast.m_airTemperature * 1.8) + 32)} F";
                W_Model[i].RainPercentage = $"{forecast.m_rainPercentage}%";
            }

            if(packet.m_sessionType != SessionTypes.Race)
            {
                model.SessionTimeRemaining = $"Time Remaining: {TimeSpan.FromSeconds(packet.m_sessionTimeLeft)}";
                model.SessionDuration = $"Session Duration: {TimeSpan.FromSeconds(packet.m_sessionDuration)}";
            }
            else
            {
                model.SessionTimeRemaining = $"{model.LeadLap} / {packet.m_totalLaps}";
                model.SessionDuration = $"# of Laps: {packet.m_totalLaps.ToString()}";
            }
        }

        private void UDPC_OnFinalClassificationDataReceive(PacketFinalClassificationData packet)
        {
            for (int i = 0; i < packet.m_classificationData.Length; i++)
            {
                var finalData = packet.m_classificationData[i];

                Driver[i].CarPosition = (byte)finalData.m_position;
                Driver[i].CurrentLapNum = (int)finalData.m_numLaps;
                Driver[i].BestLapTime = TimeSpan.FromMilliseconds(finalData.m_bestLapTimeInMS);
            }
        }

        private void UDPC_OnEventDataReceive(PacketEventData packet)
        {
            string s = new string(packet.m_eventStringCode);

            // EVENT STRING CODES
            switch (s)
            {
                case "FTLP":
                    model.EventStringCode = "New Fastest Lap";
                    Flap = TimeSpan.FromSeconds(packet.m_eventDetails.fastestLap.lapTime);
                    break;
                case "SPTP":
                    model.EventStringCode = "New Speed Trap Speed Set";
                    SpeedTrap = packet.m_eventDetails.speedTrap.speed;
                    break;
                case "SEND":
                    model.EventStringCode = "Session End";
                    model.SessionTimeRemaining = "Session Complete";
                    for (int i = 0; i < model.NumOfParticipants; i++)
                    {
                        if (Driver[i].CarPosition == 1)
                        {
                            model.SessionFastestLap = Driver[i].BestLapTime;
                        }
                        Driver[i].DriverStatus = "";
                        Driver[i].BestLapGap = model.SessionFastestLap - Driver[i].BestLapTime;
                    }
                    break;
                case "PENA":
                    model.EventStringCode = "Penalty Issued";
                    break;
            }
        }

        private void UDPC_OnLapDataReceive(PacketLapData packet)
        {
            for (int i = 0; i < packet.lapData.Length; i++)
            {
                var lapData = packet.lapData[i];

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
                        Driver[i].PositionChangeIcon = "/Core/Images/down_arrow.png";
                    }
                    else if(Driver[i].PositionChange < 0)
                    {
                        Driver[i].PositionChangeIcon = "/Core/Images/up_arrow.png";
                    }
                }

                // PENALTIES & WARNINGS
                Driver[i].Warnings = lapData.warnings;
                Driver[i].Penalties = lapData.penalties;

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

                if(Driver[i].S1Time != SectorMin2)
                {
                    if(Driver[i].S1Time < Driver[i].S1Time)
                    {
                        Driver[i].BestS1 = TimeSpan.FromMilliseconds(lapData.sector1TimeInMS);
                    }
                    if (Driver[i].S2Time < Driver[i].S2Time)
                    {
                        Driver[i].BestS2 = TimeSpan.FromMilliseconds(lapData.sector2TimeInMS);
                    }
                    if (Driver[i].S3Time < Driver[i].S3Time)
                    {
                        Driver[i].BestS3 = Driver[i].S3Time;
                    }
                }

                // DRIVER STATUS ICONS
                switch (Driver[i].DriverStatusUpdate)
                {
                    case DriverStatus.InGarage:
                        Driver[i].DriverStatusSource = "/Core/Images/Garage.png";
                        break;
                    case DriverStatus.OutLap:
                        Driver[i].DriverStatusSource = "/Core/Images/out-lap.png";
                        break;
                    case DriverStatus.InLap:
                        Driver[i].DriverStatusSource = "/Core/Images/in-lap.png";
                        break;
                    case DriverStatus.FlyingLap:
                        Driver[i].DriverStatusSource = "/Core/Images/fast.png";
                        break;
                }
            }
        }

        private void UDPC_OnParticipantDataReceive(PacketParticipantsData packet)
        {
            model.NumOfActiveCars = packet.m_numActiveCars.ToString();
            model.TotalParticipants = packet.m_participants.Length;

            for (int i = 0; i < packet.m_participants.Length; i++)
            {
                var participant = packet.m_participants[i];

                Driver[i].TeamID = participant.m_teamId;
                Driver[i].TeamName = Regex.Replace(participant.m_teamId.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
                Driver[i].raceNumber = participant.m_raceNumber;
                Driver[i].AI = participant.m_aiControlled;

                if(model.NetworkGame == "Offline")
                {
                    Driver[i].DriverName = Regex.Replace(participant.m_driverId.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
                }

                // TEAM COLORS
                switch (Driver[i].TeamID)
                {
                    case Teams.Mercedes:
                        Driver[i].TeamRect = "#00D2BE";
                        break;
                    case Teams.RedBullRacing:
                        Driver[i].TeamRect = "#0600EF";
                        break;
                    case Teams.Ferrari:
                        Driver[i].TeamRect = "#C00000";
                        break;
                    case Teams.Mclaren:
                        Driver[i].TeamRect = "#FF8700";
                        break;
                    case Teams.Haas:
                        Driver[i].TeamRect = "#FFFFFFFF";
                        break;
                    case Teams.Williams:
                        Driver[i].TeamRect = "#0082FA";
                        break;
                    case Teams.AlphaTauri:
                        Driver[i].TeamRect = "#C8C8C8";
                        break;
                    case Teams.Alpine:
                        Driver[i].TeamRect = "#FF00D1FF";
                        break;
                    case Teams.AlfaRomeo:
                        Driver[i].TeamRect = "#FF870000";
                        break;
                    case Teams.AstonMartin:
                        Driver[i].TeamRect = "#FF2F4F4F";
                        break;
                }
            }
        }
    }
}
