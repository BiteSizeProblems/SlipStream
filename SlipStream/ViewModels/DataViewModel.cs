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
        // Module Set-Up (Singleton Instance with thread safety)
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

        private Timer averagesTimer;
        private Timer pitstopStrategyTimer;

        private Timer slowTimer;
        private Timer mediumTimer;
        private Timer fastTimer;

        public PacketSessionData latestSessionDataPacket { get; set; }
        public PacketLapData latestLapDataPacket { get; set; }
        public PacketCarStatusData latestCarStatusPacket { get; set; }
        public PacketCarDamageData latestCarDamagePacket { get; set; }

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
            UDPC.OnSessionHistoryDataReceive += UDPC_OnSessionHistoryDataReceive;
            UDPC.OnFinalClassificationDataReceive += UDPC_OnFinalClassificationDataReceive;

            UDPC.OnSessionDataReceive += StoreSessionData;
            UDPC.OnLapDataReceive += StoreLapData;
            UDPC.OnCarStatusDataReceive += StoreCarStatusData;
            UDPC.OnCarDamageDataReceive += StoreCarDamageData;

            slowTimer = new Timer(GetIntervals, null, 0, 1000);
            averagesTimer = new Timer(GetAverageLapTimes, null, 0, 3000);
            pitstopStrategyTimer = new Timer(GetPitStrategy, null, 0, 1000);
            mediumTimer = new Timer(GetLapTimeRanking, null, 0, 2000);
        }

        private void StoreSessionData(PacketSessionData packet)
        {
            latestSessionDataPacket = packet;
        }
        private void StoreCarDamageData(PacketCarDamageData packet)
        {
            latestCarDamagePacket = packet;
        }
        private void StoreLapData(PacketLapData packet)
        {
            latestLapDataPacket = packet;
        }
        private void StoreCarStatusData(PacketCarStatusData packet)
        {
            latestCarStatusPacket = packet;
        }

        private void UDPC_OnEventDataReceive(PacketEventData packet)
        {
            string s = new(packet.m_eventStringCode);

            // EVENT STRING CODES
            switch (s)
            {
                case "SSTA":
                    model.EventStringCode = "Session Started";
                    break;
                case "SEND":
                    model.EventStringCode = "Session End";
                    model.LeftInSession = "Session Complete";
                    break;
                case "FTLP":
                    model.EventStringCode = "New Fastest Lap";
                    break;
                case "SPTP":
                    model.EventStringCode = "New Speed Trap Speed Set";
                    break;
                case "PENA":
                    model.EventStringCode = "Penalty Issued";
                    break;
            }

            if (model.EventStringCode == "Session Started")
            {
                Driver.Clear();
                W_Model.Clear();
            }
        }

        private void UDPC_OnSessionHistoryDataReceive(PacketSessionHistoryData packet)
        {
            TimeSpan[] allLastLapsArray = new TimeSpan[22];
            TimeSpan[] allLastS1Array = new TimeSpan[22];
            TimeSpan[] allLastS2Array = new TimeSpan[22];
            TimeSpan[] allLastS3Array = new TimeSpan[22];

            TimeSpan[] allFastestLapsArray = new TimeSpan[22];
            TimeSpan[] allFastestS1Array = new TimeSpan[22];
            TimeSpan[] allFastestS2Array = new TimeSpan[22];
            TimeSpan[] allFastestS3Array = new TimeSpan[22];

            var carId = packet.m_carIdx;
            var driverData = Driver[carId];

            Driver[carId].BestLapNum = packet.m_bestLapTimeLapNum;
            Driver[carId].BestS1Num = packet.m_bestSector1LapNum;
            Driver[carId].BestS2Num = packet.m_bestSector2LapNum;
            Driver[carId].BestS3Num = packet.m_bestSector3LapNum;
            Driver[carId].NumLaps = packet.m_numLaps;
            Driver[carId].NumTireStints = packet.m_numTyreStints;

            for (int i = 0; i < packet.m_lapHistoryData.Length; i++)
            {
                var histData = packet.m_lapHistoryData[i];

                Driver[carId].AllLapValid[i] = histData.m_lapValidBitFlags;
                Driver[carId].AllLaptimes[i] = histData.m_lapTimeInMS;
                Driver[carId].AllS1Times[i] = histData.m_sector1TimeInMS;
                Driver[carId].AllS2Times[i] = histData.m_sector2TimeInMS;
                Driver[carId].AllS3Times[i] = histData.m_sector3TimeInMS;
            }

            for (int i = 0; i < 22; i++)
            {
                if (Driver[carId].BestLapNum != 0)
                {
                    driverData.LastLapTime = TimeSpan.FromMilliseconds(driverData.AllLaptimes.Where(x => x != 0).DefaultIfEmpty().Last());
                    driverData.LastS1 = TimeSpan.FromMilliseconds(driverData.AllS1Times.Where(x => x != 0).DefaultIfEmpty().Last());
                    driverData.LastS2 = TimeSpan.FromMilliseconds(driverData.AllS2Times.Where(x => x != 0).DefaultIfEmpty().Last());
                    driverData.LastS3 = TimeSpan.FromMilliseconds(driverData.AllS3Times.Where(x => x != 0).DefaultIfEmpty().Last());

                    driverData.BestLapTime = TimeSpan.FromMilliseconds(Driver[carId].AllLaptimes[(Driver[carId].BestLapNum - 1)]);
                    driverData.BestS1 = TimeSpan.FromMilliseconds(Driver[carId].AllS1Times[(Driver[carId].BestS1Num - 1)]);
                    driverData.BestS2 = TimeSpan.FromMilliseconds(Driver[carId].AllS2Times[(Driver[carId].BestS2Num - 1)]);
                    driverData.BestS3 = TimeSpan.FromMilliseconds(Driver[carId].AllS3Times[(Driver[carId].BestS3Num - 1)]);

                    allFastestLapsArray[i] = Driver[i].BestLapTime;
                    allFastestS1Array[i] = Driver[i].BestS1;
                    allFastestS2Array[i] = Driver[i].BestS2;
                    allFastestS3Array[i] = Driver[i].BestS3;
                }
            }
            
            model.FastestLap = allFastestLapsArray.Where(x => x != TimeSpan.Zero).DefaultIfEmpty(model.FastestLap).Min();
            model.FastestS1 = allFastestS1Array.Where(x => x != TimeSpan.Zero).DefaultIfEmpty().Min();
            model.FastestS2 = allFastestS2Array.Where(x => x != TimeSpan.Zero).DefaultIfEmpty().Min();
            model.FastestS3 = allFastestS3Array.Where(x => x != TimeSpan.Zero).DefaultIfEmpty().Min();

            // Last Lap Time Ranking
            var fastestLastLapTimeLookupArray = allLastLapsArray
                .Where(allLastLapsArray => allLastLapsArray != TimeSpan.Zero)
                .Select((data, index) => new { data, index })   // Create a object that looks like {LapData data, int index}
                .OrderBy(value => value.data)                   // Order it by time.
                .Select(value => value.index)                   // Take only the index property from the sorted array
                .ToArray();                                     // Convert the array;

            for (int i = 0; i < fastestLastLapTimeLookupArray.Length; i++)
            {
                var originalIndex = fastestLastLapTimeLookupArray[i];
                var laptimeRank = i + 1;                         

                Driver[fastestLastLapTimeLookupArray[i]].LastLapRank = i + 1;
            }

            // Last S1 Time Ranking
            var fastestLastS1LookupArray = allLastS1Array
                .Where(allLastS1Array => allLastS1Array != TimeSpan.Zero)
                .Select((data, index) => new { data, index })   // Create a object that looks like {LapData data, int index}
                .OrderBy(value => value.data)                   // Order it by time.
                .Select(value => value.index)                   // Take only the index property from the sorted array
                .ToArray();                                     // Convert the array;

            for (int i = 0; i < fastestLastS1LookupArray.Length; i++)
            {
                var originalIndex = fastestLastS1LookupArray[i];
                var laptimeRank = i + 1;

                Driver[fastestLastS1LookupArray[i]].LastS1Rank = i + 1;
            }

            // Last S2 Time Ranking
            var fastestLastS2LookupArray = allLastS2Array
                .Where(allLastS2Array => allLastS2Array != TimeSpan.Zero)
                .Select((data, index) => new { data, index })   // Create a object that looks like {LapData data, int index}
                .OrderBy(value => value.data)                   // Order it by time.
                .Select(value => value.index)                   // Take only the index property from the sorted array
                .ToArray();                                     // Convert the array;

            for (int i = 0; i < fastestLastS2LookupArray.Length; i++)
            {
                var originalIndex = fastestLastS2LookupArray[i];
                var laptimeRank = i + 1;

                Driver[fastestLastS2LookupArray[i]].LastS2Rank = i + 1;
            }

            // Last S3 Time Ranking
            var fastestLastS3LookupArray = allLastS3Array
                .Where(allLastS3Array => allLastS3Array != TimeSpan.Zero)
                .Select((data, index) => new { data, index })   // Create a object that looks like {LapData data, int index}
                .OrderBy(value => value.data)                   // Order it by time.
                .Select(value => value.index)                   // Take only the index property from the sorted array
                .ToArray();                                     // Convert the array;

            for (int i = 0; i < fastestLastS3LookupArray.Length; i++)
            {
                var originalIndex = fastestLastS3LookupArray[i];
                var laptimeRank = i + 1;

                Driver[fastestLastS3LookupArray[i]].LastS3Rank = i + 1;
            }

            // Fastest Lap Time Ranking
            var fastestLapTimeLookupArray = allFastestLapsArray
                .Where(allFastestLapsArray => allFastestLapsArray != TimeSpan.Zero)
                .Select((data, index) => new { data, index })   // Create a object that looks like {LapData data, int index}
                .OrderBy(value => value.data)                   // Order it by time.
                .Select(value => value.index)                   // Take only the index property from the sorted array
                .ToArray();                                     // Convert the array;

            for (int i = 0; i < fastestLapTimeLookupArray.Length; i++)
            {
                var originalIndex = fastestLapTimeLookupArray[i];
                var laptimeRank = i + 1;

                Driver[fastestLapTimeLookupArray[i]].FastestLapRank = i + 1;
            }

            // Fastest S1 Time Ranking
            var FastestS1LookupArray = allFastestS1Array
                .Where(allFastestS1Array => allFastestS1Array != TimeSpan.Zero)
                .Select((data, index) => new { data, index })   // Create a object that looks like {LapData data, int index}
                .OrderBy(value => value.data)                   // Order it by time.
                .Select(value => value.index)                   // Take only the index property from the sorted array
                .ToArray();                                     // Convert the array;

            for (int i = 0; i < FastestS1LookupArray.Length; i++)
            {
                var originalIndex = FastestS1LookupArray[i];
                var laptimeRank = i + 1;

                Driver[FastestS1LookupArray[i]].FastestS1Rank = i + 1;
            }

            // Fastest S2 Time Ranking
            var FastestS2LookupArray = allFastestS2Array
                .Where(allFastestS2Array => allFastestS2Array != TimeSpan.Zero)
                .Select((data, index) => new { data, index })   // Create a object that looks like {LapData data, int index}
                .OrderBy(value => value.data)                   // Order it by time.
                .Select(value => value.index)                   // Take only the index property from the sorted array
                .ToArray();                                     // Convert the array;

            for (int i = 0; i < FastestS2LookupArray.Length; i++)
            {
                var originalIndex = FastestS2LookupArray[i];
                var laptimeRank = i + 1;

                Driver[FastestS2LookupArray[i]].FastestS2Rank = i + 1;
            }

            // Fastest S3 Time Ranking
            var FastestS3LookupArray = allFastestS3Array
                .Where(allFastestS3Array => allFastestS3Array != TimeSpan.Zero)
                .Select((data, index) => new { data, index })   // Create a object that looks like {LapData data, int index}
                .OrderBy(value => value.data)                   // Order it by time.
                .Select(value => value.index)                   // Take only the index property from the sorted array
                .ToArray();                                     // Convert the array;

            for (int i = 0; i < FastestS3LookupArray.Length; i++)
            {
                var originalIndex = FastestS3LookupArray[i];
                var laptimeRank = i + 1;

                Driver[FastestS3LookupArray[i]].FastestS3Rank = i + 1;
            }

            // Tire History Data
            for (int i = 0; i < packet.m_tyreStintsHistoryData.Length; i++) // For each tire stint
            {
                var tireData = packet.m_tyreStintsHistoryData[i];

                if (tireData.m_endLap == 255)
                {
                    Driver[carId].EndLap[i] = "Current";
                }
                else
                {
                    Driver[carId].EndLap[i] = tireData.m_endLap.ToString();
                }
                
                Driver[carId].TireActual[i] = tireData.m_tyreActualCompound.ToString();
                Driver[carId].TireVisual[i] = (VisualTireCompounds)tireData.m_tyreVisualCompound;
            }

            if (model.LeftInSession == "Session Complete")
            {
                
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
                model.IsRace = true;
            }

            if (model.IsRace == true)
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

                // Only show weather forecasts for the current session.
                if (W_Model[i].SessionType == model.CurrentSession)
                {
                    W_Model[i].IsCurrentSession = true;
                }
                else
                {
                    W_Model[i].IsCurrentSession = false;
                }
            }

            // Set Leaderboard Light Descriptions.
            if (model.IsRace == true)
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
                model.LightSix = "In-Garage"; // white
                model.LightSeven = "Out-Lap";
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
                if (model.NetworkGame == NetworkTypes.Offline | Driver[i].AI == 1)
                {
                    Driver[i].DriverName = Regex.Replace(participant.m_driverId.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
                }
            }
        }

        private void UDPC_OnLapDataReceive(PacketLapData packet)
        {
            for (int i = 0; i < model.NumOfActiveCars; i++)
            {
                var lapData = packet.lapData[i];

                Driver[i].CarPosition = lapData.carPosition;
                Driver[i].GridPosition = lapData.gridPosition;
                Driver[i].CurrentLapTime = TimeSpan.FromMilliseconds(lapData.currentLapTimeInMS);
                Driver[i].NumPitstops = lapData.numPitStops;
                Driver[i].Warnings = lapData.warnings;
                Driver[i].Penalties = lapData.penalties;
                Driver[i].DriverStatus = lapData.driverStatus;
                Driver[i].ResultStatus = lapData.resultStatus;
                Driver[i].PitStatus = lapData.pitStatus;

                if (model.IsRace == true) // Set current lap num
                {
                    Driver[i].CurrentLapNum = lapData.currentLapNum - 1;
                }
                else
                {
                    Driver[i].CurrentLapNum = lapData.currentLapNum;
                }

                if (model.IsRace == true) // Set Actual Driver Status
                {
                    switch (Driver[i].PitStatus)
                    {
                        case PitStatus.Unknown:
                        case PitStatus.None:
                            Driver[i].ActualDriverStatus = Driver[i].ResultStatus.ToString();
                            break;
                        case PitStatus.In_This_Lap:
                        case PitStatus.In_Pit_Lane:
                            Driver[i].ActualDriverStatus = Driver[i].PitStatus.ToString();
                            break;
                        default:
                            Driver[i].ActualDriverStatus = Driver[i].ResultStatus.ToString();
                            break;
                    }
                }
                else
                {
                    Driver[i].ActualDriverStatus = Driver[i].DriverStatus.ToString();
                }

                if (model.IsRace == true) // Set lead lap, and current/fastest laptime.
                {
                    Driver[i].PositionChange = (sbyte)(Driver[i].GridPosition - Driver[i].CarPosition); // Calculate position change from starting position.

                    if (Driver[i].CarPosition == 1)
                    {
                        model.LeadLap = lapData.currentLapNum; // Lead lap of the race.
                        model.LeadLapTime = Driver[i].CurrentLapTime; // Current lap time for lead driver.
                    }
                }

                switch (Driver[i].ActualDriverStatus) // Set which sector times to display on leaderboard
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
            for (int i = 0; i < model.NumOfActiveCars; i++)
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
            for (int i = 0; i < model.NumOfActiveCars; i++)
            {
                var carDamageData = packet.m_carDamageData[i];

                for (int a = 0; a < carDamageData.m_tyresWear.Length; a++)
                {
                    Driver[i].RLTireWear = carDamageData.m_tyresWear[0];
                    Driver[i].RRTireWear = carDamageData.m_tyresWear[1];
                    Driver[i].FLTireWear = carDamageData.m_tyresWear[2];
                    Driver[i].FRTireWear = carDamageData.m_tyresWear[3];
                }
                Driver[i].TireWear = ((Driver[i].RLTireWear + Driver[i].RRTireWear + Driver[i].FLTireWear + Driver[i].FRTireWear) / 4);
            }
        }

        private void GetLapTimeRanking(object state = null)
        {
            for (int i = 0; i < model.NumOfActiveCars; i++)
            {
                if (Driver[i].BestLapTime == model.FastestLap && Driver[i].BestLapTime != TimeSpan.Zero)
                {
                    Driver[i].HasFastestLap = true;
                }
                else
                {
                    Driver[i].HasFastestLap = false;
                }

                if (Driver[i].BestS1 == model.FastestS1 && Driver[i].BestS1 != TimeSpan.Zero)
                {
                    Driver[i].HasFastestS1 = true;
                }
                else
                {
                    Driver[i].HasFastestS1 = false;
                }

                if (Driver[i].BestS2 == model.FastestS2 && Driver[i].BestS2 != TimeSpan.Zero)
                {
                    Driver[i].HasFastestS2 = true;
                }
                else
                {
                    Driver[i].HasFastestS2 = false;
                }

                if (Driver[i].BestS3 == model.FastestS3 && Driver[i].BestS3 != TimeSpan.Zero)
                {
                    Driver[i].HasFastestS3 = true;
                }
                else
                {
                    Driver[i].HasFastestS3 = false;
                }
            }
        }

        private void GetPitStrategy(object state = null)
        {
            PacketSessionData sessionDataPacket = latestSessionDataPacket;
            PacketLapData lapDataPacket = latestLapDataPacket;

            int[] IndexToPositionArr = new int[22]; // 22 cars.
            int[] numStops = new int[22];

            if (lapDataPacket.lapData != null && model.IsRace == true) // Get number of drivers who have made a pitstop.
            {
                // GET NUMBER OF DRIVERS THAT HAVE MADE A PITSTOP
                for (int i = 0; i < lapDataPacket.lapData.Length; i++)
                {
                    if (Driver[i].NumPitstops > 0)
                    {
                        numStops[i] = 1;
                    }
                    else
                    {
                        numStops[i] = 0;
                    }
                }

                // GET AVERAGE PITLANE TIMES
                float[] allPitlaneTimes = new float[lapDataPacket.lapData.Length]; // Create an array of pitstop times.

                if (numStops.Sum() > 0)
                {
                    for (int i = 0; i < lapDataPacket.lapData.Length; i++)
                    {
                        var lapData = lapDataPacket.lapData[i];

                        Debug.WriteLine(lapData.pitLaneTimeInLaneInMS);

                        if (lapData.pitLaneTimeInLaneInMS != 0 && lapData.pitLaneTimeInLaneInMS > 15000)
                        {
                            model.AllPitlaneTimes[i] = lapData.pitLaneTimeInLaneInMS;
                        }
                    }

                    if (model.AllPitlaneTimes.Average() > 0)
                    {
                        model.AverageTimeInPitlane = TimeSpan.FromMilliseconds(model.AllPitlaneTimes.Where(x => x != 0).DefaultIfEmpty().Average());
                    }
                    else
                    {
                        model.AverageTimeInPitlane = TimeSpan.FromSeconds(21);
                    }
                }
                else
                {
                    model.AverageTimeInPitlane = TimeSpan.FromSeconds(21);
                }

                Debug.WriteLine(model.AverageTimeInPitlane);
               

                // PITSTOP REJOIN STRATEGY
                int playerIndex = lapDataPacket.header.playerCarIndex; // Set player's index
                var playerData = lapDataPacket.lapData[playerIndex]; // Set player data

                LapDataUtils.UpdatePositionArray(lapDataPacket.lapData, ref IndexToPositionArr); // Sort drivers by position

                // Player: 45
                // Ahead: 23 ( + 22 seconds )
                // Pitstop: 25

                // 45 - 23 = 22
                // 22 - 25 = -3 <------ CORRECT!

                // Player: 45
                // Behind: 17 ( + 28 seconds )
                // Pitstop: 25

                // 45 - 17 = 28
                // 28 - 25 = +3 <------ CORRECT!

                // If a lap back <- ( last lap + current lap ) - behind current lap

                int playerRejoinPosition = sessionDataPacket.m_pitStopRejoinPosition;// Set Player's pit rejoin position

                if (playerRejoinPosition > 1) // Can't calculate delta to car ahead if car will rejoin in 1st.
                {
                    int driverAheadOnRejoinPosition = playerRejoinPosition - 1; // Set position of car ahead of driver after pitstop rejoin.

                    var driverAheadData = lapDataPacket.lapData[IndexingUtils.GetRealIndex(IndexToPositionArr, driverAheadOnRejoinPosition)];

                    if (playerData.currentLapNum == driverAheadData.currentLapNum) // If cars are on the same lap.
                    {
                        var Gap = TimeSpan.FromMilliseconds(playerData.currentLapTimeInMS - driverAheadData.currentLapTimeInMS);
                        model.GapToCarAheadOnRejoin = Gap - model.AverageTimeInPitlane; // Calculate delta.
                    }
                    else if (playerData.currentLapNum == driverAheadData.currentLapNum - 1) // If driver ahead on rejoin is 1 lap behind player.
                    {
                        model.GapToCarAheadOnRejoin = TimeSpan.FromMilliseconds((playerData.lastLapTimeInMS + playerData.currentLapTimeInMS) - driverAheadData.currentLapTimeInMS) - model.AverageTimeInPitlane; // Calculate delta.
                    }
                    else
                    {
                        model.GapToCarBehindOnRejoin = TimeSpan.FromSeconds(0); // Else, no delta
                    }
                }
                else
                {
                    model.GapToCarAheadOnRejoin = TimeSpan.FromSeconds(0);
                }

                if (playerRejoinPosition != 0 && playerRejoinPosition != model.MaxIndex + 1) // Can't calculate delta to car behind if car will rejoin in last.
                {
                    int driverBehindOnRejoinPosition = playerRejoinPosition + 1; // Set position of car behind of driver after pitstop rejoin.

                    var driverBehindData = lapDataPacket.lapData[IndexingUtils.GetRealIndex(IndexToPositionArr, driverBehindOnRejoinPosition)];

                    if (driverBehindData.currentLapNum == playerData.currentLapNum)
                    {
                        model.GapToCarBehindOnRejoin = TimeSpan.FromMilliseconds(playerData.currentLapTimeInMS - driverBehindData.currentLapTimeInMS) - model.AverageTimeInPitlane;
                    }
                    else if (driverBehindData.currentLapNum == playerData.currentLapNum - 1)
                    {
                        model.GapToCarBehindOnRejoin = TimeSpan.FromMilliseconds((playerData.lastLapTimeInMS + playerData.currentLapTimeInMS) - driverBehindData.currentLapTimeInMS) - model.AverageTimeInPitlane; ;
                    }
                    else
                    {
                        model.GapToCarBehindOnRejoin = TimeSpan.FromSeconds(0);
                    }
                }
                else
                {
                    model.GapToCarBehindOnRejoin = TimeSpan.FromSeconds(0);
                }

                //Debug.WriteLine("Gap To Ahead: " + " + " + model.GapToCarAheadOnRejoin + " Gap To Behind: " + " + " + model.GapToCarBehindOnRejoin);
            }
        }

        private void GetAverageLapTimes(object state = null)
        {
            PacketLapData lapDataPacket = latestLapDataPacket;
            PacketCarStatusData carStatusDataPacket = latestCarStatusPacket;
            PacketCarDamageData carDamageDataPacket = latestCarDamagePacket;

            // Tire wear per lap is just difference from the last lap.
            if (lapDataPacket.lapData != null && carStatusDataPacket.m_carStatusData != null && carDamageDataPacket.m_carDamageData != null)
            {
                float[] allLapTimes = new float[model.MaxIndex + 1]; // Collect all laptimes
                float[] softTimes = new float[model.NumSoftTires]; // Collect all soft tire laptimes
                float[] mediumTimes = new float[model.NumMediumTires];
                float[] hardTimes = new float[model.NumHardTires];
                float[] interTimes = new float[model.NumInterTires];
                float[] wetTimes = new float[model.NumWetTires];

                float[] allWear = new float[model.MaxIndex + 1];
                float[] softWear = new float[model.NumSoftTires];
                float[] mediumWear = new float[model.NumMediumTires];
                float[] hardWear = new float[model.NumHardTires];
                float[] interWear = new float[model.NumInterTires];
                float[] wetWear = new float[model.NumWetTires];

                float[] allWearLaps = new float[model.MaxIndex + 1];
                float[] softWearLaps = new float[model.NumSoftTires];
                float[] mediumWearLaps = new float[model.NumMediumTires];
                float[] hardWearLaps = new float[model.NumHardTires];
                float[] interWearLaps = new float[model.NumInterTires];
                float[] wetWearLaps = new float[model.NumWetTires];

                for (int i = 0; i < lapDataPacket.lapData.Length; i++)
                {
                    var carStatusData = latestCarStatusPacket.m_carStatusData[i];
                    var lapData = lapDataPacket.lapData[i];
                    var carDamageData = latestCarDamagePacket.m_carDamageData[i];

                    if (carStatusData.m_visualTyreCompound == VisualTireCompounds.Soft)
                    {
                        for (int j = 0; j < model.NumSoftTires; j++)
                        {
                            var driverWear = (carDamageData.m_tyresWear[0] + carDamageData.m_tyresWear[1] + carDamageData.m_tyresWear[2] + carDamageData.m_tyresWear[3]) / 4;
                            softTimes[j] = lapData.lastLapTimeInMS;
                            softWear[j] = driverWear;
                            softWearLaps[j] = carStatusData.m_tyresAgeLaps;

                        }
                    }
                    else if (carStatusData.m_visualTyreCompound == VisualTireCompounds.Medium)
                    {
                        for (int j = 0; j < model.NumMediumTires; j++)
                        {
                            var driverWear = (carDamageData.m_tyresWear[0] + carDamageData.m_tyresWear[1] + carDamageData.m_tyresWear[2] + carDamageData.m_tyresWear[3]) / 4;
                            mediumTimes[j] = lapData.lastLapTimeInMS;
                            mediumWear[j] = driverWear;
                            mediumWearLaps[j] = carStatusData.m_tyresAgeLaps;
                        }
                    }
                    else if (carStatusData.m_visualTyreCompound == VisualTireCompounds.Hard)
                    {
                        for (int j = 0; j < model.NumHardTires; j++)
                        {
                            var driverWear = (carDamageData.m_tyresWear[0] + carDamageData.m_tyresWear[1] + carDamageData.m_tyresWear[2] + carDamageData.m_tyresWear[3]) / 4;
                            hardTimes[j] = lapData.lastLapTimeInMS;
                            hardWear[j] = driverWear;
                            hardWearLaps[j] = carStatusData.m_tyresAgeLaps;
                        }
                    }
                    else if (carStatusData.m_visualTyreCompound == VisualTireCompounds.Inter)
                    {
                        for (int j = 0; j < model.NumInterTires; j++)
                        {
                            var driverWear = (carDamageData.m_tyresWear[0] + carDamageData.m_tyresWear[1] + carDamageData.m_tyresWear[2] + carDamageData.m_tyresWear[3]) / 4;
                            interTimes[j] = lapData.lastLapTimeInMS;
                            interWear[j] = driverWear;
                            interWearLaps[j] = carStatusData.m_tyresAgeLaps;
                        }
                    }
                    else if (carStatusData.m_visualTyreCompound == VisualTireCompounds.Wet)
                    {
                        for (int j = 0; j < model.NumWetTires; j++)
                        {
                            var driverWear = (carDamageData.m_tyresWear[0] + carDamageData.m_tyresWear[1] + carDamageData.m_tyresWear[2] + carDamageData.m_tyresWear[3]) / 4;
                            wetTimes[j] = lapData.lastLapTimeInMS;
                            wetWear[j] = driverWear;
                            wetWearLaps[j] = carStatusData.m_tyresAgeLaps;
                        }
                    }
                }

                if (allLapTimes.Length != 0)
                {
                    model.AverageLapTime = TimeSpan.FromMilliseconds(allLapTimes.Average());
                    model.AverageTireWear = allWear.Average();
                }
                else
                {
                    model.AverageLapTime = TimeSpan.Zero;
                    model.AverageTireWear = 0;
                }

                if (softTimes.Length != 0)
                {
                    model.AverageSoftTime = TimeSpan.FromMilliseconds(softTimes.Average());
                    model.AverageSoftWear = softWear.Average();

                    if (model.LeadLap > 1)
                    {
                        model.AverageSoftWearRate = model.AverageSoftWear / (model.LeadLap - 1);
                    }
                    else
                    {
                        model.AverageSoftWearRate = 0;
                    }
                }
                else
                {
                    model.AverageSoftTime = TimeSpan.Zero;
                    model.AverageSoftWear = 0;
                }

                if (mediumTimes.Length != 0)
                {
                    model.AverageMediumTime = TimeSpan.FromMilliseconds(mediumTimes.Average());
                    model.AverageMediumWear = mediumWear.Average();

                    if (model.LeadLap > 1)
                    {
                        model.AverageMediumWearRate = model.AverageMediumWear / (model.LeadLap - 1);
                    }
                    else
                    {
                        model.AverageMediumWearRate = 0;
                    }
                }
                else
                {
                    model.AverageMediumTime = TimeSpan.Zero;
                    model.AverageMediumWear = 0;
                }

                if (hardTimes.Length != 0)
                {
                    model.AverageHardTime = TimeSpan.FromMilliseconds(hardTimes.Average());
                    model.AverageHardWear = hardWear.Average();

                    if (model.LeadLap > 1)
                    {
                        model.AverageHardWearRate = model.AverageHardWear / (model.LeadLap - 1);
                    }
                    else
                    {
                        model.AverageHardWearRate = 0;
                    }
                }
                else
                {
                    model.AverageHardTime = TimeSpan.Zero;
                    model.AverageHardWear = 0;
                }

                if (interTimes.Length != 0)
                {
                    model.AverageInterTime = TimeSpan.FromMilliseconds(interTimes.Average());
                    model.AverageInterWear = interWear.Average();

                    if (model.LeadLap > 1)
                    {
                        model.AverageInterWearRate = model.AverageInterWear / (model.LeadLap - 1);
                    }
                    else
                    {
                        model.AverageInterWearRate = 0;
                    }
                }
                else
                {
                    model.AverageInterTime = TimeSpan.Zero;
                    model.AverageInterWear = 0;
                }

                if (wetTimes.Length != 0)
                {
                    model.AverageWetTime = TimeSpan.FromMilliseconds(wetTimes.Average());
                    model.AverageWetWear = wetWear.Average();

                    if (model.LeadLap > 1)
                    {
                        model.AverageWetWearRate = model.AverageWetWear / (model.LeadLap - 1);
                    }
                    else
                    {
                        model.AverageWetWearRate = 0;
                    }
                }
                else
                {
                    model.AverageWetTime = TimeSpan.Zero;
                    model.AverageWetWear = 0;
                }
            }
        }

        private void GetIntervals(object state = null)
        {
            PacketLapData lapDataPacket = latestLapDataPacket;

            // Interval to car ahead.
            if (lapDataPacket.lapData != null)
            {
                int[] IndexToPositionArr = new int[22]; // 22 cars.

                LapDataUtils.UpdatePositionArray(lapDataPacket.lapData, ref IndexToPositionArr); // Sort drivers by position.
                                                                                                 // Set the delta for the first person to zero
                Driver[IndexingUtils.GetRealIndex(IndexToPositionArr, 1)].RaceInterval = TimeSpan.FromMilliseconds(0);

                for (int i = 0; i < model.NumOfActiveCars - 1; i++)
                {
                    var currCar = IndexingUtils.GetByRealPosition(lapDataPacket.lapData, IndexToPositionArr, i + 1); // 0 + 1 = Position 1 // Car Ahead
                    var carBehind = IndexingUtils.GetByRealPosition(lapDataPacket.lapData, IndexToPositionArr, i + 2); // 0 + 2 == Position 2 // Car Behind

                    uint delta = currCar.currentLapTimeInMS - carBehind.currentLapTimeInMS;

                    if (currCar.currentLapNum == carBehind.currentLapNum)
                    {
                        Driver[IndexingUtils.GetRealIndex(IndexToPositionArr, i + 2)].RaceInterval = TimeSpan.FromMilliseconds(delta);
                    }

                    if (Driver[i].CurrentLapNum == model.LeadLap - 1 && Driver[i].CarPosition != 1)
                    {
                        Driver[i].RaceIntervalLeader = model.LeadLapTime - Driver[i].CurrentLapTime; // If car is not leading, set interval to leader.
                    }
                }
            }
            // Interval to fastest lap
            for (int i = 0; i < 22; i++)
            {
                if (Driver[i].BestLapTime != TimeSpan.Zero)
                {
                    Driver[i].BestLapDelta = model.FastestLap - Driver[i].BestLapTime;
                }
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