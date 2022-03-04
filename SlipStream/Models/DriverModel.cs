using SlipStream.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        // Args CTOR
        public DriverModel(int c, Drivers d, Teams t, int r)
        {
            this.CarPosition = c;
            this.DriverID = d;
            this.TeamID = t;
            this.raceNumber = r;
        }

        // Create a observable collection of DriverModel
        public ObservableCollection<DriverModel> Driver { get; set; }
        private object _driverLock = new object();

        // NoArgs CTOR
        public DriverModel()
        {
            this.DriverID = Drivers.Unknown;
            this.TeamID = Teams.Unknown;
        }

        // Driver Index

        private int _driverIndex;
        public int DriverIndex
        {
            get { return _driverIndex; }
            set { SetField(ref _driverIndex, value, nameof(DriverIndex)); }
        }

        private int _maxIndex;
        public int MaxIndex
        {
            get { return _maxIndex; }
            set { SetField(ref _maxIndex, value, nameof(MaxIndex)); }
        }

        // Driver Participant Data

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

        private TimeSpan _currentLapTime;
        public TimeSpan CurrentLapTime
        {
            get { return _currentLapTime; }
            set { SetField(ref _currentLapTime, value, nameof(CurrentLapTime)); }
        }

        // Driver Car Data

        private int _tireAge;
        public int TireAge
        {
            get { return _tireAge; }
            set { SetField(ref _tireAge, value, nameof(TireAge)); }
        }

        private string _fuelMix;
        public string FuelMix
        {
            get { return _fuelMix; }
            set { SetField(ref _fuelMix, value, nameof(FuelMix)); }
        }

        private string _ersDeployMode;
        public string ErsDeployMode
        {
            get { return _ersDeployMode; }
            set { SetField(ref _ersDeployMode, value, nameof(ErsDeployMode)); }
        }

        private string _vehicleFlag;
        public string VehicleFlag
        {
            get { return _vehicleFlag; }
            set { SetField(ref _vehicleFlag, value, nameof(VehicleFlag)); }
        }

        private string _ersRemaining;
        public string ErsRemaining
        {
            get { return _ersRemaining; }
            set { SetField(ref _ersRemaining, value, nameof(ErsRemaining)); }
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
            set 
            { 
                SetField(ref sector1Time, value, nameof(Sector1Time));
                
                if (CurrentLapNum < 1)
                {
                    BestS1 = value;
                }
                else if (Sector1Time < BestS1)
                {
                    BestS1 = value;
                }
            }
        }

        private TimeSpan sector2Time;
        public TimeSpan Sector2Time
        {
            get { return sector2Time; }
            set
            {
                SetField(ref sector2Time, value, nameof(Sector2Time));

                if (CurrentLapNum < 1)
                {
                    BestS2 = value;
                }
                else if (Sector2Time < BestS2)
                {
                    BestS2 = value;
                }
            }
        }

        private TimeSpan sector3Time;
        public TimeSpan Sector3Time
        {
            get { return sector3Time; }
            set
            {
                SetField(ref sector3Time, value, nameof(Sector3Time));

                if (CurrentLapNum < 1)
                {
                    BestS3 = value;
                }
                else if (Sector3Time < BestS3)
                {
                    BestS3 = value;
                }
            }
        }

        private TimeSpan bestS1;
        public TimeSpan BestS1
        {
            get { return bestS1; }
            set { SetField(ref bestS1, value, nameof(BestS1)); }
        }

        private TimeSpan bestS2;
        public TimeSpan BestS2
        {
            get { return bestS2; }
            set { SetField(ref bestS2, value, nameof(BestS2)); }
        }

        private TimeSpan bestS3;
        public TimeSpan BestS3
        {
            get { return bestS3; }
            set { SetField(ref bestS3, value, nameof(BestS3)); }
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

        private int _gridPosition;
        public int GridPosition
        {
            get { return _gridPosition; }
            set { SetField(ref _gridPosition, value, nameof(GridPosition)); }
        }

        private sbyte _positionChange;
        public sbyte PositionChange
        {
            get { return _positionChange; }
            set { SetField(ref _positionChange, value, nameof(PositionChange)); }
        }

        private string _positionChangeIcon;
        public string PositionChangeIcon
        {
            get { return _positionChangeIcon; }
            set { SetField(ref _positionChangeIcon, value, nameof(PositionChangeIcon)); }
        }

        // Packet Session History Data

        private int _lapNum;
        public int lapNum
        {
            get { return _lapNum; }
            set { SetField(ref _lapNum, value, nameof(lapNum)); }
        }

        private uint _carIdx;
        public uint CarIdx
        {
            get { return _carIdx; }
            set { SetField(ref _carIdx, value, nameof(CarIdx)); }
        }

        private uint _numLaps;
        public uint NumLaps
        {
            get { return _numLaps; }
            set { SetField(ref _numLaps, value, nameof(NumLaps)); }
        }

        private uint _numTireStints;
        public uint NumTireStints
        {
            get { return _numTireStints; }
            set { SetField(ref _numTireStints, value, nameof(NumTireStints)); }
        }

        private uint _bestLapNum;
        public uint BestLapNum
        {
            get { return _bestLapNum; }
            set { SetField(ref _bestLapNum, value, nameof(BestLapNum)); }
        }

        private uint _bestS1Num;
        public uint BestS1Num
        {
            get { return _bestS1Num; }
            set { SetField(ref _bestS1Num, value, nameof(BestS1Num)); }
        }

        private uint _bestS2Num;
        public uint BestS2Num
        {
            get { return _bestS2Num; }
            set { SetField(ref _bestS2Num, value, nameof(BestS2Num)); }
        }

        private uint _bestS3Num;
        public uint BestS3Num
        {
            get { return _bestS3Num; }
            set { SetField(ref _bestS3Num, value, nameof(BestS3Num)); }
        }

        // Lap History Data

        private uint _lapTime;
        public uint LapTime
        {
            get { return _lapTime; }
            set { SetField(ref _lapTime, value, nameof(LapTime)); }
        }

        private TimeSpan _s1Time;
        public TimeSpan S1Time
        {
            get { return _s1Time; }
            set { SetField(ref _s1Time, value, nameof(S1Time)); }
        }

        private TimeSpan _s2Time;
        public TimeSpan S2Time
        {
            get { return _s2Time; }
            set { SetField(ref _s2Time, value, nameof(S2Time)); }
        }

        private TimeSpan _s3Time;
        public TimeSpan S3Time
        {
            get { return _s3Time; }
            set { SetField(ref _s3Time, value, nameof(S3Time)); }
        }

        private uint _lapValid;
        public uint LapValid
        {
            get { return _lapValid; }
            set { SetField(ref _lapValid, value, nameof(LapValid)); }
        }

        // Tire History Data

        private uint _endLap;
        public uint EndLap
        {
            get { return _endLap; }
            set { SetField(ref _endLap, value, nameof(EndLap)); }
        }

        private TyreCompounds _tireActualCompound;
        public TyreCompounds TireActualCompound
        {
            get { return _tireActualCompound; }
            set { SetField(ref _tireActualCompound, value, nameof(TireActualCompound)); }
        }

        private TyreCompounds _tireVisualCompound;
        public TyreCompounds TireVisualCompound
        {
            get { return _tireVisualCompound; }
            set { SetField(ref _tireVisualCompound, value, nameof(TireVisualCompound)); }
        }

        
    }
}
