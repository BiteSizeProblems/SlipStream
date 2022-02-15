using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlipStream.Models;

namespace SlipStream.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {

        // === BEGINING OF MODULE SETUP ===
        // === Singleton Instance with Thread Saftey ===
        private static HomeViewModel _instance = null;
        private static object _singletonLock = new object();
        public static HomeViewModel GetInstance()
        {
            lock (_singletonLock)
            {
                if (_instance == null) { _instance = new HomeViewModel(); }
                return _instance;
            }
        }
        // === END OF MODULE SETUP ===

        private string _ConnectionStatus;
        public string ConnectionStatus
        {
            get { return _ConnectionStatus; }
            set { SetField(ref _ConnectionStatus, value, nameof(ConnectionStatus)); }
        }

        public string Test1 { get; set; }

        public HomeViewModel() : base()
        {
            Test1 = "Test1";

            UDPC.OnSessionDataReceive += UDPC_OnSessionDataReceive;
        }

        private void UDPC_OnSessionDataReceive(PacketSessionData packet)
        {
            ConnectionStatus = "Connected";
        }
    }
}
