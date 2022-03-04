using SlipStream.Core;
using System;
using static SlipStream.Structs.Appendeces;

namespace SlipStream.Models
{
    // MODEL 
    public class DriverData : ObservableObject
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

        private TimeSpan lastLapTime;
        public TimeSpan LastLapTime
        {
            get { return lastLapTime; }
            set
            {
                SetField(ref lastLapTime, value, nameof(LastLapTime));

                if (CurrentLapNum == 1)
                {
                    bestLapTime = LastLapTime;
                }
                else if (CurrentLapNum > 1 && LastLapTime < BestLapTime)
                {
                    bestLapTime = LastLapTime;
                }
                else
                {
                    bestLapTime = LastLapTime;
                }

            }
        }

        private TimeSpan bestLapTime;
        public TimeSpan BestLapTime
        {
            get { return bestLapTime; }
            set { SetField(ref bestLapTime, value, nameof(BestLapTime)); }
        }



        public TimeSpan BestLapGap { get; set; }

        private string bestSector1Time;
        public string BestSector1Time
        {
            get { return bestSector1Time; }
            set { SetField(ref bestSector1Time, value, nameof(BestSector1Time)); }
        }

        private TimeSpan bestLapSector1TimeInMS;
        public TimeSpan BestLapSector1TimeInMS
        {
            get { return bestLapSector1TimeInMS; }
            set { SetField(ref bestLapSector1TimeInMS, value, nameof(BestLapSector1TimeInMS)); }
        }

        private TimeSpan bestLapSector2TimeInMS;
        public TimeSpan BestLapSector2TimeInMS
        {
            get { return bestLapSector2TimeInMS; }
            set { SetField(ref bestLapSector2TimeInMS, value, nameof(BestLapSector2TimeInMS)); }
        }

        private string bestLapSector3TimeInMS;
        public string BestLapSector3TimeInMS
        {
            get { return bestLapSector3TimeInMS; }
            set { SetField(ref bestLapSector3TimeInMS, value, nameof(BestLapSector3TimeInMS)); }
        }

        private byte carPosition;
        public byte CarPosition
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

        // Args CTOR
        public DriverData(Drivers d, Teams t, int r)
        {
            this.DriverID = d;
            this.TeamID = t;
            this.raceNumber = r;
        }

        // NoArgs CTOR
        public DriverData()
        {
            this.DriverID = Drivers.Unknown;
            this.TeamID = Teams.Unknown;
            this.CurrentLapTime = 0;
        }
    }
}
