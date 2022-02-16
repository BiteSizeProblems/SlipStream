using SlipStream.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlipStream.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public RelayCommand HomeViewCommand { get; set; }
        public HomeViewModel HomeVM { get; set; }

        public RelayCommand LeaderboardViewCommand { get; set; }
        public LeaderboardViewModel LVM { get; set; }

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
            HomeVM = new HomeViewModel();
            LVM = new LeaderboardViewModel();

            CurrentView = HomeVM;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;
            });

            LeaderboardViewCommand = new RelayCommand(o =>
            {
                CurrentView = LVM;
            });
        }

    }
}
