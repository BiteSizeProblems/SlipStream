using SlipStream.Core;
using SlipStream.Views;
using SlipStream.Views.Multi;
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
        // MAIN WINDOW BUTTON COMMANDS

        public RelayCommand DataViewCommand { get; set; }
        public DataViewModel DataVM { get; set; }

            // HOME VIEW
        public RelayCommand HomeViewCommand { get; set; }
        public HomeView HV { get; set; }

        // SESSION INFO VIEW
        public RelayCommand SessionInfoViewCommand { get; set; }
        public SessionInfoDefaultView SIDV { get; set; }

        // LEADERBOARD VIEW
        public RelayCommand LeaderboardViewCommand { get; set; }
        public LeaderboardView LV { get; set; }

            // DRIVER VIEWS
        public RelayCommand DriverDefaultViewCommand { get; set; }
        public DriverDefaultView DDV { get; set; }

        public RelayCommand DriverStrategyViewCommand { get; set; }
        public DriverStrategyView DSV { get; set; }

            // ENGINEER VIEWS
        public RelayCommand EngineerDefaultViewCommand { get; set; }
        public EngineerDefaultView EDV { get; set; }

        public RelayCommand EngineerStrategyViewCommand { get; set; }
        public EngineerStrategyView ESV { get; set; }

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

        public MainViewModel()
        {
            DataVM = DataViewModel.GetInstance();

            HV = new HomeView();

            SIDV = new SessionInfoDefaultView();

            LV = new LeaderboardView();

            DDV = new DriverDefaultView();
            DSV = new DriverStrategyView();

            EDV = new EngineerDefaultView();
            ESV = new EngineerStrategyView();

            // SET CURRENT VIEW

            CurrentView = HV;

            // MENU SINGLE-VIEW COMMANDS

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HV;
            });

            SessionInfoViewCommand = new RelayCommand(o =>
            {
                CurrentView = SIDV;
                SIDV.DataContext = DataVM;
            });

            LeaderboardViewCommand = new RelayCommand(o =>
            {
                CurrentView = LV;
                LV.DataContext = DataVM;
            });

            // MENU MULTI-VIEW COMMANDS

            DriverDefaultViewCommand = new RelayCommand(o =>
            {
                CurrentView = DDV;
                DDV.DataContext = DataVM;
            });

            DriverStrategyViewCommand = new RelayCommand(o =>
            {
                CurrentView = DSV;
                DSV.DataContext = DataVM;
            });

            EngineerDefaultViewCommand = new RelayCommand(o =>
            {
                CurrentView = EDV;
                EDV.DataContext = DataVM;
            });

            EngineerStrategyViewCommand = new RelayCommand(o =>
            {
                CurrentView = ESV;
                ESV.DataContext = DataVM;
            });

        }

    }
}
