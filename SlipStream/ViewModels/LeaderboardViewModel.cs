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

            // thread saftey
            BindingOperations.EnableCollectionSynchronization(DriverArr, _driverArrLock);

            // Could be UPTO 22 participants
            // Fill the Array up
            for (int i = 0; i < 22; i++)
            {
                // Add a new Default Driver
                DriverArr.Add(new DriverData());
            }

            UDPC.OnParticipantsDataReceive += UDPC_OnParticipantDataReceive;
            UDPC.OnLapDataReceive += UDPC_OnLapDataReceive;
        }



        private void UDPC_OnLapDataReceive(PacketLapData packet)
        {
            // Loop through the participants the game is giving us
            for (int i = 0; i < packet.lapData.Length; i++)
            {
                var lapData = packet.lapData[i];
                // Update it in the array
                DriverArr[i].CurrentLapTime = lapData.currentLapTimeInMS;
            }
        }

        private void UDPC_OnParticipantDataReceive(PacketParticipantsData packet)
        {
            // Loop through the participants the game is giving us
            for(int i=0; i < packet.participants.Length; i++)
            {
                var participant = packet.participants[i];
                // Update them in the array
                DriverArr[i].DriverID = participant.driverId;
                DriverArr[i].TeamID = participant.teamId;
            }
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
            private Teams _teamID;
            public Teams TeamID
            {
                get { return _teamID; }
                set { SetField(ref _teamID, value, nameof(TeamID)); }
            }

            private float _currentLapTime;
            public float CurrentLapTime
            {
                get { return _currentLapTime; }
                set { SetField(ref _currentLapTime, value, nameof(CurrentLapTime)); }
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
