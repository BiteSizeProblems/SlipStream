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

        // Create a observable collection of DriverModel
        public ObservableCollection<DriverModel> Driver { get; set; }
        private object _driverLock = new object();

        // Create a observable collection of WeatherModel
        public ObservableCollection<WeatherModel> W_Model { get; set; }
        private object _w_modelLock = new object();

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

            this.model = new SessionModel();

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
            UDPC.OnParticipantsDataReceive += UDPC_OnParticipantsDataReceive;
            UDPC.OnSessionDataReceive += UDPC_OnSessionDataReceive;
            UDPC.OnCarStatusDataReceive += UDPC_OnCarStatusDataReceive;
            UDPC.OnCarDamageDataReceive += UDPC_OnCarDamageDataReceive;
            UDPC.OnSessionHistoryDataReceive += UDPC_OnSessionHistoryDataReceive;
        }

        private void UDPC_OnSessionHistoryDataReceive(PacketSessionHistoryData packet)
        {
            for (int i = 0; i < 22; i++)
            {
                var lapHistoryData = packet.m_lapHistoryData[i];

                Driver[i].BestLapTime = TimeSpan.FromMilliseconds(lapHistoryData.m_lapTimeInMS);
                Driver[i].S1Time = TimeSpan.FromMilliseconds(lapHistoryData.m_sector1TimeInMS);
                Driver[i].S2Time = TimeSpan.FromMilliseconds(lapHistoryData.m_sector2TimeInMS);
                Driver[i].S3Time = TimeSpan.FromMilliseconds(lapHistoryData.m_sector3TimeInMS);
            }
        }

        private void UDPC_OnCarDamageDataReceive(PacketCarDamageData packet)
        {

        }

        private void UDPC_OnSessionDataReceive(PacketSessionData packet)
        {
            if (packet.networkGame == 0)
            {
                model.NetworkGame = "Network Type: Offline";
            }
            else
            {
                model.NetworkGame = "Network Type: Online";
            }

            model.SafetyCarStatus = Regex.Replace(packet.safetyCarStatus.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();

            if(model.SafetyCarStatus == "Clear")
            {
                model.SafetyCarColor = "Green";
            }
            else model.SafetyCarColor = "Yellow";

            model.CurrentWeather = Regex.Replace(packet.weather.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();

            if(model.CurrentWeather == "Clear")
            {
                model.CurrentWeatherIcon = "/Core/Images/clear.png";
            }
            else if (model.CurrentWeather == "Overcast")
            {
                model.CurrentWeatherIcon = "/Core/Images/overcast.png";
            }
            else if (model.CurrentWeather == "Light Rain")
            {
                model.CurrentWeatherIcon = "/Core/Images/light_rain.png";
            }
            else if (model.CurrentWeather == "Heavy Rain")
            {
                model.CurrentWeatherIcon = "/Core/Images/heavy_rain.png";
            }
            else if (model.CurrentWeather == "Storm")
            {
                model.CurrentWeatherIcon = "/Core/Images/storm.png";
            }
        }

        private void UDPC_OnParticipantsDataReceive(PacketParticipantsData packet)
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

                if (model.NetworkGame == "Network Type: Offline")
                {
                    Driver[i].DriverName = Regex.Replace(participant.m_driverId.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
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

                Driver[i].TireAge = carStatusData.m_tyresAgeLaps;

                Driver[i].VisualTireCompound = (VisualTireCompounds)carStatusData.m_visualTyreCompound;

                //if(IndexDriverStatus != InGarage)

                if (carStatusData.m_fuelMix == 0)
                {
                    Driver[i].FuelMix = $"Current Fuel Mix: Lean";
                }
                if (carStatusData.m_fuelMix == 0)
                {
                    Driver[i].FuelMix = $"Current Fuel Mix: Standard";
                }
                if (carStatusData.m_fuelMix == 0)
                {
                    Driver[i].FuelMix = $"Current Fuel Mix: Rich";
                }
                if (carStatusData.m_fuelMix == 0)
                {
                    Driver[i].FuelMix = $"Current Fuel Mix: Max";
                }



                Driver[i].VehicleFlag = $"Zone Flags: {carStatusData.m_vehicleFiaFlags}";

                Driver[i].ErsRemaining = $"ERS Remaining: {(carStatusData.m_ersStoreEnergy / 40000)}%";

                if (carStatusData.m_ersDeployMode == 0)
                {
                    Driver[i].ErsDeployMode = $"ERS Mode: None";
                }
                if (carStatusData.m_ersDeployMode == 1)
                {
                    Driver[i].ErsDeployMode = $"ERS Mode: Medium";
                }
                if (carStatusData.m_ersDeployMode == 2)
                {
                    Driver[i].ErsDeployMode = $"ERS Mode: Hotlap";
                }
                if (carStatusData.m_ersDeployMode == 3)
                {
                    Driver[i].ErsDeployMode = $"ERS Mode: Overtake";
                }




                if (Driver[i].VisualTireCompound == VisualTireCompounds.Soft)
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

        private void UDPC_OnLapDataReceive(PacketLapData packet)
        {
            
            
            for (int i = 0; i < packet.lapData.Length; i++)
            {
                var lapData = packet.lapData[i];

                Driver[i].LastLapTime = TimeSpan.FromMilliseconds(lapData.lastLapTimeInMS);
            }

                
        }
    }
}
