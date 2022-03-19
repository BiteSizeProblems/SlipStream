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

            // RACE CONTROL VIEWS
        public RelayCommand RaceControlDefaultViewCommand { get; set; }
        public RaceControlDefaultView RCDV { get; set; }

        public RelayCommand RaceControlWeatherViewCommand { get; set; }
        public RaceControlWeatherView RCWV { get; set; }

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

            LV = new LeaderboardView();

            DDV = new DriverDefaultView();
            DSV = new DriverStrategyView();

            EDV = new EngineerDefaultView();
            ESV = new EngineerStrategyView();

            RCDV = new RaceControlDefaultView();
            RCWV = new RaceControlWeatherView();

            // SET CURRENT VIEW

            CurrentView = HV;

            // MENU SINGLE-VIEW COMMANDS

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HV;
            });

            LeaderboardViewCommand = new RelayCommand(o =>
            {
                CurrentView = LV;
            });

            // MENU MULTI-VIEW COMMANDS

            DriverDefaultViewCommand = new RelayCommand(o =>
            {
                CurrentView = DDV;
            });

            DriverStrategyViewCommand = new RelayCommand(o =>
            {
                CurrentView = DSV;
            });


            EngineerDefaultViewCommand = new RelayCommand(o =>
            {
                CurrentView = EDV;
            });

            EngineerStrategyViewCommand = new RelayCommand(o =>
            {
                CurrentView = ESV;
            });


            RaceControlDefaultViewCommand = new RelayCommand(o =>
            {
                CurrentView = RCDV;
            });

            RaceControlWeatherViewCommand = new RelayCommand(o =>
            {
                CurrentView = RCWV;
            });

        }

    }
}
