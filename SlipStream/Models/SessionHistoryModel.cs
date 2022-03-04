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

        public uint m_bestLapTimeLapNum;                       // Lap the best lap time was achieved on
        public uint m_bestSector1LapNum;                       // Lap the best Sector 1 time was achieved on
        public uint m_bestSector2LapNum;                       // Lap the best Sector 2 time was achieved on
        public uint m_bestSector3LapNum;                       // Lap the best Sector 3 time was achieved on

        // Lap History Data

        public uint m_lapTimeInMS;                // lap time in milliseconds
        public uint m_sector1TimeInMS;            // Sector 1 time in milliseconds
        public uint m_sector2TimeInMS;            // Sector 2 time in milliseconds
        public uint m_sector3TimeInMS;            // Sector 3 time in milliseconds
        public uint m_lapValidBitFlags;           // 0x01 bit set-lap valid,

        // Tire History Data

        public uint m_endLap;                       // Lap the tyre usage ends on (255 of current tyre)
        public uint m_tyreActualCompound;           // Actual tyres used by this driver
        public uint m_tyreVisualCompound;           // Visual tyres used by this driver

    }
}
