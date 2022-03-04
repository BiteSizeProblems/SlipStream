using SlipStream.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using static SlipStream.Structs.Appendeces;

namespace SlipStream.Models
{
    // MODEL 
    public class DriverModel : ObservableObject
    {
        private Drivers _driverID;
        public Drivers DriverID
        {
            get { return _driverID; }
            set { SetField(ref _driverID, value, nameof(DriverID)); }
        }

        private string _driverName;
        public string DriverName
        {
            get { return _driverName; }
            set { SetField(ref _driverName, value, nameof(DriverName)); }
        }

        private string _driverTag;
        public string DriverTag
        {
            get { return _driverTag; }
            set { SetField(ref _driverTag, value, nameof(DriverTag)); }
        }

        private byte ai;
        public byte AI
        {
            get { return ai; }
            set { SetField(ref ai, value, nameof(AI)); }
        }

        private Teams _teamID;
        public Teams TeamID
        {
            get { return _teamID; }
            set
            {
                SetField(ref _teamID, value, nameof(TeamID));

                if (this.TeamID != value)
                {
                    this._teamID = value;
                    switch (this._teamID)
                    {
                        default:
                            this.TeamColor = new SolidColorBrush(Color.FromRgb(255, 0, 255));
                            break;
                        case Teams.Mercedes:
                        case Teams.Mercedes2020:
                            this.TeamColor = new SolidColorBrush(Color.FromRgb(0, 210, 90));
                            break;
                        case Teams.Ferrari:
                        case Teams.Ferrari2020:
                            this.TeamColor = new SolidColorBrush(Color.FromRgb(220, 0, 0));
                            break;
                        case Teams.RedBullRacing:
                        case Teams.RedBull2020:
                            this.TeamColor = new SolidColorBrush(Color.FromRgb(6, 0, 239));
                            break;
                        case Teams.Alpine:
                        case Teams.Renault2020:
                            this.TeamColor = new SolidColorBrush(Color.FromRgb(0, 144, 255));
                            break;
                        case Teams.Haas:
                        case Teams.Haas2020:
                            this.TeamColor = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                            break;
                        case Teams.AstonMartin:
                        case Teams.RacingPoint2020:
                            this.TeamColor = new SolidColorBrush(Color.FromRgb(0, 111, 98));
                            break;
                        case Teams.AlphaTauri:
                        case Teams.AlphaTauri2020:
                            this.TeamColor = new SolidColorBrush(Color.FromRgb(43, 69, 98));
                            break;
                        case Teams.Mclaren:
                        case Teams.McLaren2020:
                            this.TeamColor = new SolidColorBrush(Color.FromRgb(255, 135, 0));
                            break;
                        case Teams.AlfaRomeo:
                        case Teams.AlfaRomeo2020:
                            this.TeamColor = new SolidColorBrush(Color.FromRgb(144, 0, 0));
                            break;
                        case Teams.Williams:
                        case Teams.Williams2020:
                            this.TeamColor = new SolidColorBrush(Color.FromRgb(0, 90, 255));
                            break;
                    }
                }
            }
        }

        private string _teamName;
        public string TeamName
        {
            get { return _teamName; }
            set { SetField(ref _teamName, value, nameof(TeamName)); }
        }

        private SolidColorBrush _teamColor;
        public SolidColorBrush TeamColor
        {
            get
            {
                return this._teamColor;
            }
            set
            {
                if (value != this.TeamColor)
                {
                    this._teamColor = value;
                    this.OnPropertyChanged("TeamColor");
                }
            }
        }

        private string _teamRect;
        public string TeamRect
        {
            get
            {
                return this._teamRect;
            }
            set
            {
                if (value != this._teamRect)
                {
                    this._teamRect = value;
                    this.OnPropertyChanged("TeamRect");
                }
            }
        }

        private int _raceNumber;
        public int raceNumber
        {
            get { return _raceNumber; }
            set { SetField(ref _raceNumber, value, nameof(raceNumber)); }
        }

        private float _currentLapTime;
        public float CurrentLapTime
        {
            get { return _currentLapTime; }
            set { SetField(ref _currentLapTime, value, nameof(CurrentLapTime)); }
        }

        private string _tireIconSource;
        public string TireIconSource
        {
            get { return _tireIconSource; }
            set { SetField(ref _tireIconSource, value, nameof(TireIconSource)); }
        }

        private DriverStatus _driverStatusUpdate;
        public DriverStatus DriverStatusUpdate
        {
            get { return _driverStatusUpdate; }
            set { SetField(ref _driverStatusUpdate, value, nameof(DriverStatusUpdate)); }
        }

        private string _driverStatusSource;
        public string DriverStatusSource
        {
            get { return _driverStatusSource; }
            set { SetField(ref _driverStatusSource, value, nameof(DriverStatusSource)); }
        }

        private TimeSpan lastLapTime;
        public TimeSpan LastLapTime
        {
            get { return lastLapTime; }
            set
            {
                SetField(ref lastLapTime, value, nameof(LastLapTime));

                if (CurrentLapNum < 1)
                {
                    BestLapTime = LastLapTime;
                }
                else if (LastLapTime < BestLapTime)
                {
                    BestLapTime = value;
                }
            }
        }

        private TimeSpan bestLapTime;
        public TimeSpan BestLapTime
        {
            get { return bestLapTime; }
            set { SetField(ref bestLapTime, value, nameof(BestLapTime)); }
        }

        private TimeSpan bestLapGap;
        public TimeSpan BestLapGap
        {
            get { return bestLapGap; }
            set { SetField(ref bestLapGap, value, nameof(BestLapGap)); }
        }

        private TimeSpan sector1Time;
        public TimeSpan Sector1Time
        {
            get { return sector1Time; }
            set { SetField(ref sector1Time, value, nameof(Sector1Time)); }
        }

        private TimeSpan sector2Time;
        public TimeSpan Sector2Time
        {
            get { return sector2Time; }
            set { SetField(ref sector2Time, value, nameof(Sector2Time)); }
        }

        private TimeSpan sector3Time;
        public TimeSpan Sector3Time
        {
            get { return sector3Time; }
            set { SetField(ref sector3Time, value, nameof(Sector3Time)); }
        }

        private int carPosition;
        public int CarPosition
        {
            get { return carPosition; }
            set { SetField(ref carPosition, value, nameof(CarPosition)); }
        }

        private int currentLapNum;
        public int CurrentLapNum
        {
            get { return currentLapNum; }
            set { SetField(ref currentLapNum, value, nameof(CurrentLapNum)); }
        }

        private string driverStatus;
        public string DriverStatus
        {
            get { return driverStatus; }
            set { SetField(ref driverStatus, value, nameof(DriverStatus)); }
        }

        private VisualTireCompounds visualTireCompound;
        public VisualTireCompounds VisualTireCompound
        {
            get { return visualTireCompound; }
            set { SetField(ref visualTireCompound, value, nameof(VisualTireCompound)); }
        }

        private string numTireStints;
        public string NumTireStints
        {
            get { return numTireStints; }
            set { SetField(ref numTireStints, value, nameof(NumTireStints)); }
        }

        private sbyte _positionChange;
        public sbyte PositionChange
        {
            get { return _positionChange; }
            set { SetField(ref _positionChange, value, nameof(PositionChange)); }
        }

        // Args CTOR
        public DriverModel(int c, Drivers d, Teams t, int r)
        {
            this.CarPosition = c;
            this.DriverID = d;
            this.TeamID = t;
            this.raceNumber = r;
        }

        // NoArgs CTOR
        public DriverModel()
        {
            this.DriverID = Drivers.Unknown;
            this.TeamID = Teams.Unknown;
            this.CurrentLapTime = 0;
        }
    }
}
