using SlipStream.Core;
using System;
using System.Collections.ObjectModel;
using static SlipStream.Structs.Appendeces;

namespace SlipStream.Models
{
    // MODEL 
    public class DriverModel : ObservableObject                     // 22 SIZE ARRAYS ONLY!
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
        private object _driverLock = new();

        // NoArgs CTOR
        public DriverModel()
        {
            this.DriverID = Drivers.Unknown;
            this.TeamID = Teams.Unknown;
        }

        // GAME SETTINGS

        private TelemetrySettings _uDPSetting;
        public TelemetrySettings UDPSetting
        {
            get { return _uDPSetting; }
            set { SetField(ref _uDPSetting, value, nameof(UDPSetting)); }
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

        private int _thisCarPosition;
        public int ThisCarPosition
        {
            get { return _thisCarPosition; }
            set { SetField(ref _thisCarPosition, value, nameof(ThisCarPosition)); }
        }

        private int _carAheadPosition;
        public int CarAheadPosition
        {
            get { return _carAheadPosition; }
            set { SetField(ref _carAheadPosition, value, nameof(CarAheadPosition)); }
        }

        // Motion Data
        private float _totalDistance;
        public float TotalDistance
        {
            get { return _totalDistance; }
            set { SetField(ref _totalDistance, value, nameof(TotalDistance)); }
        }

        // SESSION HISTORY

        public byte[] AllLapValid = new byte[100];
        public float[] AllLaptimes = new float[100];
        public float[] AllS1Times = new float[100];
        public float[] AllS2Times = new float[100];
        public float[] AllS3Times = new float[100];

        public string[] EndLap = new string[8];
        public string[] TireActual = new string[8];
        public VisualTireCompounds[] TireVisual = new VisualTireCompounds[8];

        // RANKING
        private int _lastLapRank;
        public int LastLapRank
        {
            get { return _lastLapRank; }
            set { SetField(ref _lastLapRank, value, nameof(LastLapRank)); }
        }

        private int _lastS1Rank;
        public int LastS1Rank
        {
            get { return _lastS1Rank; }
            set { SetField(ref _lastS1Rank, value, nameof(LastS1Rank)); }
        }

        private int _lastS2Rank;
        public int LastS2Rank
        {
            get { return _lastS2Rank; }
            set { SetField(ref _lastS2Rank, value, nameof(LastS2Rank)); }
        }

        private int _lastS3Rank;
        public int LastS3Rank
        {
            get { return _lastS3Rank; }
            set { SetField(ref _lastS3Rank, value, nameof(LastS3Rank)); }
        }

        private int _fastestLapRank;
        public int FastestLapRank
        {
            get { return _fastestLapRank; }
            set { SetField(ref _fastestLapRank, value, nameof(FastestLapRank)); }
        }

        private int _fastestS1Rank;
        public int FastestS1Rank
        {
            get { return _fastestS1Rank; }
            set { SetField(ref _fastestS1Rank, value, nameof(FastestS1Rank)); }
        }

        private int _fastestS2Rank;
        public int FastestS2Rank
        {
            get { return _fastestS2Rank; }
            set { SetField(ref _fastestS2Rank, value, nameof(FastestS2Rank)); }
        }

        private int _fastestS3Rank;
        public int FastestS3Rank
        {
            get { return _fastestS3Rank; }
            set { SetField(ref _fastestS3Rank, value, nameof(FastestS3Rank)); }
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

        private Sectors _currentSector;
        public Sectors CurrentSector
        {
            get { return _currentSector; }
            set { SetField(ref _currentSector, value, nameof(CurrentSector)); }
        }

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
            set { SetField(ref lastLapTime, value, nameof(LastLapTime)); }
        }

        private TimeSpan _bestLapTime;
        public TimeSpan BestLapTime
        {
            get { return _bestLapTime; }
            set { SetField(ref _bestLapTime, value, nameof(BestLapTime)); }
        }

        private Boolean _hasFastestLap;
        public Boolean HasFastestLap
        {
            get { return _hasFastestLap; }
            set { SetField(ref _hasFastestLap, value, nameof(HasFastestLap)); }
        }

        private Boolean _hasFastestS1;
        public Boolean HasFastestS1
        {
            get { return _hasFastestS1; }
            set { SetField(ref _hasFastestS1, value, nameof(HasFastestS1)); }
        }

        private Boolean _hasFastestS2;
        public Boolean HasFastestS2
        {
            get { return _hasFastestS2; }
            set { SetField(ref _hasFastestS2, value, nameof(HasFastestS2)); }
        }

        private Boolean _hasFastestS3;
        public Boolean HasFastestS3
        {
            get { return _hasFastestS3; }
            set { SetField(ref _hasFastestS3, value, nameof(HasFastestS3)); }
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
            set { SetField(ref _lastS3, value, nameof(LastS3)); }
        }

        // ACTIVE LAST SECTOR TIMES
        private TimeSpan _activeLastLap;
        public TimeSpan ActiveLastLap
        {
            get { return _activeLastLap; }
            set { SetField(ref _activeLastLap, value, nameof(ActiveLastLap)); }
        }

        private TimeSpan _activeLastS1;
        public TimeSpan ActiveLastS1
        {
            get { return _activeLastS1; }
            set { SetField(ref _activeLastS1, value, nameof(ActiveLastS1)); }
        }

        private TimeSpan _activeLastS2;
        public TimeSpan ActiveLastS2
        {
            get { return _activeLastS2; }
            set { SetField(ref _activeLastS2, value, nameof(ActiveLastS2)); }
        }

        private TimeSpan _activeLastS3;
        public TimeSpan ActiveLastS3
        {
            get { return _activeLastS3; }
            set { SetField(ref _activeLastS3, value, nameof(ActiveLastS3)); }
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

        // CAR / DRIVER STATUS
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

        // PIT STOP DATA
        private PitStatus _pitStatus;
        public PitStatus PitStatus
        {
            get { return _pitStatus; }
            set { SetField(ref _pitStatus, value, nameof(PitStatus)); }
        }

        private int _numPitstops;
        public int NumPitstops
        {
            get { return _numPitstops; }
            set { SetField(ref _numPitstops, value, nameof(NumPitstops)); }
        }

        private TimeSpan _pitstopTrafficGap;
        public TimeSpan PitstopTrafficGap
        {
            get { return _pitstopTrafficGap; }
            set { SetField(ref _pitstopTrafficGap, value, nameof(PitstopTrafficGap)); }
        }

        // TIRE DATA

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

                switch (VisualTireCompound) // Tire Compound - Shortened
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

        // ENGINE & ERS DATA

        private FuelMix _fuelMix;
        public FuelMix FuelMix
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

        // FINAL CLASSIFICATION DATA

        private TimeSpan _totalRaceTime;
        public TimeSpan TotalRaceTime
        {
            get { return _totalRaceTime; }
            set { SetField(ref _totalRaceTime, value, nameof(TotalRaceTime)); }
        }

        private TimeSpan _finalRaceTime;
        public TimeSpan FinalRaceTime
        {
            get { return _finalRaceTime; }
            set 
            { 
                SetField(ref _finalRaceTime, value, nameof(FinalRaceTime));
                TotalRaceTime = value + FinalPenaltiesTime;
            }
        }

        private TimeSpan _finalPenaltiesTime;
        public TimeSpan FinalPenaltiesTime
        {
            get { return _finalPenaltiesTime; }
            set { SetField(ref _finalPenaltiesTime, value, nameof(FinalPenaltiesTime)); }
        }

        private int _finalPenaltiesNum;
        public int FinalPenaltiesNum
        {
            get { return _finalPenaltiesNum; }
            set { SetField(ref _finalPenaltiesNum, value, nameof(FinalPenaltiesNum)); }
        }

        private int _finalTireStintsNum;
        public int FinalTireStintsNum
        {
            get { return _finalTireStintsNum; }
            set { SetField(ref _finalTireStintsNum, value, nameof(FinalTireStintsNum)); }
        }

        private int _pendingPoints;
        public int PendingPoints
        {
            get { return _pendingPoints; }
            set { SetField(ref _pendingPoints, value, nameof(PendingPoints)); }
        }

        private int _pointsReceived;
        public int PointsReceived
        {
            get { return _pointsReceived; }
            set { SetField(ref _pointsReceived, value, nameof(PointsReceived)); }
        }
    }
}
