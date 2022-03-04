using System.Runtime.InteropServices;
using static SlipStream.Structs.Appendeces;

namespace SlipStream.Structs
{
    /// <summary>
    /// Session data
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MarshalZone
    {
        /// <summary>
        /// Fraction (0..1) of way through the lap the marshal zone starts
        /// </summary>
        public float zoneStart;

        /// <summary>
        /// -1 = invalid/unknown, 0 = none, 1 = green, 2 = blue, 3 = yellow, 4 = red
        /// </summary>
        public Flags Flags;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct WeatherForecastSample
    {
        /// <summary>
        /// The current session type
        /// </summary>
        public SessionTypes m_sessionType;

        /// <summary>
        /// Time in minutes the forecast is for
        /// </summary>
        public byte m_timeOffset;

        /// <summary>
        /// The weather for this sample
        /// </summary>
        public WeatherTypes m_weather;

        /// <summary>
        /// Track temp. in degrees celsius
        /// </summary>
        public sbyte m_trackTemperature;

        /// <summary>
        /// Track temp. in degrees celsius
        /// </summary>
        public sbyte m_trackTemperatureChange;

        /// <summary>
        /// Air temp. in degrees celsius
        /// </summary>
        public sbyte m_airTemperature;

        /// <summary>
        /// Air temp. in degrees celsius
        /// </summary>
        public sbyte m_airTemperatureChange;

        /// <summary>
        /// Rain percentage (0-100)
        /// </summary>
        public byte m_rainPercentage;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketSessionData
    {
        /// <summary>
        /// Header
        /// </summary>
        public PacketHeader header;

        /// <summary>
        /// The current weather
        /// </summary>
        public WeatherTypes weather;

        /// <summary>
        /// Track temp. in degrees celsius
        /// </summary>
        public sbyte trackTemperature;

        /// <summary>
        /// Air temp. in degrees celsius
        /// </summary>
        public sbyte airTemperature;

        /// <summary>
        /// Total number of laps in this race
        /// </summary>
        public byte totalLaps;

        /// <summary>
        /// Track length in metres
        /// </summary>
        public ushort trackLength;

        /// <summary>
        /// The current session type
        /// </summary>
        public SessionTypes sessionType;

        /// <summary>
        /// The current track ID
        /// </summary>
        public Tracks trackId;

        /// <summary>
        /// Type of Formula
        /// </summary>
        public Formulas formula;

        /// <summary>
        /// Time left in session in seconds
        /// </summary>
        public ushort sessionTimeLeft;

        /// <summary>
        /// Session duration in seconds
        /// </summary>
        public ushort sessionDuration;

        /// <summary>
        /// Pit speed limit in kilometres per hour
        /// </summary>
        public byte pitSpeedLimit;

        /// <summary>
        /// Whether the game is paused
        /// </summary>
        public byte gamePaused;

        /// <summary>
        /// Whether the player is spectating
        /// </summary>
        public byte isSpectating;

        /// <summary>
        /// Index of the car being spectated
        /// </summary>
        public byte spectatorCarIndex;

        /// <summary>
        /// SLI Pro support, 0 = inactive, 1 = active
        /// </summary>
        public byte sliProNativeSupport;

        /// <summary>
        /// Number of marshal zones to follow
        /// </summary>
        public byte numMarshalZones;

        /// <summary>
        /// List of marshal zones – max 21
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
        public MarshalZone[] marshalZones;

        /// <summary>
        /// Current safety car status
        /// </summary>
        public SafetyCarStatuses safetyCarStatus;

        /// <summary>
        /// 0 = offline, 1 = online
        /// </summary>
        public byte networkGame;

        /// <summary>
        /// Number of weather samples to follow
        /// </summary>
        public byte numWeatherForecastSamples;

        /// <summary>
        /// Array of weather forecast samples
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public WeatherForecastSample[] weatherForecastSamples;
    }
}
