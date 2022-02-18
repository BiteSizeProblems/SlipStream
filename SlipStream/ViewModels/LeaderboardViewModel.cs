using SlipStream.Models;
using SlipStream.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using static SlipStream.Core.Appendeces;
using SlipStream.Core;
using System.Diagnostics;
using System.Windows.Data;

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


        // When constructing this model we want to...
        // INIT driverArr
        // Enable multithreading
        // Fill it with default values
        // "Subscribe" to OnParticipantsDataReceive

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
            for (int i=0; i < packet.participants.Length; i++)
            {
                var participant = packet.participants[i];
                // Update them in the array
                //DriverArr[i].DriverID = participant.driverId;
                DriverArr[i].DriverName = Regex.Replace(participant.driverId.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
                DriverArr[i].TeamName = Regex.Replace(participant.teamId.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
                DriverArr[i].raceNumber = participant.raceNumber;
                

            }
        }

        private void UDPC_OnEventDataReceive(PacketEventData packet)
        {
            SessionFastestLap = TimeSpan.FromMilliseconds(packet.eventDataDetails.fastestLap.lapTime);
        }


        // MODEL 
        public class DriverData : ObservableObject
        {
            private Drivers _driverID;
            public Drivers DriverID
            {
                get { return _driverID; }
                set { SetField(ref _driverID, value, nameof(DriverID)); }
            }
            private string _driverName;
            public string DriverName
            {
                get { return _driverName; }
                set { SetField(ref _driverName, value, nameof(DriverName)); }
            }
            private Teams _teamID;
            public Teams TeamID
            {
                get { return _teamID; }
                set { SetField(ref _teamID, value, nameof(TeamID)); }
            }
            private string _teamName;
            public string TeamName
            {
                get { return _teamName; }
                set { SetField(ref _teamName, value, nameof(TeamName)); }
            }
            private int _raceNumber;
            public int raceNumber
            {
                get { return _raceNumber; }
                set { SetField(ref _raceNumber, value, nameof(raceNumber)); }
            }

            private float _currentLapTime;
            public float CurrentLapTime
            {
                get { return _currentLapTime; }
                set { SetField(ref _currentLapTime, value, nameof(CurrentLapTime)); }
            }

            private TimeSpan lastLapTime;
            public TimeSpan LastLapTime
            {
                get { return lastLapTime; }
                set 
                {
                    SetField(ref lastLapTime, value, nameof(LastLapTime));

                    if (CurrentLapNum == 1)
                    {
                        bestLapTime = LastLapTime;
                    }
                    else if (CurrentLapNum > 1 && LastLapTime < BestLapTime)
                    {
                        bestLapTime = LastLapTime;
                    }
                    else
                    {
                        bestLapTime = LastLapTime;
                    }
                     
                }
            }

            private TimeSpan bestLapTime;
            public TimeSpan BestLapTime
            {
                get { return bestLapTime; }
                set { SetField(ref bestLapTime, value, nameof(BestLapTime));}
            }

            

            public TimeSpan BestLapGap { get; set; }

            private string bestSector1Time;
            public string BestSector1Time
            {
                get { return bestSector1Time; }
                set { SetField(ref bestSector1Time, value, nameof(BestSector1Time)); }
            }

            private TimeSpan bestLapSector1TimeInMS;
            public TimeSpan BestLapSector1TimeInMS
            {
                get { return bestLapSector1TimeInMS; }
                set { SetField(ref bestLapSector1TimeInMS, value, nameof(BestLapSector1TimeInMS)); }
            }

            private TimeSpan bestLapSector2TimeInMS;
            public TimeSpan BestLapSector2TimeInMS
            {
                get { return bestLapSector2TimeInMS; }
                set { SetField(ref bestLapSector2TimeInMS, value, nameof(BestLapSector2TimeInMS)); }
            }

            private string bestLapSector3TimeInMS;
            public string BestLapSector3TimeInMS
            {
                get { return bestLapSector3TimeInMS; }
                set { SetField(ref bestLapSector3TimeInMS, value, nameof(BestLapSector3TimeInMS)); }
            }

            private byte carPosition;
            public byte CarPosition
            {
                get { return carPosition; }
                set { SetField(ref carPosition, value, nameof(CarPosition)); }
            }

            private int currentLapNum;
            public int CurrentLapNum
            {
                get { return currentLapNum; }
                set { SetField(ref currentLapNum, value, nameof(CurrentLapNum)); }
            }

            private string driverStatus;
            public string DriverStatus
            {
                get { return driverStatus; }
                set { SetField(ref driverStatus, value, nameof(DriverStatus)); }
            }

            // Args CTOR
            public DriverData(Drivers d, Teams t, int r)
            {
                this.DriverID = d;
                this.TeamID = t;
                this.raceNumber = r;
            }

            // NoArgs CTOR
            public DriverData()
            {
                this.DriverID = Drivers.Unknown;
                this.TeamID = Teams.Unknown;
                this.CurrentLapTime = 0;
            }
        }

    }
}
