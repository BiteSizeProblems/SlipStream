﻿using System.Runtime.InteropServices;
using static SlipStream.Core.Appendeces;

namespace SlipStream.Models
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ParticipantData
    {
        /// <summary>
        /// Whether the vehicle is AI (1) or Human (0) controlled
        /// </summary>
        public byte aiControlled;

        /// <summary>
        /// Driver id - see appendix
        /// </summary>
        public Drivers driverId;

        /// <summary>
        /// Team id - see appendix
        /// </summary>
        public byte teamId;

        /// <summary>
        /// Race number of the car
        /// </summary>
        public byte raceNumber;

        /// <summary>
        /// Nationality of the driver
        /// </summary>
        public byte nationality;

        /// <summary>
        /// Name of participant in UTF-8 format – null terminated
        /// Will be truncated with … (U+2026) if too long
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
        public char[] name;

        /// <summary>
        /// The player's UDP setting, 0 = restricted, 1 = public
        /// </summary>
        public byte yourTelemetry;
    }

    /// <summary>
    /// This is a list of participants in the race.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketParticipantsData
    {

        /// <summary>
        /// Header
        /// </summary>
        public PacketHeader header;

        /// <summary>
        /// Number of active cars in the data – should match number of cars on HUD
        /// </summary>
        public byte numActiveCars;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        public ParticipantData[] participants;
    }

}
