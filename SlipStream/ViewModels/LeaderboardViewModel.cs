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

        private Drivers _DriverID;
        public Drivers DriverID
        {
            get { return _DriverID; }
            set { SetField(ref _DriverID, value, nameof(DriverID)); }
        }

        ObservableCollection<DriverData> DrArr = new ObservableCollection<DriverData>();

        public LeaderboardViewModel() : base()
        {

            UDPC.OnParticipantsDataReceive += UDPC_OnParticipantDataReceive;

            for(int i=0; i<20; i++)
            {
                DrArr.Add(0);
            }

        }

        // TODO: Create a listview for the leaderboard

        

        private void UDPC_OnParticipantDataReceive(PacketParticipantsData packet)
        {
            for(int i=0; i < packet.participants.Length; i++)
            {
                DrArr[i] = packet.participants[i].teamId;
            }
        }

        private void PlayerList(int numberOfPlayers)
        {
            //if (this..Items.Count == 0)
            {
                //var tmp = new ObservableCollection<LeaderboardModel>();

                //for (int i = 0; i < numberOfPlayers; i++)
                {
                    //var data = new LeaderboardModel();
                    //data.ArrayIndex = i;
                    //tmp.Add(data);
                }

                //this.listBox_drivers.ItemsSource = tmp;
            }
        }

        private int _arrayIndex;
        public int ArrayIndex
        {
            get { return _arrayIndex; }
            set
            {
                if (this.ArrayIndex != value)
                {
                    _arrayIndex = value;
                    this.OnPropertyChanged("CarPosition");
                }
            }
        }



        public class DriverData
        {
            Drivers DriverID;
            Teams TeamID;
        }

    }
}
