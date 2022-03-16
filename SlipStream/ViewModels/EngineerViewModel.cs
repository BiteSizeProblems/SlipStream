using SlipStream.Core;
using SlipStream.Models;
using SlipStream.Structs;
using SlipStream.Views;
using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Media;
using static SlipStream.Structs.Appendeces;

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

        public SessionModel model { get; set; }
        public DriverModel driver { get; set; }
        public WeatherModel w_model { get; set; }

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

        // Create a observable collection of Models
        public ObservableCollection<DriverModel> Driver { get; set; }
        private object _driverLock = new object();
        public ObservableCollection<WeatherModel> W_Model { get; set; }
        private object _w_modelLock = new object();

        private EngineerViewModel() : base()
        {
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

            this.model = new SessionModel();

            // Set New Observable Collection
            Driver = new ObservableCollection<DriverModel>();
            BindingOperations.EnableCollectionSynchronization(Driver, _driverLock);
            W_Model = new ObservableCollection<WeatherModel>();
            BindingOperations.EnableCollectionSynchronization(W_Model, _w_modelLock);

            NumActiveCars = 22;

            for (int i = 0; i < NumActiveCars; i++)
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
            UDPC.OnParticipantsDataReceive += UDPC_OnParticipantsDataReceive;
            UDPC.OnSessionDataReceive += UDPC_OnSessionDataReceive;
            UDPC.OnCarStatusDataReceive += UDPC_OnCarStatusDataReceive;
            UDPC.OnCarDamageDataReceive += UDPC_OnCarDamageDataReceive;
            UDPC.OnSessionHistoryDataReceive += UDPC_OnSessionHistoryDataReceive;
        }

        private void UDPC_OnSessionHistoryDataReceive(PacketSessionHistoryData packet)
        {
            for (int i = 0; i < NumActiveCars; i++)
            {
                var lapHistoryData = packet.m_lapHistoryData[i];

                Driver[i].lapNum = Driver[i].lapNum + 1;
                Driver[i].CarIdx = packet.m_carIdx;
                Driver[i].LapTime = TimeSpan.FromMilliseconds(lapHistoryData.m_lapTimeInMS);
                Driver[i].BestLapTime = TimeSpan.FromMilliseconds(lapHistoryData.m_lapTimeInMS);
                Driver[i].S1Time = TimeSpan.FromMilliseconds(lapHistoryData.m_sector1TimeInMS);
                Driver[i].S2Time = TimeSpan.FromMilliseconds(lapHistoryData.m_sector2TimeInMS);
                Driver[i].S3Time = TimeSpan.FromMilliseconds(lapHistoryData.m_sector3TimeInMS);
            }
        }

        private void UDPC_OnCarDamageDataReceive(PacketCarDamageData packet)
        {
            for (int i = 0; i < NumActiveCars; i++)
            {
                var carDamageData = packet.m_carDamageData[i];

                for (int a = 0; a < carDamageData.m_tyresWear.Length; a++)
                {
                    Driver[i].FLTireWear = carDamageData.m_tyresWear[1];
                    Driver[i].FRTireWear = carDamageData.m_tyresWear[1];
                    Driver[i].RLTireWear = carDamageData.m_tyresWear[1];
                    Driver[i].RRTireWear = carDamageData.m_tyresWear[1];
                }   
            }   
        }

        private void UDPC_OnSessionDataReceive(PacketSessionData packet)
        {
            model.Formula = Regex.Replace(packet.m_formula.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
            model.Circuit = Regex.Replace(packet.m_trackId.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
            model.CurrentSession = Regex.Replace(packet.m_sessionType.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
            model.SafetyCarStatus = Regex.Replace(packet.m_safetyCarStatus.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
            model.CurrentWeather = Regex.Replace(packet.m_weather.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();

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
            else { model.NetworkGame = "Online";}

            // SAFETY CAR STATUS
            if (model.SafetyCarStatus == "Clear")
            {
                model.SafetyCarIcon = "/Core/Images/check.png";
            }
            else if(model.SafetyCarStatus != "Clear")
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
        }

        private void UDPC_OnParticipantsDataReceive(PacketParticipantsData packet)
        {
            model.NumOfActiveCars = packet.m_numActiveCars.ToString();
            NumActiveCars = packet.m_numActiveCars;
            model.TotalParticipants = packet.m_participants.Length;

            for (int i = 0; i < packet.m_participants.Length; i++)
            {
                var participant = packet.m_participants[i];

                Driver[i].TeamID = participant.m_teamId;
                Driver[i].TeamName = Regex.Replace(participant.m_teamId.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
                Driver[i].raceNumber = participant.m_raceNumber;
                Driver[i].AI = participant.m_aiControlled;

                if (model.NetworkGame == "Offline")
                {
                    Driver[i].DriverName = Regex.Replace(participant.m_driverId.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
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
                Driver[i].VisualTireCompound = (VisualTireCompounds)carStatusData.m_visualTyreCompound;
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
                Driver[i].CurrentLapNum = lapData.currentLapNum - 1;
                Driver[i].DriverStatus = Regex.Replace(lapData.driverStatus.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
                Driver[i].DriverStatusUpdate = lapData.driverStatus;

                // POSITION CHANGE
                if (model.CurrentSession == "Race")
                {
                    Driver[i].PositionChange = (sbyte)(Driver[i].CarPosition - Driver[i].GridPosition);

                    if (Driver[i].PositionChange > 0)
                    {
                        Driver[i].PositionChangeIcon = "/Core/Images/up_arrow.png";
                    }
                    else if (Driver[i].PositionChange < 0)
                    {
                        Driver[i].PositionChangeIcon = "/Core/Images/down_arrow.png";
                    }
                }

                // BEST LAP GAP
                if (model.CurrentSession != "Race")
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
    }
}
