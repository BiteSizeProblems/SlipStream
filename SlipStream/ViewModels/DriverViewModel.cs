using SlipStream.Core;
using SlipStream.Models;
using SlipStream.Structs;
using SlipStream.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using static SlipStream.Structs.Appendeces;

namespace SlipStream.ViewModels
{
    public class DriverViewModel : BaseViewModel
    {
        // === BEGINING OF MODULE SETUP ===
        // === Singleton Instance with Thread Saftey ===
        private static DriverViewModel _instance = null;
        private static object _singletonLock = new object();
        public static DriverViewModel GetInstance()
        {
            lock (_singletonLock)
            {
                if (_instance == null) { _instance = new DriverViewModel(); }
                return _instance;
            }
        }
        // === END OF MODULE SETUP ===

        public SessionModel model { get; set; }
        public DriverModel driver { get; set; }

        public RelayCommand DriverDefaultViewCommand { get; set; }
        public DriverDefaultView DDV { get; set; }

        public RelayCommand DriverLeaderboardViewCommand { get; set; }
        public DriverLeaderboardView DLV { get; set; }

        public RelayCommand DriverStrategyViewCommand { get; set; }
        public DriverStrategyView DSV { get; set; }

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

        // Create a observable collection of Models
        public ObservableCollection<DriverModel> Driver { get; set; }
        private object _driverLock = new object();

        private DriverViewModel() : base()
        {
            DDV = new DriverDefaultView();
            DLV = new DriverLeaderboardView();
            DSV = new DriverStrategyView();

            CurrentSubView = DDV;

            DriverDefaultViewCommand = new RelayCommand(o =>
            {
                CurrentSubView = DDV;
            });

            DriverStrategyViewCommand = new RelayCommand(o =>
            {
                CurrentSubView = DSV;
            });

            DriverLeaderboardViewCommand = new RelayCommand(o =>
            {
                CurrentSubView = DLV;
            });

            // Set New Observable Collection
            Driver = new ObservableCollection<DriverModel>();
            BindingOperations.EnableCollectionSynchronization(Driver, _driverLock);

            for (int i = 0; i < 22; i++)
            {
                // Add a new Default Driver
                Driver.Add(new DriverModel());
                Driver[i].DriverName = "Placeholder";
                Driver[i].DriverIndex = i + 1;
            }

            UDPC.OnLapDataReceive += UDPC_OnLapDataReceive;
            UDPC.OnParticipantsDataReceive += UDPC_OnParticipantsDataReceive;
            UDPC.OnCarDamageDataReceive += UDPC_OnCarDamageDataReceive;
            UDPC.OnCarStatusDataReceive += UDPC_OnCarStatusDataReceive;
        }

        private void UDPC_OnParticipantsDataReceive(PacketParticipantsData packet)
        {
            

            for (int i = 0; i < 22; i++)
            {
                var participant = packet.m_participants[i];

                Driver[i].TeamID = participant.m_teamId;
                Driver[i].TeamName = Regex.Replace(participant.m_teamId.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
                Driver[i].raceNumber = participant.m_raceNumber;
                Driver[i].AI = participant.m_aiControlled; 
                
                Driver[i].DriverName = Regex.Replace(participant.m_driverId.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();

            }
        }

        private void UDPC_OnLapDataReceive(PacketLapData packet)
        {
            for (int i = 0; i < packet.lapData.Length; i++)
            {
                var lapData = packet.lapData[i];

                Driver[i].DriverStatus = Regex.Replace(lapData.driverStatus.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
                Driver[i].DriverStatusUpdate = lapData.driverStatus;
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
    }
}
