using SlipStream.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using static SlipStream.Structs.Appendeces;
using static SlipStream.Structs.Enums;

namespace SlipStream.Models
{
    // MODEL 
    public class DriverModel : ObservableObject                         // 22 SIZE ARRAYS ONLY!
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

        // INDEXING

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

        // DRIVER INFO

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

        private byte ai;
        public byte AI
        {
            get { return ai; }
            set { SetField(ref ai, value, nameof(AI)); }
        }

        // PENALTIES & WARNINGS

        private int _warnings;
        public int Warnings
        {
            get { return _warnings; }
            set { SetField(ref _warnings, value, nameof(Warnings)); }
        }

        private int _penalties;
        public int Penalties
        {
            get { return _penalties; }
            set { SetField(ref _penalties, value, nameof(Penalties)); }
        }

        // TEAM INFO
        private Teams _teamID;
        public Teams TeamID
        {
            get { return _teamID; }
            set { SetField(ref _teamID, value, nameof(TeamID)); }
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

        // CAR INFO
        private int _raceNumber;
        public int raceNumber
        {
            get { return _raceNumber; }
            set { SetField(ref _raceNumber, value, nameof(raceNumber)); }
        }

        private string _vehicleFlag;
        public string VehicleFlag
        {
            get { return _vehicleFlag; }
            set { SetField(ref _vehicleFlag, value, nameof(VehicleFlag)); }
        }

        // LAP / SECTOR DATA, TIMES & GAPS

        private int currentLapNum;
        public int CurrentLapNum
        {
            get { return currentLapNum; }
            set { SetField(ref currentLapNum, value, nameof(CurrentLapNum)); }
        }

        private TimeSpan _currentLapTime;
        public TimeSpan CurrentLapTime
        {
            get { return _currentLapTime; }
            set { SetField(ref _currentLapTime, value, nameof(CurrentLapTime)); }
        }

        private TimeSpan lastLapTime;
        public TimeSpan LastLapTime
        {
            get { return lastLapTime; }
            set
            {
                SetField(ref lastLapTime, value, nameof(LastLapTime));

                if(LastS1 != TimeSpan.FromSeconds(0) && LastS2 != TimeSpan.FromSeconds(0) && LastLapTime != TimeSpan.FromSeconds(0) && DriverStatus != DriverStatus.InGarage)
                {
                    LastS3 = LastLapTime - (LastS1 + LastS2);
                }

                if (CurrentLapNum < 1)
                {
                    BestLapTime = LastLapTime;
                    BestS3 = LastS3;
                }
                else if (LastLapTime < BestLapTime)
                {
                    BestLapTime = value;
                    BestS3 = LastS3;
                }
            }
        }

        private TimeSpan bestLapTime;
        public TimeSpan BestLapTime
        {
            get { return bestLapTime; }
            set { SetField(ref bestLapTime, value, nameof(BestLapTime)); }
        }

        // LAST SECTOR TIMES
        private TimeSpan _lastS1;
        public TimeSpan LastS1
        {
            get { return _lastS1; }
            set { SetField(ref _lastS1, value, nameof(LastS1)); }
        }

        private TimeSpan _lastS2;
        public TimeSpan LastS2
        {
            get { return _lastS2; }
            set { SetField(ref _lastS2, value, nameof(LastS2)); }
        }

        private TimeSpan _lastS3;
        public TimeSpan LastS3
        {
            get { return _lastS3; }
            set {SetField(ref _lastS3, value, nameof(LastS3));}
        }

        // BEST SECTOR TIMES
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

        // DISPLAYED SECTOR TIMES
        private TimeSpan _s1Display;
        public TimeSpan S1Display
        {
            get { return _s1Display; }
            set { SetField(ref _s1Display, value, nameof(S1Display)); }
        }

        private TimeSpan _s2Display;
        public TimeSpan S2Display
        {
            get { return _s2Display; }
            set { SetField(ref _s2Display, value, nameof(S2Display)); }
        }

        private TimeSpan _s3Display;
        public TimeSpan S3Display
        {
            get { return _s3Display; }
            set { SetField(ref _s3Display, value, nameof(S3Display)); }
        }

        // GAPS & INTERVALS

            // DELTA TO FASTEST LAP
        private TimeSpan bestLapDelta; // Delta of driver's best lap time to the fastest overall lap time in a session.
        public TimeSpan BestLapDelta
        {
            get { return bestLapDelta; }
            set { SetField(ref bestLapDelta, value, nameof(BestLapDelta)); }
        }

            // INTERVAL TO DRIVER AHEAD
        private TimeSpan _raceInterval; // Interval to driver ahead (Race Only).
        public TimeSpan RaceInterval
        {
            get { return _raceInterval; }
            set { SetField(ref _raceInterval, value, nameof(RaceInterval)); }
        }

            // INTERVAL TO LEADER
        private TimeSpan _raceIntervalLeader;  // Interval to race leader (Race Only).
        public TimeSpan RaceIntervalLeader
        {
            get { return _raceIntervalLeader; }
            set { SetField(ref _raceIntervalLeader, value, nameof(RaceIntervalLeader)); }
        }

            // DELTA DISPLAYED
        private TimeSpan _selectedDelta;  // Gap / Interval selected by user.
        public TimeSpan SelectedDelta
        {
            get { return _selectedDelta; }
            set { SetField(ref _selectedDelta, value, nameof(SelectedDelta)); }
        }

        // CAR / DRIVER STATUS
        private string _driverStatusSource;
        public string DriverStatusSource
        {
            get { return _driverStatusSource; }
            set { SetField(ref _driverStatusSource, value, nameof(DriverStatusSource)); }
        }

        private DriverStatus driverStatus;
        public DriverStatus DriverStatus
        {
            get { return driverStatus; }
            set { SetField(ref driverStatus, value, nameof(DriverStatus)); }
        }

        private ResultStatus _resultStatus;
        public ResultStatus ResultStatus
        {
            get { return _resultStatus; }
            set { SetField(ref _resultStatus, value, nameof(ResultStatus)); }
        }

        private string _actualDriverStatus;
        public string ActualDriverStatus
        {
            get { return _actualDriverStatus; }
            set { SetField(ref _actualDriverStatus, value, nameof(ActualDriverStatus)); }
        }

        // CAR POSITIONS

        private int carPosition;
        public int CarPosition
        {
            get { return carPosition; }
            set { SetField(ref carPosition, value, nameof(CarPosition)); }
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

        // PIT STOP DATA

        private uint _pitWindowIdeal;
        public uint PitWindowIdeal
        {
            get { return _pitWindowIdeal; }
            set { SetField(ref _pitWindowIdeal, value, nameof(PitWindowIdeal)); }
        }

        private uint _pitWindowLate;
        public uint PitWindowLate
        {
            get { return _pitWindowLate; }
            set { SetField(ref _pitWindowLate, value, nameof(PitWindowLate)); }
        }

        private uint _pitRejoin;
        public uint PitRejoin
        {
            get { return _pitRejoin; }
            set { SetField(ref _pitRejoin, value, nameof(PitRejoin)); }
        }

        private PitStatus _pitStatus;
        public PitStatus PitStatus
        {
            get { return _pitStatus; }
            set { SetField(ref _pitStatus, value, nameof(PitStatus)); }
        }

        // TIRE DATA

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

        private VisualTireCompounds visualTireCompound;
        public VisualTireCompounds VisualTireCompound
        {
            get { return visualTireCompound; }
            set 
            { 
                SetField(ref visualTireCompound, value, nameof(VisualTireCompound));

                // TIRE SHORT
                switch (VisualTireCompound)
                {
                    case VisualTireCompounds.Soft:
                        TireCompoundShort = "S";
                        break;
                    case VisualTireCompounds.Medium:
                        TireCompoundShort = "M";
                        break;
                    case VisualTireCompounds.Hard:
                        TireCompoundShort = "H";
                        break;
                    case VisualTireCompounds.Inter:
                        TireCompoundShort = "I";
                        break;
                    case VisualTireCompounds.Wet:
                        TireCompoundShort = "W";
                        break;
                }
            }
        }

        private string _tireCompoundShort;
        public string TireCompoundShort
        {
            get { return _tireCompoundShort; }
            set { SetField(ref _tireCompoundShort, value, nameof(TireCompoundShort)); }
        }

        private int _tireAge;
        public int TireAge
        {
            get { return _tireAge; }
            set { SetField(ref _tireAge, value, nameof(TireAge)); }
        }

        private string _tireIconSource;
        public string TireIconSource
        {
            get { return _tireIconSource; }
            set { SetField(ref _tireIconSource, value, nameof(TireIconSource)); }
        }

        // ENGINE & ERS DATA

        private string _fuelMix;
        public string FuelMix
        {
            get { return _fuelMix; }
            set { SetField(ref _fuelMix, value, nameof(FuelMix)); }
        }

        private ErsDeployMode _ersDeployMode;
        public ErsDeployMode ErsDeployMode
        {
            get { return _ersDeployMode; }
            set { SetField(ref _ersDeployMode, value, nameof(ErsDeployMode)); }
        }

        private int _ersRemaining;
        public int ErsRemaining
        {
            get { return _ersRemaining; }
            set { SetField(ref _ersRemaining, value, nameof(ErsRemaining)); }
        }

        private float _ersUsed;
        public float ErsUsed
        {
            get { return _ersUsed; }
            set { SetField(ref _ersUsed, value, nameof(ErsUsed)); }
        }

        // TIRE WEAR
        private float _flTireWear;
        public float FLTireWear
        {
            get { return _flTireWear; }
            set { SetField(ref _flTireWear, value, nameof(FLTireWear)); }
        }
        private float _frTireWear;
        public float FRTireWear
        {
            get { return _frTireWear; }
            set { SetField(ref _frTireWear, value, nameof(FRTireWear)); }
        }
        private float _rlTireWear;
        public float RLTireWear
        {
            get { return _rlTireWear; }
            set { SetField(ref _rlTireWear, value, nameof(RLTireWear)); }
        }
        private float _rrTireWear;
        public float RRTireWear
        {
            get { return _rrTireWear; }
            set { SetField(ref _rrTireWear, value, nameof(RRTireWear)); }
        }
        private float _tireWear;
        public float TireWear
        {
            get { return _tireWear; }
            set { SetField(ref _tireWear, value, nameof(TireWear)); }
        }

        // TIRE WEAR COLORS
        private string _rlTireWearColor;
        public string RLTireWearColor
        {
            get { return _rlTireWearColor; }
            set { SetField(ref _rlTireWearColor, value, nameof(RLTireWearColor)); }
        }
        private string _rrTireWearColor;
        public string RRTireWearColor
        {
            get { return _rrTireWearColor; }
            set { SetField(ref _rrTireWearColor, value, nameof(RRTireWearColor)); }
        }
        private string _flTireWearColor;
        public string FLTireWearColor
        {
            get { return _flTireWearColor; }
            set { SetField(ref _flTireWearColor, value, nameof(FLTireWearColor)); }
        }
        private string _frTireWearColor;
        public string FRTireWearColor
        {
            get { return _frTireWearColor; }
            set { SetField(ref _frTireWearColor, value, nameof(FRTireWearColor)); }
        }

        // SESSION HISTORY PACKET
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

        private TimeSpan _lapTimeHist;
        public TimeSpan LapTimeHist
        {
            get { return _lapTimeHist; }
            set { SetField(ref _lapTimeHist, value, nameof(LapTimeHist)); }
        }

        private TimeSpan _s1TimeHist;
        public TimeSpan S1TimeHist
        {
            get { return _s1TimeHist; }
            set { SetField(ref _s1TimeHist, value, nameof(S1TimeHist)); }
        }

        private TimeSpan _s2TimeHist;
        public TimeSpan S2TimeHist
        {
            get { return _s2TimeHist; }
            set { SetField(ref _s2TimeHist, value, nameof(S2TimeHist)); }
        }

        private TimeSpan _s3TimeHist;
        public TimeSpan S3TimeHist
        {
            get { return _s3TimeHist; }
            set { SetField(ref _s3TimeHist, value, nameof(S3TimeHist)); }
        }

        private uint _lapValidHist;
        public uint LapValidHist
        {
            get { return _lapValidHist; }
            set { SetField(ref _lapValidHist, value, nameof(LapValidHist)); }
        }
    }
}
