﻿using SlipStream.Core;
using SlipStream.Models;
using SlipStream.Structs;
using SlipStream.Views;
using SlipStream.Views.Multi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;
using static SlipStream.Structs.Appendeces;

namespace SlipStream.ViewModels
{
    public class DataViewModel : BaseViewModel
    {
        // === BEGINING OF MODULE SETUP ===
        // === Singleton Instance with Thread Saftey ===
        private static DataViewModel _instance = null;
        private static object _singletonLock = new object();
        public static DataViewModel GetInstance()
        {
            lock (_singletonLock)
            {
                if (_instance == null) { _instance = new DataViewModel(); }
                return _instance;
            }
        }
        // === END OF MODULE SETUP ===

        public SessionModel model { get; set; }
        public DriverModel driver { get; set; }
        public WeatherModel w_model { get; set; }

        // VIEW COMMANDS
        public RelayCommand LeaderboardViewCommand { get; set; }
        public LeaderboardView LV { get; set; }

        // VIEW MODEL VARIABLES
        private int _numActiveCars;
        public int NumActiveCars
        {
            get { return _numActiveCars; }
            set { SetField(ref _numActiveCars, value, nameof(NumActiveCars)); }
        }

        // DRIVER INDEXING
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

        // Create observable collections of models
        public ObservableCollection<DriverModel> Driver { get; set; }
        private object _driverLock = new object();
        public ObservableCollection<WeatherModel> W_Model { get; set; }
        private object _w_modelLock = new object();

        private DataViewModel() : base()
        {
            this.model = new SessionModel();

            // SET NEW OBSERVABLE COLLECTIONS
            Driver = new ObservableCollection<DriverModel>();
            BindingOperations.EnableCollectionSynchronization(Driver, _driverLock);
            W_Model = new ObservableCollection<WeatherModel>();
            BindingOperations.EnableCollectionSynchronization(W_Model, _w_modelLock);

            // SET DEFAULT DRIVERS & WEATHER FORECASTS
            for (int i = 0; i < 22; i++)
            {
                Driver.Add(new DriverModel());
                Driver[i].DriverName = "Placeholder";
                Driver[i].DriverIndex = i + 1;
            }
            for (int i = 0; i < 56; i++)
            {
                W_Model.Add(new WeatherModel());
            }

            // SET INITIAL INDEX
            SelectedIndex = 0;

            // SET DATA RECEIVE ACTIONS
            UDPC.OnSessionDataReceive += UDPC_OnSessionDataReceive;
            UDPC.OnParticipantsDataReceive += UDPC_OnParticipantDataReceive;

            UDPC.OnLapDataReceive += UDPC_OnLapDataReceive;
            UDPC.OnCarStatusDataReceive += UDPC_OnCarStatusDataReceive;
            UDPC.OnCarDamageDataReceive += UDPC_OnCarDamageDataReceive;
            UDPC.OnEventDataReceive += UDPC_OnEventDataReceive;

            UDPC.OnFinalClassificationDataReceive += UDPC_OnFinalClassificationDataReceive;
            UDPC.OnSessionHistoryDataReceive += UDPC_OnSessionHistoryDataReceive;
        }

        private void UDPC_OnSessionDataReceive(PacketSessionData packet)
        {
            model.Formula = Regex.Replace(packet.m_formula.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
            model.Circuit = Regex.Replace(packet.m_trackId.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
            model.CurrentSession = Regex.Replace(packet.m_sessionType.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
            model.CurrentWeather = Regex.Replace(packet.m_weather.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
            model.SafetyCarStatus = Regex.Replace(packet.m_safetyCarStatus.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();

            // PITSTOP DATA
            for (int i = 0; i < NumActiveCars; i++)
            {
                Driver[i].PitWindowIdeal = packet.m_pitStopWindowIdealLap;
                Driver[i].PitWindowLate = packet.m_pitStopWindowLatestLap;
                Driver[i].PitRejoin = packet.m_pitStopRejoinPosition;
            }

            // NETWORK STATUS

            if (packet.m_networkGame == 0)
            {
                model.NetworkGame = "Offline";
            }
            else { model.NetworkGame = "Online"; }

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

            if (packet.m_sessionType != SessionTypes.Race)
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

                // OFFLINE DRIVER NAMES
                if (model.NetworkGame == "Offline")
                {
                    Driver[i].DriverName = Regex.Replace(participant.m_driverId.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
                }
                else { Driver[i].DriverName = "Placeholder"; }

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

        private void UDPC_OnLapDataReceive(PacketLapData packet)
        {
            for (int i = 0; i < packet.lapData.Length; i++)
            {
                var lapData = packet.lapData[i];

                Driver[i].CurrentLapTime = TimeSpan.FromMilliseconds(lapData.currentLapTimeInMS);
                Driver[i].CarPosition = lapData.carPosition;
                Driver[i].GridPosition = lapData.gridPosition;
                Driver[i].LastLapTime = TimeSpan.FromMilliseconds(lapData.lastLapTimeInMS);
                Driver[i].CurrentLapNum = lapData.currentLapNum - 1;
                Driver[i].DriverStatus = Regex.Replace(lapData.driverStatus.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
                Driver[i].DriverStatusUpdate = lapData.driverStatus;

                // POSITION CHANGE
                if (model.CurrentSession == "Race")
                {
                    Driver[i].PositionChange = (sbyte)(Driver[i].CarPosition - Driver[i].GridPosition);

                    if (Driver[i].PositionChange > 0)
                    {
                        Driver[i].PositionChangeIcon = "/Core/Images/down_arrow.png";
                    }
                    else if (Driver[i].PositionChange < 0)
                    {
                        Driver[i].PositionChangeIcon = "/Core/Images/up_arrow.png";
                    }
                }

                // PENALTIES & WARNINGS
                Driver[i].Warnings = lapData.warnings;
                Driver[i].Penalties = lapData.penalties;

                // BEST LAPTIME DELTA
                if (Driver[i].CurrentLapNum > 0)
                {
                    Driver[i].BestLapDelta = Driver[i].BestLapTime - model.SessionFastestLap;
                }

                // INTERVAL
                if (Driver[i].CurrentLapNum == Driver[i-1].CurrentLapNum)
                {
                    Driver[i].RaceInterval = Driver[i-1].CurrentLapTime;
                }

                // LEADER BENCHMARK

                if (Driver[i].CarPosition == 1)
                {
                    model.LeadLapTime = Driver[i].CurrentLapTime;
                }

                // INTERVAL TO LEADER

                if (Driver[i].CurrentLapNum == Driver[i - 1].CurrentLapNum)
                {
                    Driver[i].RaceIntervalLeader = model.LeadLapTime - Driver[i].CurrentLapTime;
                }
                else
                {
                    Driver[i].RaceIntervalLeader = TimeSpan.FromSeconds(1.5);
                    // Driver[i].RaceIntervalLeader = model.LeadLap = Driver[i].CurrentLapNum;
                }
                    

                // SESSION FASTEST LAPTIME & LEAD LAP NUM
                if (Driver[i].CarPosition == 1)
                {
                    model.SessionFastestLap = Driver[i].BestLapTime;
                    model.LeadLap = Driver[i].CurrentLapNum + 1;
                }

                // LAST SECTOR TIMES
                Driver[i].LastS1 = TimeSpan.FromMilliseconds(lapData.sector1TimeInMS);
                Driver[i].LastS2 = TimeSpan.FromMilliseconds(lapData.sector2TimeInMS);

                // LAST S3 <- CALCULATED IN DRIVER MODEL

                // BEST S1
                if (Driver[i].LastS1 != TimeSpan.FromMilliseconds(0))
                {
                    if (Driver[i].BestS1 == TimeSpan.FromMilliseconds(0))
                    {
                        Driver[i].BestS1 = Driver[i].LastS1;
                    }
                    else if (Driver[i].LastS1 < Driver[i].BestS1)
                    {
                        Driver[i].BestS1 = Driver[i].LastS1;
                    }
                }

                // BEST S2
                if (Driver[i].LastS2 != TimeSpan.FromMilliseconds(0))
                {
                    if (Driver[i].BestS2 == TimeSpan.FromMilliseconds(0))
                    {
                        Driver[i].BestS2 = Driver[i].LastS2;
                    }
                    else if (Driver[i].LastS2 < Driver[i].BestS2)
                    {
                        Driver[i].BestS2 = Driver[i].LastS2;
                    }
                }

                // BEST S3 <- CALCULATED IN DRIVER MODEL

                // DRIVER STATUS ICONS
                switch (Driver[i].DriverStatusUpdate)
                {
                    case DriverStatus.InGarage:
                        Driver[i].DriverStatusSource = "/Core/Images/black_circle.png";
                        Driver[i].S1Display = Driver[i].BestS1;
                        Driver[i].S2Display = Driver[i].BestS2;
                        Driver[i].S3Display = Driver[i].BestS3;
                        break;
                    case DriverStatus.OutLap:
                        Driver[i].DriverStatusSource = "/Core/Images/yellow_circle.png";
                        Driver[i].S1Display = Driver[i].LastS1;
                        Driver[i].S2Display = Driver[i].LastS2;
                        Driver[i].S3Display = Driver[i].LastS3;
                        break;
                    case DriverStatus.InLap:
                        Driver[i].DriverStatusSource = "/Core/Images/red_circle.png";
                        Driver[i].S1Display = Driver[i].LastS1;
                        Driver[i].S2Display = Driver[i].LastS2;
                        Driver[i].S3Display = Driver[i].LastS3;
                        break;
                    case DriverStatus.FlyingLap:
                        Driver[i].DriverStatusSource = "/Core/Images/Green_circle.png";
                        Driver[i].S1Display = Driver[i].LastS1;
                        Driver[i].S2Display = Driver[i].LastS2;
                        Driver[i].S3Display = Driver[i].LastS3;
                        break;
                }
            }
        }

        private void UDPC_OnCarStatusDataReceive(PacketCarStatusData packet)
        {
            for (int i = 0; i < packet.m_carStatusData.Length; i++)
            {
                var carStatusData = packet.m_carStatusData[i];

                Driver[i].VisualTireCompound = (VisualTireCompounds)carStatusData.m_visualTyreCompound;
                Driver[i].TireAge = carStatusData.m_tyresAgeLaps;
                Driver[i].VehicleFlag = $"Zone Flags: {carStatusData.m_vehicleFiaFlags}";
                Driver[i].ErsRemaining = ((int)(carStatusData.m_ersStoreEnergy / 40000));
                Driver[i].ErsDeployMode = (Enums.ErsDeployMode)carStatusData.m_ersDeployMode;

                switch (carStatusData.m_fuelMix)
                {
                    case 0:
                        Driver[i].FuelMix = $"Lean";
                        break;
                    case 1:
                        Driver[i].FuelMix = $"Standard";
                        break;
                    case 2:
                        Driver[i].FuelMix = $"Rich";
                        break;
                    case 3:
                        Driver[i].FuelMix = $"Max";
                        break;
                }

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

        private void UDPC_OnCarDamageDataReceive(PacketCarDamageData packet)
        {
            for (int i = 0; i < 22; i++)
            {
                var carDamageData = packet.m_carDamageData[i];

                for (int a = 0; a < carDamageData.m_tyresWear.Length; a++)
                {
                    Driver[i].RLTireWear = carDamageData.m_tyresWear[a];
                    Driver[i].RRTireWear = carDamageData.m_tyresWear[a];
                    Driver[i].FLTireWear = carDamageData.m_tyresWear[a];
                    Driver[i].FRTireWear = carDamageData.m_tyresWear[a];

                    // TIRE WEAR COLOR
                    switch (Driver[i].RLTireWear)
                    {
                        case < 25f:
                            Driver[i].RLTireWearColor = "#FF2EB521";
                            break;
                        case < 50f:
                            Driver[i].RLTireWearColor = "#FFDDD803";
                            break;
                        case > 50f:
                            Driver[i].RLTireWearColor = "#FFB10000";
                            break;
                    }
                    // TIRE WEAR COLOR
                    switch (Driver[i].RRTireWear)
                    {
                        case < 25f:
                            Driver[i].RRTireWearColor = "#FF2EB521";
                            break;
                        case < 50f:
                            Driver[i].RRTireWearColor = "#FFDDD803";
                            break;
                        case > 50f:
                            Driver[i].RRTireWearColor = "#FFB10000";
                            break;
                    }
                    // TIRE WEAR COLOR
                    switch (Driver[i].FLTireWear)
                    {
                        case < 25f:
                            Driver[i].FLTireWearColor = "#FF2EB521";
                            break;
                        case < 50f:
                            Driver[i].FLTireWearColor = "#FFDDD803";
                            break;
                        case > 50f:
                            Driver[i].FLTireWearColor = "#FFB10000";
                            break;
                    }
                    // TIRE WEAR COLOR
                    switch (Driver[i].FRTireWear)
                    {
                        case < 25f:
                            Driver[i].FRTireWearColor = "#FF2EB521";
                            break;
                        case < 50f:
                            Driver[i].FRTireWearColor = "#FFDDD803";
                            break;
                        case > 50f:
                            Driver[i].FRTireWearColor = "#FFB10000";
                            break;
                    }
                }
                Driver[i].TireWear = ((Driver[i].RLTireWear + Driver[i].RRTireWear + Driver[i].FLTireWear + Driver[i].FRTireWear) / 4);

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
                    break;
                case "SPTP":
                    model.EventStringCode = "New Speed Trap Speed Set";
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
                        Driver[i].BestLapDelta = model.SessionFastestLap - Driver[i].BestLapTime;
                    }
                    break;
                case "PENA":
                    model.EventStringCode = "Penalty Issued";
                    break;
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

        private void UDPC_OnSessionHistoryDataReceive(PacketSessionHistoryData packet)
        {
            for (int i = 0; i < packet.m_lapHistoryData.Length; i++)
            {
                var lapHistory = packet.m_lapHistoryData[i];
                var tireHistory = packet.m_tyreStintsHistoryData[i];

                Driver[i].lapNum = Driver[i].lapNum + 1;
                Driver[i].CarIdx = packet.m_carIdx;
                Driver[i].LapTimeHist = TimeSpan.FromMilliseconds(lapHistory.m_lapTimeInMS);
            }
        }
    }
}