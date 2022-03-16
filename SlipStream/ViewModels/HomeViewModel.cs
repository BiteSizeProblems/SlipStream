﻿using System;
using System.Text.RegularExpressions;
using SlipStream.Structs;

namespace SlipStream.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {

        // === BEGINING OF MODULE SETUP ===
        // === Singleton Instance with Thread Saftey ===
        private static HomeViewModel _instance = null;
        private static object _singletonLock = new object();
        public static HomeViewModel GetInstance()
        {
            lock (_singletonLock)
            {
                if (_instance == null) { _instance = new HomeViewModel(); }
                return _instance;
            }
        }
        // === END OF MODULE SETUP ===

        private string _ConnectionStatus;
        public string ConnectionStatus
        {
            get { return _ConnectionStatus; }
            set { SetField(ref _ConnectionStatus, value, nameof(ConnectionStatus)); }
        }

        private string _Formula;
        public string Formula
        {
            get { return _Formula; }
            set { SetField(ref _Formula, value, nameof(Formula)); }
        }

        private string _Circuit;
        public string Circuit
        {
            get { return _Circuit; }
            set { SetField(ref _Circuit, value, nameof(Circuit)); }
        }

        private string _CurrentSession;
        public string CurrentSession
        {
            get { return _CurrentSession; }
            set { SetField(ref _CurrentSession, value, nameof(CurrentSession)); }
        }

        private string _CurrentWeather;
        public string CurrentWeather
        {
            get { return _CurrentWeather; }
            set { SetField(ref _CurrentWeather, value, nameof(CurrentWeather)); }
        }

        private string _SessionDuration;
        public string SessionDuration
        {
            get { return _SessionDuration; }
            set { SetField(ref _SessionDuration, value, nameof(SessionDuration)); }
        }

        private string _SessionTimeRemaining;
        public string SessionTimeRemaining
        {
            get { return _SessionTimeRemaining; }
            set { SetField(ref _SessionTimeRemaining, value, nameof(SessionTimeRemaining)); }
        }

        private string _NumOfActiveCars;
        public string NumOfActiveCars
        {
            get { return _NumOfActiveCars; }
            set { SetField(ref _NumOfActiveCars, value, nameof(NumOfActiveCars)); }
        }

        private string _NumOfParticipants;
        public string NumOfParticipants
        {
            get { return _NumOfParticipants; }
            set { SetField(ref _NumOfParticipants, value, nameof(NumOfParticipants)); }
        }

        public HomeViewModel() : base()
        {
            UDPC.OnSessionDataReceive += UDPC_OnSessionDataReceive;
        }

        private void UDPC_OnSessionDataReceive(PacketSessionData packet)
        {
            ConnectionStatus = "Status: Connected";
            Formula = ("Formula: " + Regex.Replace(packet.m_formula.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim());
            Circuit = ("Circuit: " + Regex.Replace(packet.m_trackId.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim());
            CurrentSession = ("Session Type: " + Regex.Replace(packet.m_sessionType.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim());
            CurrentWeather = ("Weather: " + Regex.Replace(packet.m_weather.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim());
            SessionDuration = $"Session Duration: {TimeSpan.FromSeconds(packet.m_sessionDuration)}";
            SessionTimeRemaining = $"Session Time Remaining: {TimeSpan.FromSeconds(packet.m_sessionTimeLeft)}";
        }

        private void UDPC_OnParticipantsDataReceive(PacketParticipantsData packet)
        {
            ConnectionStatus = "Status: Connected";
            NumOfActiveCars = $"Active Cars: {packet.m_numActiveCars}";
            NumOfParticipants = $"Session Participants: {packet.m_participants}";
        }
    }
}
