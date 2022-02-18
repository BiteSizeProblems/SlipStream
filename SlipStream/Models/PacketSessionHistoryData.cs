using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SlipStream.Models
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LapHistoryData
    {
        public uint m_lapTimeInMS;                // lap time in milliseconds
        public uint m_sector1TimeInMS;            // Sector 1 time in milliseconds
        public uint m_sector2TimeInMS;            // Sector 2 time in milliseconds
        public uint m_sector3TimeInMS;            // Sector 3 time in milliseconds
        public uint m_lapValidBitFlags;           // 0x01 bit set-lap valid,
                                                  // 0x02 bit set-sector 1 valid,
                                                  // 0x04 bit set-sector 2 valid,
                                                  // 0x08 bit set-sector 3 valid
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TyreStintHistoryData
    {
        public uint m_endLap;                       // Lap the tyre usage ends on (255 of current tyre)
        public uint m_tyreActualCompound;           // Actual tyres used by this driver
        public uint m_tyreVisualCompound;           // Visual tyres used by this driver
    }

    /// <summary>
    /// The lap data packet gives details of all the cars in the session
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketSessionHistoryData
    {
        /// <summary>
        /// Header
        /// </summary>
        public PacketHeader m_header;                          // Header

        public uint m_carIdx;                                  // Index of the car this lap data relates to
        public uint m_numLaps;                                 // Num laps in the data (including current partial lap)
        public uint m_numTyreStints;                           // Number of tyre stints in the data

        public uint m_bestLapTimeLapNum;                       // Lap the best lap time was achieved on
        public uint m_bestSector1LapNum;                       // Lap the best Sector 1 time was achieved on
        public uint m_bestSector2LapNum;                       // Lap the best Sector 2 time was achieved on
        public uint m_bestSector3LapNum;                       // Lap the best Sector 3 time was achieved on


        /// <summary>
        /// Array containing lap history data for all cars
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        public LapHistoryData[] m_lapHistoryData;               // 100 laps of data max

        /// <summary>
        /// Array containing tyre history data for all cars
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public TyreStintHistoryData[] m_tyreStintsHistoryData;
    }
}
