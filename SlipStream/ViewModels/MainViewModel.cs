using SlipStream.Core;
using SlipStream.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlipStream.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public RelayCommand HomeViewCommand { get; set; }
        public HomeViewModel HomeVM { get; set; }

        public RelayCommand DriverViewCommand { get; set; }
        public DriverViewModel DVM { get; set; }

        public RelayCommand RaceControlViewCommand { get; set; }
        public RaceControlViewModel RCVM { get; set; }

        public RelayCommand EngineerViewCommand { get; set; }
        public EngineerViewModel EVM { get; set; }

        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged("CurrentView");
            }
        }

        private object _menuView;
        public object MenuView
        {
            get { return _menuView; }
            set
            {
                _menuView = value;
                OnPropertyChanged("MenuView");
            }
        }

        public MainViewModel()
        {
            HomeVM = new HomeViewModel();
            DVM = DriverViewModel.GetInstance();
            RCVM = RaceControlViewModel.GetInstance();
            EVM = EngineerViewModel.GetInstance();

            CurrentView = HomeVM;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;
            });

            DriverViewCommand = new RelayCommand(o =>
            {
                CurrentView = DVM;
            });

            RaceControlViewCommand = new RelayCommand(o =>
            {
                CurrentView = RCVM;
            });

            EngineerViewCommand = new RelayCommand(o =>
            {
                CurrentView = EVM;
            });

        }

    }
}
