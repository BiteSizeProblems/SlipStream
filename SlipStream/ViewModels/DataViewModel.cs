using SlipStream.Core.Utils;
using SlipStream.Models;
using SlipStream.Structs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Threading;
using static SlipStream.Structs.Appendeces;
using System.Diagnostics;

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
        public SessionHistoryModel histModel { get; set; }

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

        private Timer slowTimer;
        public PacketLapData latestLapDataPacket { get; set; }

        // Create observable collections of models
        public ObservableCollection<DriverModel> Driver { get; set; }
        private object _driverLock = new object();
        public ObservableCollection<WeatherModel> W_Model { get; set; }
        private object _w_modelLock = new object();
        public ObservableCollection<SessionHistoryModel> HistModel { get; set; }
        private object _histLock = new object();

        private DataViewModel() : base()
        {
            this.model = new SessionModel();

            // SET NEW OBSERVABLE COLLECTIONS
            Driver = new ObservableCollection<DriverModel>();
            BindingOperations.EnableCollectionSynchronization(Driver, _driverLock);
            W_Model = new ObservableCollection<WeatherModel>();
            BindingOperations.EnableCollectionSynchronization(W_Model, _w_modelLock);
            HistModel = new ObservableCollection<SessionHistoryModel>();
            BindingOperations.EnableCollectionSynchronization(HistModel, _histLock);

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
            for (int i = 0; i < 100; i++)
            {
                HistModel.Add(new SessionHistoryModel());
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
            UDPC.OnSessionHistoryDataReceive += UDPC_OnSessionHistoryDataReceive;
            UDPC.OnFinalClassificationDataReceive += UDPC_OnFinalClassificationDataReceive;

            UDPC.OnLapDataReceive += updater;

            slowTimer = new Timer(UpdateValuesSlow, null, 0, 1000);
        }

        private void UDPC_OnSessionHistoryDataReceive(PacketSessionHistoryData packet)
        {
            int DriverIndex = (int)packet.m_carIdx;

            for (int i = 0; i < model.NumOfActiveCars; i++)
            {
                Driver[DriverIndex].BestLapNum = packet.m_bestLapTimeLapNum;
                Driver[DriverIndex].BestS1Num = packet.m_bestSector1LapNum;
                Driver[DriverIndex].BestS2Num = packet.m_bestSector2LapNum;
                Driver[DriverIndex].BestS3Num = packet.m_bestSector3LapNum;
                Driver[DriverIndex].NumTireStints = packet.m_numTyreStints;
            }

            // Lap History Data
            for (int i = 0; i < packet.m_lapHistoryData.Length; i++) // For each lap
            {
                var lapHistoryData = packet.m_lapHistoryData[i];

                HistModel[i].LapTime = TimeSpan.FromMilliseconds(lapHistoryData.m_lapTimeInMS);
                HistModel[i].Sector1Time = lapHistoryData.m_sector1TimeInMS;
                HistModel[i].Sector2Time = lapHistoryData.m_sector2TimeInMS;
                HistModel[i].Sector3Time = lapHistoryData.m_sector3TimeInMS;

                //Driver[CarIdx].CarIdx = packet.m_carIdx; // This style is used to calculate the rankings.
                //Driver[0].CarIdx = packet.m_carIdx;
            }

            // Tire History Data
            for (int i = 0; i < packet.m_tyreStintsHistoryData.Length; i++) // For each tire stint
            {
                var tireHistoryData = packet.m_tyreStintsHistoryData[i];

                HistModel[i].EndLap = tireHistoryData.m_endLap;
                HistModel[i].TireActual = tireHistoryData.m_tyreActualCompound;
                HistModel[i].TireVisual = tireHistoryData.m_tyreVisualCompound;
            }
        }

        private void updater(PacketLapData packet)
        {
            latestLapDataPacket = packet;
        }

        private void UpdateValuesSlow(object state = null)
        {
            // Say we wanted to order the drivers in order of m_lastLapTimeInMS to see who is currently lapping the quickest
            PacketLapData lapData = latestLapDataPacket;

            if(lapData.lapData != null)
            {
                var lastLapTimeLookupArray = lapData.lapData
                    .Where(lapData => lapData.lastLapTimeInMS != 0) // Filter out drivers who do not have a time set.
                                .Select((data, index) => new { data, index }) // Create a object that looks like {LapData data, int index}
                                .OrderBy(value => value.data.lastLapTimeInMS) // Order it by the m_lastLapTimeInMS property
                                .Select(value => value.index) // Take only the index property from the sorted array
                                .ToArray(); // Convert the array;

                for (int i = 0; i < lastLapTimeLookupArray.Length; i++)
                {
                    var originalDriverIndex = lastLapTimeLookupArray[i];
                    //Debug.WriteLine(lastLapTimeLookupArray[i]); // Returns original index

                    var lastLapDriverIndex = i + 1;
                    //var lastLapDriverIndex = lapData.lapData[lastLapTimeLookupArray[i]].carPosition;
                    //Debug.WriteLine(lapData.lapData[lastLapTimeLookupArray[i]].carPosition); // Returns Array of car positions sorted by Last Lap Time.

                    Driver[originalDriverIndex].LastLapRank = lastLapDriverIndex;
                }
            }

            if (lapData.lapData != null)
            {
                var lastS1TimeLookupArray = lapData.lapData
                    .Where(lapData => lapData.sector1TimeInMS != 0) // Filter out drivers who do not have a time set.
                                .Select((data, index) => new { data, index }) // Create a object that looks like {LapData data, int index}
                                .OrderBy(value => value.data.sector1TimeInMS) // Order it by the m_lastLapTimeInMS property
                                .Select(value => value.index) // Take only the index property from the sorted array
                                .ToArray(); // Convert the array;

                for (int i = 0; i < lastS1TimeLookupArray.Length; i++)
                {
                    var originalDriverIndex = lastS1TimeLookupArray[i];
                    var lastLapDriverIndex = i + 1;

                    Driver[originalDriverIndex].LastS1Rank = lastLapDriverIndex;
                }
            }

            if (lapData.lapData != null)
            {
                var lastS2TimeLookupArray = lapData.lapData.Where(lapData => lapData.sector2TimeInMS != 0).Select((data, index) => new { data, index })
                    .OrderBy(value => value.data.sector2TimeInMS).Select(value => value.index).ToArray();

                for (int i = 0; i < lastS2TimeLookupArray.Length; i++)
                {
                    var originalDriverIndex = lastS2TimeLookupArray[i];
                    var lastLapDriverIndex = i + 1;

                    Driver[originalDriverIndex].LastS2Rank = lastLapDriverIndex;
                }
            }

            if (driver != null)
            {
                var lastS3TimeLookupArray = Driver.Where(driver => driver.LastS3 != TimeSpan.Zero).Select((data, index) => new { data, index })
                    .OrderBy(value => value.data.LastS3).Select(value => value.index).ToArray();

                for (int i = 0; i < lastS3TimeLookupArray.Length; i++)
                {
                    var originalDriverIndex = lastS3TimeLookupArray[i];
                    var lastLapDriverIndex = i + 1;

                    //Driver[originalDriverIndex].LastS3Rank = lastLapDriverIndex;
                }
            }

            // Gap to next car.
            if (lapData.lapData != null)
            {
                int[] IndexToPositionArr = new int[22]; // 22 cars.

                LapDataUtils.UpdatePositionArray(lapData.lapData, ref IndexToPositionArr); // Sort drivers by position.
                                                                                           // Set the delta for the first person to zero
                Driver[IndexingUtils.GetRealIndex(IndexToPositionArr, 1)].RaceInterval = TimeSpan.FromMilliseconds(0);

                for (int i = 0; i < model.NumOfActiveCars-1; i++)
                {
                    var currCar = IndexingUtils.GetByRealPosition(lapData.lapData, IndexToPositionArr, i + 1); // 0 + 1 = Position 1 // Car Ahead
                    var carBehind = IndexingUtils.GetByRealPosition(lapData.lapData, IndexToPositionArr, i + 2); // 0 + 2 == Position 2 // Car Behind

                    uint delta = currCar.currentLapTimeInMS - carBehind.currentLapTimeInMS;

                    Driver[IndexingUtils.GetRealIndex(IndexToPositionArr, i + 2)].RaceInterval = TimeSpan.FromMilliseconds(delta);
                }
            }

        }

        private void UDPC_OnSessionDataReceive(PacketSessionData packet)
        {
            model.Formula = packet.m_formula;
            model.Circuit = packet.m_trackId;
            model.CurrentSession = packet.m_sessionType;
            model.CurrentWeather = packet.m_weather;
            model.SafetyCarStatus = packet.m_safetyCarStatus;
            model.Track = packet.m_trackId;
            model.NetworkGame = packet.m_networkGame;
            model.PitWindowIdeal = packet.m_pitStopWindowIdealLap;
            model.PitWindowLate = packet.m_pitStopWindowLatestLap;
            model.PitRejoin = packet.m_pitStopRejoinPosition;
            model.SessionDuration = TimeSpan.FromSeconds(packet.m_sessionDuration);
            model.SessionTimeRemaining = TimeSpan.FromSeconds(packet.m_sessionTimeLeft);
            model.TotalLaps = packet.m_totalLaps;

            model.RaceLapCount = $"{model.LeadLap} / {packet.m_totalLaps}";

            if (model.CurrentSession == SessionTypes.RACE | model.CurrentSession == SessionTypes.RACE_TWO)
            {
                model.LeftInSession = model.RaceLapCount;
            }
            else
            {
                model.LeftInSession = model.SessionTimeRemaining.ToString();
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
            }
        }

        private void UDPC_OnLapDataReceive(PacketLapData packet)
        {
            int[] IndexToPositionArr = new int[22]; // 22 cars.
            float[] DeltaArr = new float[22]; // 21 deltas.
            LapDataUtils.UpdatePositionArray(packet.lapData, ref IndexToPositionArr); // Sort drivers by position.

            for (int i = 0; i < 22; i++)
            {
                var lapData = packet.lapData[i];

                Driver[i].CarPosition = lapData.carPosition;
                Driver[i].GridPosition = lapData.gridPosition;

                Driver[i].CurrentLapNum = lapData.currentLapNum;
                Driver[i].CurrentLapTime = TimeSpan.FromMilliseconds(lapData.currentLapTimeInMS);
                Driver[i].CurrentLapTimeFloat = lapData.currentLapTimeInMS;
                Driver[i].LastLapTime = TimeSpan.FromMilliseconds(lapData.lastLapTimeInMS);
                Driver[i].LastS1 = TimeSpan.FromMilliseconds(lapData.sector1TimeInMS);
                Driver[i].LastS2 = TimeSpan.FromMilliseconds(lapData.sector2TimeInMS);

                Driver[i].Warnings = lapData.warnings;
                Driver[i].Penalties = lapData.penalties;

                Driver[i].DriverStatus = lapData.driverStatus;
                Driver[i].ResultStatus = lapData.resultStatus;
                Driver[i].PitStatus = lapData.pitStatus;

                // Calculate average lap times by tire.
                if (Driver[i].ResultStatus == ResultStatus.Active)
                {
                    if (Driver[i].VisualTireCompound == VisualTireCompounds.Soft && Driver[i].ResultStatus == ResultStatus.Active)
                    {
                        var laptimes = new float[22];

                        laptimes[i] = lapData.lastLapTimeInMS;

                        model.AverageSoftTime = TimeSpan.FromMilliseconds(laptimes.Sum());
                    }
                    else if (Driver[i].VisualTireCompound == VisualTireCompounds.Medium)
                    {
                        var laptimes = new float[22];

                        laptimes[i] = lapData.lastLapTimeInMS;

                        model.AverageMediumTime = TimeSpan.FromMilliseconds(laptimes.Sum());
                    }
                }

                // Set Actual Driver Status
                if (Driver[i].PitStatus == PitStatus.None | Driver[i].PitStatus == PitStatus.Unknown)
                {
                    switch (model.CurrentSession)
                    {
                        case SessionTypes.P1:
                            Driver[i].ActualDriverStatus = Driver[i].DriverStatus.ToString();
                            break;
                        case SessionTypes.RACE:
                        case SessionTypes.RACE_TWO:
                            Driver[i].ActualDriverStatus = Driver[i].ResultStatus.ToString();
                            break;
                    }
                }
                else
                {
                    Driver[i].ActualDriverStatus = Driver[i].PitStatus.ToString();
                }

                // Set Leaderboard Light Descriptions.
                if (model.CurrentSession == SessionTypes.RACE | model.CurrentSession == SessionTypes.RACE_TWO)
                {
                    model.LightOne = "Retired"; //black
                    model.LightTwo = "In-Pitlane"; //blue
                    model.LightThree = "Active"; //green
                    model.LightFour = "N/A"; //orange
                    model.LightFive = "DSQ"; //red
                    model.LightSix = "Finished"; //white
                    model.LightSeven = "In-This-Lap"; //yellow
                }
                else
                {
                    model.LightOne = "Inactive";
                    model.LightTwo = "N/A";
                    model.LightThree = "Flying Lap";
                    model.LightFour = "In-Lap";
                    model.LightFive = "DSQ";
                    model.LightSix = "In-Garage";
                    model.LightSeven = "Out-Lap";
                }

                // BEST S1
                if (Driver[i].BestS1 == TimeSpan.Zero | Driver[i].LastS1 < Driver[i].BestS1)
                {
                    Driver[i].BestS1 = Driver[i].LastS1;
                }

                // BEST S2
                if (Driver[i].BestS2 == TimeSpan.Zero | Driver[i].LastS2 < Driver[i].BestS2)
                {
                    Driver[i].BestS2 = Driver[i].LastS2;
                }

                // Set driver's delta to fastest lap of the session.
                if (Driver[i].BestLapTime != TimeSpan.Zero)
                {
                    Driver[i].BestLapDelta = model.SessionFastestLap - Driver[i].BestLapTime;
                }

                // Set lead lap, and current/fastest laptime.
                if (model.CurrentSession == SessionTypes.RACE | model.CurrentSession == SessionTypes.RACE_TWO)
                {
                    Driver[i].PositionChange = (sbyte)(Driver[i].GridPosition - Driver[i].CarPosition); // Calculate position change from starting position.

                    if (Driver[i].CarPosition == 1)
                    {
                        model.SessionFastestLap = Driver[i].BestLapTime; // Fastest lap of the session.
                        model.LeadLap = Driver[i].CurrentLapNum + 1; // Lead lap of the race.
                        model.LeadLapTime = Driver[i].CurrentLapTime; // Current lap time for lead driver.
                    }
                    else if (Driver[i].CurrentLapNum == model.LeadLap - 1 && Driver[i].CurrentLapTime < TimeSpan.FromSeconds(1))
                    {
                        Driver[i].RaceIntervalLeader = model.LeadLapTime - Driver[i].CurrentLapTime; // If car is not leading, set interval to leader.
                    }
                }

                // Set which delta to show on the leaderboard.
                switch (model.CurrentSession)
                {
                    default:
                        Driver[i].SelectedDelta = Driver[i].BestLapDelta;
                        break;
                    case SessionTypes.RACE:
                    case SessionTypes.RACE_TWO:
                        //Driver[i].SelectedDelta = Driver[i].RaceIntervalLeader; // Interval to Leader.
                        Driver[i].SelectedDelta = Driver[i].RaceInterval; // Interval to Next Car.
                        break;
                }

                // Which sector time to display? <---------- CAN THIS BE IN A UTIL?
                switch (Driver[i].ActualDriverStatus)
                {
                    case "In_Garage":
                    case "Finished":
                        Driver[i].S1Display = Driver[i].BestS1;
                        Driver[i].S2Display = Driver[i].BestS2;
                        Driver[i].S3Display = Driver[i].BestS3;
                        break;
                    case "Out_Lap":
                    case "In_Lap":
                    case "Flying_Lap":
                    case "Active":
                    case "In_This_Lap":
                    case "In_Pit_Lane":
                        Driver[i].S1Display = Driver[i].LastS1;
                        Driver[i].S2Display = Driver[i].LastS2;
                        Driver[i].S3Display = Driver[i].LastS3;
                        break;
                    default:
                        Driver[i].S1Display = TimeSpan.Zero;
                        Driver[i].S2Display = TimeSpan.Zero;
                        Driver[i].S3Display = TimeSpan.Zero;
                        break;
                }
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
                }
                Driver[i].TireWear = ((Driver[i].RLTireWear + Driver[i].RRTireWear + Driver[i].FLTireWear + Driver[i].FRTireWear) / 4);
            }
        }

        private void UDPC_OnEventDataReceive(PacketEventData packet) // CREATE UTIL <------------------------------------------------------------------- !!
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
                    model.LeftInSession = "Session Complete";
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

        private class Dictionary<T>
        {
        }
    }
}