using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Runtime.InteropServices;
using System.Timers;
using SlipStream.Models;
using static SlipStream.Core.Appendeces;
using System.Diagnostics;

namespace SlipStream.Core
{
    public class UDPConnection
    {
        /// <summary>
        /// Time required to time out in MS.
        /// </summary>
        private const float TIMEOUT_IN_MS = 500.0f;

        private UdpClient _client;
        private IPEndPoint _endPoint;

        private Timer _timeoutTimer;

        /// <summary>
        /// Indicates if we are currently connected.
        /// </summary>
        public bool Connected { get; private set; }

        /// <summary>
        /// Connection status change delegate.
        /// </summary>
        /// <param name="connected">True if connected, false if disconnected.</param>
        public delegate void ConnectStatusChangeDelegate(bool connected);

        /// <summary>
        /// Called when connect status changed. 
        /// Connected bool indicates if there is a connection (true) or disconnection (false).
        /// </summary>
        public event ConnectStatusChangeDelegate OnConnectStatusChanged;

        // Delegates
        public delegate void MotionDataReceiveDelegate(PacketMotionData packet);
        public delegate void SessionDataReceiveDelegate(PacketSessionData packet);
        public delegate void LapDataReceiveDelegate(PacketLapData packet);
        public delegate void EventDataReceiveDelegate(PacketEventData packet);
        public delegate void ParticipantsDataReceiveDelegate(PacketParticipantsData packet);
        public delegate void CarSetupsDataReceiveDelegate(PacketCarSetupData packet);
        public delegate void CarTelemetryDataReceiveDelegate(PacketCarTelemetryData packet);
        public delegate void CarStatusDataReceiveDelegate(PacketCarStatusData packet);
        public delegate void FinalClassificationDataReceiveDelegate(PacketFinalClassificationData packet);
        public delegate void LobbyInfoDataReceiveDelegate(PacketLobbyInfoData packet);

        public PacketParticipantsData LastParticipantsPacket { get; private set; }

        // Packet events
        public event MotionDataReceiveDelegate OnMotionDataReceive;
        public event SessionDataReceiveDelegate OnSessionDataReceive;
        public event LapDataReceiveDelegate OnLapDataReceive;
        public event EventDataReceiveDelegate OnEventDataReceive;
        public event ParticipantsDataReceiveDelegate OnParticipantsDataReceive;
        public event CarSetupsDataReceiveDelegate OnCarSetupsDataReceive;
        public event CarTelemetryDataReceiveDelegate OnCarTelemetryDataReceive;
        public event CarStatusDataReceiveDelegate OnCarStatusDataReceive;
        public event FinalClassificationDataReceiveDelegate OnFinalClassificationDataReceive;
        public event LobbyInfoDataReceiveDelegate OnLobbyInfoDataReceive;

        // === Singleton Instance with Thread Saftey ===
        private static UDPConnection _instance = null;
        private static object _singletonLock = new object();
        public static UDPConnection GetInstance()
        {
            lock (_singletonLock)
            {
                if (_instance == null) { _instance = new UDPConnection(20777); }
                return _instance;
            }
        }

        /// <summary>
        /// Constructs telemetry client and sets it up for receiving data.
        /// </summary>
        /// <param name="port">The port to listen to. This should match the game setting.</param>
        private UDPConnection(int port)
        {

            Trace.WriteLine("UDP CONNECTION CTOR");

            _client = new UdpClient(port);
            _endPoint = new IPEndPoint(IPAddress.Any, 0);

            _timeoutTimer = new Timer(TIMEOUT_IN_MS);
            _timeoutTimer.AutoReset = false;
            _timeoutTimer.Elapsed += TimeoutEvent;

            // Start receiving updates.
            _client.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        }

        /// <summary>
        /// Called when data is received.
        /// </summary>
        /// <param name="result">Resulting data.</param>
        private void ReceiveCallback(IAsyncResult result)
        {
            // Handle connected event.
            if (Connected == false)
            {
                Connected = true;
                OnConnectStatusChanged?.Invoke(true);
            }

            // Restart the timeout timer.
            _timeoutTimer.Stop();
            _timeoutTimer.Start();

            // Get data we received.
            byte[] data = _client.EndReceive(result, ref _endPoint);

            // Start receiving again.
            _client.BeginReceive(new AsyncCallback(ReceiveCallback), null);

            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);

            try
            {
                // Get the header to retrieve the packet ID.
                PacketHeader header = (PacketHeader)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PacketHeader));

                // Switch on packet id, and call the correct event.
                // Cast the packet to the correct type based on the ID.
                switch (header.packetId)
                {
                    case PacketTypes.CarMotion:
                        PacketMotionData motionData = (PacketMotionData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PacketMotionData));
                        OnMotionDataReceive?.Invoke(motionData);
                        break;
                    case PacketTypes.Session:
                        PacketSessionData sessionData = (PacketSessionData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PacketSessionData));
                        OnSessionDataReceive?.Invoke(sessionData);
                        break;
                    case PacketTypes.LapData:
                        PacketLapData lapData = (PacketLapData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PacketLapData));
                        OnLapDataReceive?.Invoke(lapData);
                        break;
                    case PacketTypes.Event:
                        PacketEventData eventData = (PacketEventData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PacketEventData));
                        OnEventDataReceive?.Invoke(eventData);
                        break;
                    case PacketTypes.Participants:
                        PacketParticipantsData participantsData = (PacketParticipantsData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PacketParticipantsData));
                        OnParticipantsDataReceive?.Invoke(participantsData);
                        break;
                    case PacketTypes.CarSetups:
                        PacketCarSetupData carSetupsData = (PacketCarSetupData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PacketCarSetupData));
                        OnCarSetupsDataReceive?.Invoke(carSetupsData);
                        break;
                    case PacketTypes.CarTelemetry:
                        PacketCarTelemetryData telemetryData = (PacketCarTelemetryData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PacketCarTelemetryData));
                        OnCarTelemetryDataReceive?.Invoke(telemetryData);
                        break;
                    case PacketTypes.CarStatus:
                        PacketCarStatusData carStatusData = (PacketCarStatusData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PacketCarStatusData));
                        OnCarStatusDataReceive?.Invoke(carStatusData);
                        break;
                    case PacketTypes.FinalClassification:
                        PacketFinalClassificationData finalClassificationData = (PacketFinalClassificationData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PacketFinalClassificationData));
                        OnFinalClassificationDataReceive?.Invoke(finalClassificationData);
                        break;
                    case PacketTypes.LobbyInfo:
                        PacketLobbyInfoData lobbyInfoData = (PacketLobbyInfoData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PacketLobbyInfoData));
                        OnLobbyInfoDataReceive?.Invoke(lobbyInfoData);
                        break;
                }
            }
            catch
            {
                Console.WriteLine("Failed to receive F1 2020 packet.");
            }
            finally
            {
                handle.Free();
            }
        }

        /// <summary>
        /// Called when no data is received for a period of time.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Elapsed event arguments.</param>
        private void TimeoutEvent(object sender, ElapsedEventArgs e)
        {
            Connected = false;
            OnConnectStatusChanged?.Invoke(false);
        }
    }
}
