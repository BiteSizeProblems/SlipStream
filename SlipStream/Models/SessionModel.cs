﻿using SlipStream.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlipStream.Models
{
    // MODEL
    public class SessionModel : ObservableObject
    {
        private int _totalParticipants;
        public int TotalParticipants
        {
            get { return _totalParticipants; }
            set 
            { 
                SetField(ref _totalParticipants, value, nameof(TotalParticipants));
                MaxIndex = TotalParticipants - 1;
            }
        }

        private int _maxIndex;
        public int MaxIndex
        {
            get { return _maxIndex; }
            set { SetField(ref _maxIndex, value, nameof(MaxIndex)); }
        }

        private int _totalLaps;
        public int TotalLaps
        {
            get { return _totalLaps; }
            set { SetField(ref _totalLaps, value, nameof(TotalLaps)); }
        }

        private string _lapsRemaining;
        public string LapsRemaining
        {
            get { return _lapsRemaining; }
            set { SetField(ref _lapsRemaining, value, nameof(LapsRemaining)); }
        }

        // LEADER DATA

        private int _leadLap;
        public int LeadLap
        {
            get { return _leadLap; }
            set { SetField(ref _leadLap, value, nameof(LeadLap)); }
        }

        private TimeSpan _leadLapTime;
        public TimeSpan LeadLapTime
        {
            get { return _leadLapTime; }
            set { SetField(ref _leadLapTime, value, nameof(LeadLapTime)); }
        }

        private TimeSpan sessionFastestLap;
        public TimeSpan SessionFastestLap
        {
            get { return sessionFastestLap; }
            set { SetField(ref sessionFastestLap, value, nameof(SessionFastestLap)); }
        }

        private string sessionFastestLapID;
        public string SessionFastestLapID
        {
            get { return sessionFastestLapID; }
            set { SetField(ref sessionFastestLapID, value, nameof(SessionFastestLapID)); }
        }

        // TIME

        private string _SessionTimeRemaining;
        public string SessionTimeRemaining
        {
            get { return _SessionTimeRemaining; }
            set { SetField(ref _SessionTimeRemaining, value, nameof(SessionTimeRemaining)); }
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

        private string _networkGame;
        public string NetworkGame
        {
            get { return _networkGame; }
            set { SetField(ref _networkGame, value, nameof(NetworkGame)); }
        }

        private string _CurrentWeather;
        public string CurrentWeather
        {
            get { return _CurrentWeather; }
            set { SetField(ref _CurrentWeather, value, nameof(CurrentWeather)); }
        }

        private string _CurrentWeatherIcon;
        public string CurrentWeatherIcon
        {
            get { return _CurrentWeatherIcon; }
            set { SetField(ref _CurrentWeatherIcon, value, nameof(CurrentWeatherIcon)); }
        }

        private string _SessionDuration;
        public string SessionDuration
        {
            get { return _SessionDuration; }
            set { SetField(ref _SessionDuration, value, nameof(SessionDuration)); }
        }

        private string _NumOfActiveCars;
        public string NumOfActiveCars
        {
            get { return _NumOfActiveCars; }
            set { SetField(ref _NumOfActiveCars, value, nameof(NumOfActiveCars)); }
        }

        private int _NumOfParticipants;
        public int NumOfParticipants
        {
            get { return _NumOfParticipants; }
            set { SetField(ref _NumOfParticipants, value, nameof(NumOfParticipants)); }
        }

        private string _eventStringCode;
        public string EventStringCode
        {
            get { return _eventStringCode; }
            set { SetField(ref _eventStringCode, value, nameof(EventStringCode)); }
        }

        private string _safetyCarStatus;
        public string SafetyCarStatus
        {
            get { return _safetyCarStatus; }
            set { SetField(ref _safetyCarStatus, value, nameof(SafetyCarStatus)); }
        }

        private string _safetyCarColor;
        public string SafetyCarColor
        {
            get { return _safetyCarColor; }
            set { SetField(ref _safetyCarColor, value, nameof(SafetyCarColor)); }
        }

        private string _safetyCarIcon;
        public string SafetyCarIcon
        {
            get { return _safetyCarIcon; }
            set { SetField(ref _safetyCarIcon, value, nameof(SafetyCarIcon)); }
        }



    }
}