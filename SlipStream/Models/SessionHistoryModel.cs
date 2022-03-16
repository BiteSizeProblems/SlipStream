using SlipStream.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlipStream.Models
{
    public class SessionHistoryModel : ObservableObject
    {
        // Packet Session History Data

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

        private uint _bestLapTimeLapNum;
        public uint BestLapTimeLapNum
        {
            get { return _bestLapTimeLapNum; }
            set { SetField(ref _bestLapTimeLapNum, value, nameof(BestLapTimeLapNum)); }
        }
        private uint _bestS1LapNum;
        public uint BestS1LapNum
        {
            get { return _bestS1LapNum; }
            set { SetField(ref _bestS1LapNum, value, nameof(BestS1LapNum)); }
        }
        private uint _bestS2LapNum;
        public uint BestS2LapNum
        {
            get { return _bestS2LapNum; }
            set { SetField(ref _bestS2LapNum, value, nameof(BestS2LapNum)); }
        }
        private uint _bestS3LapNum;
        public uint BestS3LapNum
        {
            get { return _bestS3LapNum; }
            set { SetField(ref _bestS3LapNum, value, nameof(BestS3LapNum)); }
        }

        // Lap History Data

        private uint _lapTime;
        public uint LapTime
        {
            get { return _lapTime; }
            set { SetField(ref _lapTime, value, nameof(LapTime)); }
        }
        private uint _sector1Time;
        public uint Sector1Time
        {
            get { return _sector1Time; }
            set { SetField(ref _sector1Time, value, nameof(Sector1Time)); }
        }
        private uint _sector2Time;
        public uint Sector2Time
        {
            get { return _sector2Time; }
            set { SetField(ref _sector2Time, value, nameof(Sector2Time)); }
        }
        private uint _sector3Time;
        public uint Sector3Time
        {
            get { return _sector3Time; }
            set { SetField(ref _sector3Time, value, nameof(Sector3Time)); }
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
        private uint _tireActual;
        public uint TireActual
        {
            get { return _tireActual; }
            set { SetField(ref _tireActual, value, nameof(TireActual)); }
        }
        private uint _tireVisual;
        public uint TireVisual
        {
            get { return _tireVisual; }
            set { SetField(ref _tireVisual, value, nameof(TireVisual)); }
        }
    }
}
