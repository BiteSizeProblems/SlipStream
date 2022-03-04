using SlipStream.Models;
using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Data;
using SlipStream.Structs;

namespace SlipStream.ViewModels
{
    public class LeaderboardViewModel : BaseViewModel
    {
        // === BEGINING OF MODULE SETUP ===
        // === Singleton Instance with Thread Saftey ===
        private static LeaderboardViewModel _instance = null;
        private static object _singletonLock = new object();

        public static LeaderboardViewModel GetInstance()
        {
            lock (_singletonLock)
            {
                if (_instance == null) { _instance = new LeaderboardViewModel(); }
                return _instance;
            }
        }
        // === END OF MODULE SETUP ===

        private TimeSpan sessionFastestLap;
        public TimeSpan SessionFastestLap
        {
            get { return sessionFastestLap; }
            set { SetField(ref sessionFastestLap, value, nameof(SessionFastestLap)); }
        }

        // Create a observable collection of DriverData
        public ObservableCollection<DriverData> DriverArr { get; set; }
        private object _driverArrLock = new object();


        // NOTE:
        // In order to following singleton design patern constructor should be private
        // that way the only way to receive a LeaderboardViewModel instance is through getInstance();
        private LeaderboardViewModel() : base()
        {

            DriverArr = new ObservableCollection<DriverData>();

            // thread safety
            BindingOperations.EnableCollectionSynchronization(DriverArr, _driverArrLock);

            // Could be UPTO 22 participants
            // Fill the Array up
            for (int i = 0; i < 22; i++)
            {
                // Add a new Default Driver
                DriverArr.Add(new DriverData());
            }

            UDPC.OnLapDataReceive += UDPC_OnLapDataReceive;
            UDPC.OnParticipantsDataReceive += UDPC_OnParticipantDataReceive;
            UDPC.OnEventDataReceive += UDPC_OnEventDataReceive;
            
        }

        

        private void UDPC_OnLapDataReceive(PacketLapData packet)
        {
            // Loop through the participants the game is giving us
            for (int i = 0; i < packet.lapData.Length; i++)
            {
                var lapData = packet.lapData[i];
                // Update it in the array
                DriverArr[i].CurrentLapTime = lapData.currentLapTimeInMS;
                DriverArr[i].CarPosition = lapData.carPosition;
                DriverArr[i].LastLapTime = TimeSpan.FromMilliseconds(lapData.lastLapTimeInMS);
                DriverArr[i].BestLapSector1TimeInMS = TimeSpan.FromMilliseconds(lapData.sector1TimeInMS);
                DriverArr[i].BestSector1Time = TimeSpan.FromMilliseconds(lapData.sector1TimeInMS).ToString(@"mm\:ss\.fff");
                DriverArr[i].BestLapSector2TimeInMS = TimeSpan.FromMilliseconds(lapData.sector2TimeInMS);
                //DriverArr[i].BestLapSector3TimeInMS = $"{TimeSpan.FromSeconds(lapData.bestOverallSector3LapNum)}";
                DriverArr[i].CurrentLapNum = lapData.currentLapNum -1;
                DriverArr[i].DriverStatus = Regex.Replace(lapData.driverStatus.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
            }
        }

        private void UDPC_OnParticipantDataReceive(PacketParticipantsData packet)
        {
            // Loop through the participants the game is giving us
            for (int i=0; i < packet.m_participants.Length; i++)
            {
                var participant = packet.m_participants[i];
                // Update them in the array
                //DriverArr[i].DriverID = participant.driverId;
               DriverArr[i].DriverName = Regex.Replace(participant.m_driverId.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
                DriverArr[i].TeamName = Regex.Replace(participant.m_teamId.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
                DriverArr[i].raceNumber = participant.m_raceNumber;         
            }
        }

        private void UDPC_OnEventDataReceive(PacketEventData packet)
        {
            SessionFastestLap = TimeSpan.FromMilliseconds(packet.m_eventDetails.fastestLap.lapTime);
        }
    }
}
