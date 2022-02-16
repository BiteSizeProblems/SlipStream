using SlipStream.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SlipStream.Core.Appendeces;

namespace SlipStream.Models
{
    public class LeaderboardModel : ObservableObject
    {
        private Drivers _DriverID;
        public Drivers DriverID
        {
            get { return _DriverID; }
            set { SetField(ref _DriverID, value, nameof(DriverID)); }
        }
    }
}
