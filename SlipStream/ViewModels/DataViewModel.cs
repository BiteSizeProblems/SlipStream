using SlipStream.Core;
using SlipStream.Core.Utils;
using SlipStream.Models;
using SlipStream.Structs;
using SlipStream.Views;
using SlipStream.Views.Multi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
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
        }

        private void UDPC_OnSessionDataReceive(PacketSessionData packet)
        {
            model.Formula = Regex.Replace(packet.m_formula.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
            model.Circuit = Regex.Replace(packet.m_trackId.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
            model.Track = packet.m_trackId;
            model.SessionType = packet.m_sessionType;
            model.CurrentSession = Regex.Replace(packet.m_sessionType.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
            model.CurrentWeather = Regex.Replace(packet.m_weather.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
            model.SafetyCarStatus = Regex.Replace(packet.m_safetyCarStatus.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
            model.NetworkGame = packet.m_networkGame;

            // PITSTOP DATA
            for (int i = 0; i < model.NumOfActiveCars; i++)
            {
                Driver[i].PitWindowIdeal = packet.m_pitStopWindowIdealLap;
                Driver[i].PitWindowLate = packet.m_pitStopWindowLatestLap;
                Driver[i].PitRejoin = packet.m_pitStopRejoinPosition;
            }

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

                W_Model[i].SessionType = forecast.m_sessionType;
                W_Model[i].TimeOffset = forecast.m_timeOffset;
                W_Model[i].Weather = forecast.m_weather;
                W_Model[i].TrackTemperature = $"{(sbyte)((forecast.m_trackTemperature * 1.8) + 32)} F";
                W_Model[i].AirTemperature = $"{(sbyte)((forecast.m_airTemperature * 1.8) + 32)} F";
                W_Model[i].RainPercentage = $"{forecast.m_rainPercentage}%";
            }

            if (packet.m_sessionType != SessionTypes.RACE)
            {
                model.SessionTimeRemaining = $"Time Remaining: {TimeSpan.FromSeconds(packet.m_sessionTimeLeft)}";
                model.SessionDuration = TimeSpan.FromSeconds(packet.m_sessionDuration).ToString();
            }
            else
            {
                model.SessionTimeRemaining = $"{model.LeadLap} / {packet.m_totalLaps}";
                model.SessionDuration = packet.m_totalLaps.ToString();
            }

            Console.WriteLine("Here");
        }

        private void UDPC_OnParticipantDataReceive(PacketParticipantsData packet)
        {
            model.NumOfActiveCars = packet.m_numActiveCars;
            model.TotalParticipants = packet.m_participants.Length;

            for (int i = 0; i < packet.m_participants.Length; i++)
            {
                var participant = packet.m_participants[i];

                Driver[i].TeamID = participant.m_teamId;
                Driver[i].TeamName = Regex.Replace(participant.m_teamId.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
                Driver[i].raceNumber = participant.m_raceNumber;
                Driver[i].AI = participant.m_aiControlled;
                Driver[i].UDPSetting = participant.m_yourTelemetry;

                // OFFLINE DRIVER NAMES
                if (model.NetworkGame == NetworkTypes.Offline)
                {
                    Driver[i].DriverName = Regex.Replace(participant.m_driverId.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
                }

                // TEAM COLORS
                switch (Driver[i].TeamID)
                {
                    case Teams.Mercedes:
                        Driver[i].TeamColorIcon = "#00D2BE";
                        break;
                    case Teams.RedBullRacing:
                        Driver[i].TeamColorIcon = "#0600EF";
                        break;
                    case Teams.Ferrari:
                        Driver[i].TeamColorIcon = "#C00000";
                        break;
                    case Teams.Mclaren:
                        Driver[i].TeamColorIcon = "#FF8700";
                        break;
                    case Teams.Haas:
                        Driver[i].TeamColorIcon = "#FFFFFFFF";
                        break;
                    case Teams.Williams:
                        Driver[i].TeamColorIcon = "#0082FA";
                        break;
                    case Teams.AlphaTauri:
                        Driver[i].TeamColorIcon = "#C8C8C8";
                        break;
                    case Teams.Alpine:
                        Driver[i].TeamColorIcon = "#FF00D1FF";
                        break;
                    case Teams.AlfaRomeo:
                        Driver[i].TeamColorIcon = "#FF870000";
                        break;
                    case Teams.AstonMartin:
                        Driver[i].TeamColorIcon = "#FF2F4F4F";
                        break;
                }
            }
        }

        private void UDPC_OnLapDataReceive(PacketLapData packet)
        {
            // 22 cars
            int[] IndexToPositionArr = new int[22];
            // 21 deltas
            float[] DeltaArr = new float[21];

            LapDataUtils.UpdatePositionArray(packet.lapData, ref IndexToPositionArr);

            for (int i = 0; i < packet.lapData.Length; i++)
            {
                var lapData = packet.lapData[i];

                Driver[i].CurrentLapTime = TimeSpan.FromMilliseconds(lapData.currentLapTimeInMS);
                Driver[i].CarPosition = lapData.carPosition;
                Driver[i].GridPosition = lapData.gridPosition;
                Driver[i].LastLapTime = TimeSpan.FromMilliseconds(lapData.lastLapTimeInMS);
                Driver[i].CurrentLapNum = lapData.currentLapNum - 1;
                Driver[i].DriverStatus = lapData.driverStatus;
                Driver[i].ResultStatus = lapData.resultStatus;
                Driver[i].Warnings = lapData.warnings;
                Driver[i].Penalties = lapData.penalties;

                
                

                if(Driver[i].CarPosition == -10)
                {
                    LapData currCar = IndexingUtils.GetByRealPosition(packet.lapData, IndexToPositionArr, i + 1); // Car in Position

                    LapData nextCar = IndexingUtils.GetByRealPosition(packet.lapData, IndexToPositionArr, i + 2); // Position of Next Car

                    // If they are on the same lap, then we can directly do the delta calculation
                    if (nextCar.currentLapNum == currCar.currentLapNum)
                    {
                        float delta = currCar.currentLapTimeInMS - nextCar.currentLapTimeInMS; // ex 0.100

                        DeltaArr[IndexingUtils.GetRealIndex(IndexToPositionArr, i + 1)] = delta;

                        Driver[i].RaceInterval = TimeSpan.FromMilliseconds(DeltaArr[i+1]);
                    }
                    // If we are one lap ahead of the other car we must still compute delta
                    else if (currCar.currentLapNum - 1 == nextCar.currentLapNum)
                    {
                        var delta = currCar.currentLapTimeInMS - nextCar.currentLapTimeInMS + currCar.lastLapTimeInMS; // ex 0.100

                        DeltaArr[IndexingUtils.GetRealIndex(IndexToPositionArr, i + 1)] = delta;

                        Driver[i].RaceInterval = TimeSpan.FromMilliseconds(DeltaArr[i+1]);

                    }
                    //Other wise we can't compute a delta since they are +1 Lap (I think....?)
                    else
                    {
                        DeltaArr[IndexingUtils.GetRealIndex(IndexToPositionArr, i + 1)] = -1;

                        Driver[i].RaceInterval = TimeSpan.FromMilliseconds(DeltaArr[i+1]);
                    }
                }

                // POSITION CHANGE
                if (model.SessionType == SessionTypes.RACE | model.SessionType == SessionTypes.RaceTwo)
                {
                    Driver[i].PositionChange = (sbyte)(Driver[i].CarPosition - Driver[i].GridPosition);

                    if (Driver[i].PositionChange > 0)
                    {
                        Driver[i].PositionChangeIcon = "/Core/Images/Leaderboard/red_arrow.png";
                    }
                    else if (Driver[i].PositionChange < 0)
                    {
                        Driver[i].PositionChangeIcon = "/Core/Images/Leaderboard/green_arrow.png";
                    }
                    else { Driver[i].PositionChangeIcon = "/Core/Images/Leaderboard/dash.png"; }
                }

                // SESSION FASTEST LAPTIME / RACE LEAD LAP / LEAD LAP TIME / LEADER INTERVAL
                if (Driver[i].CarPosition == 1)
                {
                    model.SessionFastestLap = Driver[i].BestLapTime;

                    if(model.CurrentSession == "R A C E" | model.CurrentSession == "Race Two")
                    {
                        model.LeadLap = Driver[i].CurrentLapNum + 1;
                        model.LeadLapTime = Driver[i].CurrentLapTime;
                    }
                }
                else
                {
                    if (Driver[i].CurrentLapNum == model.LeadLap - 1)
                    {
                        if(Driver[i].CurrentLapTime < TimeSpan.FromSeconds(1))
                        {
                            Driver[i].RaceIntervalLeader = model.LeadLapTime - Driver[i].CurrentLapTime;
                        }
                    }
                }

                // INTERVAL
                //if (currCar.currentLapNum == nextCar.currentLapNum)
                {
                    //Driver[i].RaceInterval = TimeSpan.FromMilliseconds( nextCar.currentLapTimeInMS - currCar.currentLapTimeInMS );
                }

                // BEST LAPTIME DELTA
                if (Driver[i].CurrentLapNum > 0)
                {
                    Driver[i].BestLapDelta = Driver[i].BestLapTime - model.SessionFastestLap;
                }

                // SELECTED DELTA
                switch (model.CurrentSession)
                {
                    default:
                        Driver[i].SelectedDelta = Driver[i].BestLapDelta;
                        break;
                    case "R A C E":
                    case "Race Two":
                        Driver[i].SelectedDelta = Driver[i].RaceIntervalLeader;
                        break;
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

                // DRIVER STATUS
                if(model.SessionType != SessionTypes.RACE && model.SessionType != SessionTypes.RaceTwo)
                {
                    switch (Driver[i].DriverStatus)
                    {
                        case DriverStatus.InGarage:
                            Driver[i].DriverStatusSource = "/Core/Images/Lights/black_circle.png";
                            Driver[i].S1Display = Driver[i].BestS1;
                            Driver[i].S2Display = Driver[i].BestS2;
                            Driver[i].S3Display = Driver[i].BestS3;
                            break;
                        case DriverStatus.OutLap:
                            Driver[i].DriverStatusSource = "/Core/Images/Lights/yellow_circle.png";
                            Driver[i].S1Display = Driver[i].LastS1;
                            Driver[i].S2Display = Driver[i].LastS2;
                            Driver[i].S3Display = Driver[i].LastS3;
                            break;
                        case DriverStatus.InLap:
                            Driver[i].DriverStatusSource = "/Core/Images/Lights/red_circle.png";
                            Driver[i].S1Display = Driver[i].LastS1;
                            Driver[i].S2Display = Driver[i].LastS2;
                            Driver[i].S3Display = Driver[i].LastS3;
                            break;
                        case DriverStatus.FlyingLap:
                            Driver[i].DriverStatusSource = "/Core/Images/Lights/Green_circle.png";
                            Driver[i].S1Display = Driver[i].LastS1;
                            Driver[i].S2Display = Driver[i].LastS2;
                            Driver[i].S3Display = Driver[i].LastS3;
                            break;
                    }
                }
                else // IF RACING
                {
                    if (Driver[i].PitStatus != PitStatus.Pitting && Driver[i].PitStatus != PitStatus.InPitArea)
                    {
                        Driver[i].ActualDriverStatus = Regex.Replace(Driver[i].ResultStatus.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();

                        switch (Driver[i].ResultStatus)
                        {
                            default:
                                Driver[i].DriverStatusSource = "/Core/Images/Lights/black_circle.png";
                                Driver[i].S1Display = TimeSpan.FromSeconds(0);
                                Driver[i].S2Display = TimeSpan.FromSeconds(0);
                                Driver[i].S3Display = TimeSpan.FromSeconds(0);
                                break;
                            case ResultStatus.Active:
                                Driver[i].DriverStatusSource = "/Core/Images/Lights/green_circle.png";
                                Driver[i].S1Display = Driver[i].BestS1;
                                Driver[i].S2Display = Driver[i].BestS2;
                                Driver[i].S3Display = Driver[i].BestS3;
                                break;
                            case ResultStatus.Finished:
                                Driver[i].DriverStatusSource = "/Core/Images/Lights/white_circle.png";
                                Driver[i].S1Display = TimeSpan.FromSeconds(0);
                                Driver[i].S2Display = TimeSpan.FromSeconds(0);
                                Driver[i].S3Display = TimeSpan.FromSeconds(0);
                                break;
                            case ResultStatus.Disqualified:
                                Driver[i].DriverStatusSource = "/Core/Images/Lights/red_circle.png";
                                Driver[i].S1Display = TimeSpan.FromSeconds(0);
                                Driver[i].S2Display = TimeSpan.FromSeconds(0);
                                Driver[i].S3Display = TimeSpan.FromSeconds(0);
                                break;
                        }
                    }
                    else
                    {
                        Driver[i].ActualDriverStatus = Regex.Replace(Driver[i].PitStatus.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
                        Driver[i].DriverStatusSource = "/Core/Images/Lights/yellow_circle.png";
                    }
                }

                model.AverageSoftTime = TimeSpan.FromMilliseconds(CarStatusUtils.GetAverageTireLaptime(packet.lapData));
            }
        }

        private void UDPC_OnCarStatusDataReceive(PacketCarStatusData packet)
        {
            for (int i = 0; i < packet.m_carStatusData.Length; i++)
            {
                var carStatusData = packet.m_carStatusData[i];

                Driver[i].VisualTireCompound = carStatusData.m_visualTyreCompound;
                Driver[i].TireAge = carStatusData.m_tyresAgeLaps;
                Driver[i].VehicleFlag = $"Zone Flags: {carStatusData.m_vehicleFiaFlags}";
                Driver[i].ErsRemaining = ((int)(carStatusData.m_ersStoreEnergy / 40000));
                Driver[i].ErsDeployMode = carStatusData.m_ersDeployMode;
                Driver[i].FuelMix = carStatusData.m_fuelMix;

                model.NumSoftTires = CarStatusUtils.GetActiveTireCount(packet.m_carStatusData, VisualTireCompounds.Soft);
                model.NumMediumTires = CarStatusUtils.GetActiveTireCount(packet.m_carStatusData, VisualTireCompounds.Medium);
                model.NumHardTires = CarStatusUtils.GetActiveTireCount(packet.m_carStatusData, VisualTireCompounds.Hard);
                model.NumInterTires = CarStatusUtils.GetActiveTireCount(packet.m_carStatusData, VisualTireCompounds.Inter);
                model.NumWetTires = CarStatusUtils.GetActiveTireCount(packet.m_carStatusData, VisualTireCompounds.Wet);

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
                Driver[i].FinalRaceTime = TimeSpan.FromSeconds(finalData.m_totalRaceTime);
                Driver[i].FinalPenaltiesNum = finalData.m_numPenalties;
                Driver[i].FinalPenaltiesTime = TimeSpan.FromSeconds(finalData.m_penaltiesTime);
                Driver[i].FinalTireStintsNum = finalData.m_numTyreStints;
                Driver[i].LastLapTime = Driver[i].TotalRaceTime;
            }
        }
    }
}
