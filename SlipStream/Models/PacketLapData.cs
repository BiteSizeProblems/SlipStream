using System.Runtime.InteropServices;
using static SlipStream.Core.Appendeces;

namespace SlipStream.Models
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LapData
    {
        public uint lastLapTimeInMS;            // Last lap time in milliseconds
        public uint currentLapTimeInMS;     // Current time around the lap in milliseconds
        public ushort sector1TimeInMS;           // Sector 1 time in milliseconds
        public ushort sector2TimeInMS;           // Sector 2 time in milliseconds
        public float lapDistance;         // Distance vehicle is around current lap in metres – could
                                          // be negative if line hasn’t been crossed yet
        public float totalDistance;       // Total distance travelled in session in metres – could
                                          // be negative if line hasn’t been crossed yet
        public float safetyCarDelta;            // Delta in seconds for safety car
        public byte carPosition;             // Car race position
        public byte currentLapNum;       // Current lap number
        public byte pitStatus;               // 0 = none, 1 = pitting, 2 = in pit area
        public byte numPitStops;                 // Number of pit stops taken in this race
        public byte sector;                  // 0 = sector1, 1 = sector2, 2 = sector3
        public byte currentLapInvalid;       // Current lap invalid - 0 = valid, 1 = invalid
        public byte penalties;               // Accumulated time penalties in seconds to be added
        public byte warnings;                  // Accumulated number of warnings issued
        public byte numUnservedDriveThroughPens;  // Num drive through pens left to serve
        public byte numUnservedStopGoPens;        // Num stop go pens left to serve
        public byte gridPosition;            // Grid position the vehicle started the race in
        public DriverStatus driverStatus;            // Status of driver - 0 = in garage, 1 = flying lap
                                             // 2 = in lap, 3 = out lap, 4 = on track
        public byte resultStatus;              // Result status - 0 = invalid, 1 = inactive, 2 = active
                                               // 3 = finished, 4 = didnotfinish, 5 = disqualified
                                               // 6 = not classified, 7 = retired
        public byte pitLaneTimerActive;          // Pit lane timing, 0 = inactive, 1 = active
        public ushort pitLaneTimeInLaneInMS;      // If active, the current time spent in the pit lane in ms
        public ushort pitStopTimerInMS;           // Time of the actual pit stop in ms
        public byte pitStopShouldServePen;   	 // Whether the car should serve a penalty at this stop
    }

    /// <summary>
    /// The lap data packet gives details of all the cars in the session
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketLapData
    {
        /// <summary>
        /// Header
        /// </summary>
        public PacketHeader header;

        /// <summary>
        /// Array containing lap data for all cars
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        public LapData[] lapData;
    }
}
