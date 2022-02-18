using SlipStream.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public RelayCommand DriverViewSummaryCommand { get; set; }
        public DriverViewModel DVM { get; set; }

        private object _currentDriverView;
        public object CurrentDriverView
        {
            get { return _currentDriverView; }
            set
            {
                _currentDriverView = value;
                OnPropertyChanged("CurrentDriverView");
            }
        }

        public DriverViewModel() : base()
        {
            CurrentDriverView = DVM;

            DriverViewSummaryCommand = new RelayCommand(o =>
            {
                CurrentDriverView = DVM;
            });
        }
    }
}
