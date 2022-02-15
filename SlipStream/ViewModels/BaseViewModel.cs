using SlipStream.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlipStream.ViewModels
{
    public abstract class BaseViewModel : ObservableObject
    {
        protected UDPConnection UDPC = UDPConnection.GetInstance();
    }
}
