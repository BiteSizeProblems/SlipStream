﻿using System;
using System.Collections.Generic;

namespace SlipStream.Structs
{
    public static class Appendeces
    {
        /// <summary>
        /// Type of the packet (In header's PocketID property).
        /// </summary>
        public enum PacketTypes : byte
        {
            /// <summary>
            /// Contains all motion data for player’s car – only sent while player is in control
            /// </summary>
            CarMotion,
            /// <summary>
            /// Data about the session – track, time left
            /// </summary>
            Session,
            /// <summary>
            /// Data about all the lap times of cars in the session
            /// </summary>
            LapData,
            /// <summary>
            /// Various notable events that happen during a session
            /// </summary>
            Event,
            /// <summary>
            /// List of participants in the session, mostly relevant for multiplayer
            /// </summary>
            Participants,
            /// <summary>
            /// Packet detailing car setups for cars in the race
            /// </summary>
            CarSetups,
            /// <summary>
            /// Telemetry data for all cars
            /// </summary>
            CarTelemetry,
            /// <summary>
            /// Status data for all cars
            /// </summary>
            CarStatus,
            /// <summary>
            /// Final classification confirmation at the end of a race
            /// </summary>
            FinalClassification,
            /// <summary>
            /// Information about players in a multiplayer lobby
            /// </summary>
            LobbyInfo,
            /// <summary>
            /// Damage status for all cars
            /// </summary>
            CarDamage,
            /// <summary>
            /// Lap and tyre data for session
            /// </summary>
            SessionHistory
        }

        // TEAMS
        public enum Teams : byte
        {
            MyTeam = 255,
            Unknown = 254,
            Mercedes = 0,
            Ferrari = 1,
            RedBullRacing = 2,
            Williams = 3,
            AstonMartin = 4,
            Alpine = 5,
            AlphaTauri = 6,
            Haas = 7,
            Mclaren = 8,
            AlfaRomeo = 9,
            CustomTeam = 41,
            ArtGP19 = 42,
            Campos19 = 43,
            Carlin19 = 44,
            SauberJuniorCharouz19 = 45,
            Dams19 = 46,
            UniVirtuosi19 = 47,
            MPMotorsport19 = 48,
            Prema19 = 49,
            Trident19 = 50,
            Arden19 = 51,
            ArtGP20 = 70,
            Campos20 = 71,
            Carlin20 = 72,
            Charouz20 = 73,
            Dams20 = 74,
            UniVirtuosi20 = 75,
            MPMotorsport20 = 76,
            Prema20 = 77,
            Trident20 = 78,
            BWT20 = 79,
            Hitech20 = 80,
            Mercedes2020 = 85,
            Ferrari2020 = 86,
            RedBull2020 = 87,
            Williams2020 = 88,
            RacingPoint2020 = 89,
            Renault2020 = 90,
            AlphaTauri2020 = 91,
            Haas2020 = 92,
            McLaren2020 = 93,
            AlfaRomeo2020 = 94,
        }

        public enum Tracks : sbyte
        {
            Unknown = -1,
            AlbertPark = 0,
            PaulRicard = 1,
            ShanghaiInternational = 2,
            Sakhir = 3,
            Catalunya = 4,
            Monaco = 5,
            CircuitGillesVilleneuve = 6,
            Silverstone = 7,
            Hockenheim = 8,
            Hungaroring = 9,
            SpaFrancorchamps = 10,
            Monza = 11,
            MarinaBay = 12,
            Suzuka = 13,
            YasMarina = 14,
            CircuitOfTheAmericas = 15,
            Interlagos = 16,
            RedBullRing = 17,
            SochiAutodrom = 18,
            AutódromoHermanosRodríguez = 19,
            BakuCity = 20,
            SakhirShort = 21,
            SilverstoneShort = 22,
            CircuitOfTheAmericasShort = 23,
            SuzukaShort = 24,
            Hanoi = 25,
            Zandvoort = 26,
            Imola = 27,
            AlgarveInternationalCircuit = 28,
            JeddahCorniche = 29,
        }

        public enum GrandPrix : sbyte
        {
            Unknown = -1,
            AUSTRALIAN_GRAND_PRIX = 0,
            FRENCH_GRAND_PRIX = 1,
            CHINESE_GRAND_PRIX = 2,
            BAHRAIN_GRAND_PRIX = 3,
            SPANISH_GRAND_PRIX = 4,
            MONACO_GRAND_PRIX = 5,
            CANADIAN_GRAND_PRIX = 6,
            BRITISH_GRAND_PRIX = 7,
            GERMAN_GRAND_PRIX = 8,
            HUNGARIAN_GRAND_PRIX = 9,
            BELGIAN_GRAND_PRIX = 10,
            ITALIAN_GRAND_PRIX_AT_MONZA = 11,
            SINGAPORE_GRAND_PRIX = 12,
            JAPANESE_GRAND_PRIX = 13,
            ABU_DHABI_GRAND_PRIX = 14,
            UNITED_STATES_GRAND_PRIX = 15,
            BRAZILIAN_GRAND_PRIX = 16,
            AUSTRIAN_GRAND_PRIX = 17,
            RUSSIAN_GRAND_PRIX = 18,
            MEXICAN_GRAND_PRIX = 19,
            AZERBAIJAN_GRAND_PRIX = 20,
            BAHRAIN_SHORT_GRAND_PRIX = 21,
            BRITISH_SHORT_GRAND_PRIX = 22,
            UNITED_STATES_SHORT_GRAND_PRIX = 23,
            JAPANESE_SHORT_GRAND_PRIX = 24,
            VIETNAM_GRAND_PRIX = 25,
            DUTCH_GRAND_PRIX = 26,
            ITALIAN_GRAND_PRIX_AT_IMOLA = 27,
            PORTUGESE_GRAND_PRIX = 28,
            SAUDI_ARABIAN_GRAND_PRIX = 29,
        }

        public enum Drivers : byte
        {
            CarlosSainz = 0,
            DaniilKvyat = 1,
            DanielRicciardo = 2,
            FernandoAlonso = 3,
            FelipeMassa = 4,
            KimiRäikkönen = 6,
            LewisHamilton = 7,
            MaxVerstappen = 9,
            NicoHulkenburg = 10,
            KevinMagnussen = 11,
            RomainGrosjean = 12,
            SebastianVettel = 13,
            SergioPerez = 14,
            ValtteriBottas = 15,
            EstebanOcon = 17,
            LanceStroll = 19,
            ArronBarnes = 20,
            MartinGiles = 21,
            AlexMurray = 22,
            LucasRoth = 23,
            IgorCorreia = 24,
            SophieLevasseur = 25,
            JonasSchiffer = 26,
            AlainForest = 27,
            JayLetourneau = 28,
            EstoSaari = 29,
            YasarAtiyeh = 30,
            CallistoCalabresi = 31,
            NaotaIzum = 32,
            HowardClarke = 33,
            WilheimKaufmann = 34,
            MarieLaursen = 35,
            FlavioNieves = 36,
            PeterBelousov = 37,
            KlimekMichalski = 38,
            SantiagoMoreno = 39,
            BenjaminCoppens = 40,
            NoahVisser = 41,
            GertWaldmuller = 42,
            JulianQuesada = 43,
            DanielJones = 44,
            ArtemMarkelov = 45,
            TadasukeMakino = 46,
            SeanGelael = 47,
            NyckDeVries = 48,
            JackAitken = 49,
            GeorgeRussell = 50,
            MaximilianGünther = 51,
            NireiFukuzumi = 52,
            LucaGhiotto = 53,
            LandoNorris = 54,
            SérgioSetteCâmara = 55,
            LouisDelétraz = 56,
            AntonioFuoco = 57,
            CharlesLeclerc = 58,
            PierreGasly = 59,
            AlexanderAlbon = 62,
            NicholasLatifi = 63,
            DorianBoccolacci = 64,
            NikoKari = 65,
            RobertoMerhi = 66,
            ArjunMaini = 67,
            AlessioLorandi = 68,
            RubenMeijer = 69,
            RashidNair = 70,
            JackTremblay = 71,
            DevonButler = 72,
            LukasWeber = 73,
            AntonioGiovinazzi = 74,
            RobertKubica = 75,
            AlainProst = 76,
            AyrtonSenna = 77,
            NobuharuMatsushita = 78,
            NikitaMazepin = 79,
            GuanyaZhou = 80,
            MickSchumacher = 81,
            CallumIlott = 82,
            JuanManuelCorrea = 83,
            JordanKing = 84,
            MahaveerRaghunathan = 85,
            TatianaCalderon = 86,
            AnthoineHubert = 87,
            GuilianoAlesi = 88,
            RalphBoschung = 89,
            MichaelSchumacher = 90,
            DanTicktum = 91,
            MarcusArmstrong = 92,
            ChristianLundgaard = 93,
            YukiTsunoda = 94,
            JehanDaruvala = 95,
            GulhermeSamaia = 96,
            PedroPiquet = 97,
            FelipeDrugovich = 98,
            RobertSchwartzman = 99,
            RoyNissany = 100,
            MarinoSato = 101,
            AidanJackson = 102,
            CasperAkkerman = 103,
            JensonButton = 109,
            DavidCoulthard = 110,
            NicoRosberg = 111,
            Unknown = 255,
        }

        // SETTINGS

        public enum NetworkTypes : byte
        {
            Offline = 0,
            Online = 1,
        }

        public enum ButtonFlags
        {
            CrossOrA = 0x00000001,
            TriangleOrY = 0x00000002,
            CircleOrB = 0x00000004,
            SquareOrX = 0x00000008,
            DpadLeft = 0x00000010,
            DpadRight = 0x00000020,
            DpadUp = 0x00000040,
            DpadDown = 0x00000080,
            OptionsOrMenu = 0x00000100,
            L1orLB = 0x00000200,
            R1orRB = 0x00000400,
            L2orLT = 0x00000800,
            R2orRT = 0x00001000,
            LeftStickClick = 0x00002000,
            RightStickClick = 0x00004000,
            RightStickLeft = 0x00008000,
            RightStickRight = 0x00010000,
            RightStickUp = 0x00020000,
        }

        public static List<ButtonFlags> KeyChecker(UInt32 uint32b)
        {
            List<ButtonFlags> b = new List<ButtonFlags>();

            Appendeces.HasKey(uint32b, ButtonFlags.CrossOrA, ref b);
            Appendeces.HasKey(uint32b, ButtonFlags.CircleOrB, ref b);
            Appendeces.HasKey(uint32b, ButtonFlags.DpadDown, ref b);
            Appendeces.HasKey(uint32b, ButtonFlags.DpadLeft, ref b);
            Appendeces.HasKey(uint32b, ButtonFlags.DpadRight, ref b);
            Appendeces.HasKey(uint32b, ButtonFlags.DpadUp, ref b);
            Appendeces.HasKey(uint32b, ButtonFlags.L1orLB, ref b);
            Appendeces.HasKey(uint32b, ButtonFlags.L2orLT, ref b);
            Appendeces.HasKey(uint32b, ButtonFlags.LeftStickClick, ref b);
            Appendeces.HasKey(uint32b, ButtonFlags.OptionsOrMenu, ref b);
            Appendeces.HasKey(uint32b, ButtonFlags.R1orRB, ref b);
            Appendeces.HasKey(uint32b, ButtonFlags.R2orRT, ref b);
            Appendeces.HasKey(uint32b, ButtonFlags.RightStickClick, ref b);
            Appendeces.HasKey(uint32b, ButtonFlags.RightStickLeft, ref b);
            Appendeces.HasKey(uint32b, ButtonFlags.RightStickRight, ref b);
            Appendeces.HasKey(uint32b, ButtonFlags.RightStickUp, ref b);
            Appendeces.HasKey(uint32b, ButtonFlags.SquareOrX, ref b);
            Appendeces.HasKey(uint32b, ButtonFlags.TriangleOrY, ref b);

            return b;
        }

        private static void HasKey(UInt32 uint32b, ButtonFlags key, ref List<ButtonFlags> b)
        {
            if (((ButtonFlags)uint32b).HasFlag(key)) b.Add(key);
        }

        public static List<LapValid> LapValidChecker(byte val)
        {
            List<LapValid> b = new List<LapValid>();

            Appendeces.IsValid(val, LapValid.FullLapValid, ref b);
            Appendeces.IsValid(val, LapValid.Sector1Valid, ref b);
            Appendeces.IsValid(val, LapValid.Sector2Valid, ref b);
            Appendeces.IsValid(val, LapValid.Sector3Valid, ref b);

            return b;
        }

        private static void IsValid(byte uint32b, LapValid key, ref List<LapValid> b)
        {
            if (((LapValid)uint32b).HasFlag(key)) b.Add(key);
        }

        public enum LapValid
        {
            FullLapValid = 0x01,
            Sector1Valid = 0x02,
            Sector2Valid = 0x04,
            Sector3Valid = 0x08,
        }

        public enum SurfaceTypes : byte
        {
            Tarmac = 0,
            Rumblestrip = 1,
            Concrete = 2,
            Rock = 3,
            Gravel = 4,
            Mud = 5,
            Sand = 6,
            Grass = 7,
            Water = 8,
            Cobblestone = 9,
            Metal = 10,
            Ridged = 11,
        }

        public enum Nationalities : byte
        {
            Unknown = 0,
            American = 1,
            Argentinean = 2,
            Australian = 3,
            Austrian = 4,
            Azerbaijani = 5,
            Bahraini = 6,
            Belgian = 7,
            Bolivian = 8,
            Brazilian = 9,
            British = 10,
            Bulgarian = 11,
            Cameroonian = 12,
            Canadian = 13,
            Chilean = 14,
            Chinese = 15,
            Colombian = 16,
            CostaRican = 17,
            Croatian = 18,
            Cypriot = 19,
            Czech = 20,
            Danish = 21,
            Dutch = 22,
            Ecuadorian = 23,
            English = 24,
            Emirian = 25,
            Estonian = 26,
            Finnish = 27,
            French = 28,
            German = 29,
            Ghanaian = 30,
            Greek = 31,
            Guatemalan = 32,
            Honduran = 33,
            HongKonger = 34,
            Hungarian = 35,
            Icelander = 36,
            Indian = 37,
            Indonesian = 38,
            Irish = 39,
            Israeli = 40,
            Italian = 41,
            Jamaican = 42,
            Japanese = 43,
            Jordanian = 44,
            Kuwaiti = 45,
            Latvian = 46,
            Lebanese = 47,
            Lithuanian = 48,
            Luxembourger = 49,
            Malaysian = 50,
            Maltese = 51,
            Mexican = 52,
            Monegasque = 53,
            NewZealander = 54,
            Nicaraguan = 55,
            NorthernIrish = 56,
            Norwegian = 57,
            Omani = 58,
            Pakistani = 59,
            Panamanian = 60,
            Paraguayan = 61,
            Peruvian = 62,
            Polish = 63,
            Portuguese = 64,
            Qatari = 65,
            Romanian = 66,
            Russian = 67,
            Salvadoran = 68,
            Saudi = 69,
            Scottish = 70,
            Serbian = 71,
            Singaporean = 72,
            Slovakian = 73,
            Slovenian = 74,
            SouthKorean = 75,
            SouthAfrican = 76,
            Spanish = 77,
            Swedish = 78,
            Swiss = 79,
            Thai = 80,
            Turkish = 81,
            Uruguayan = 82,
            Ukrainian = 83,
            Venezuelan = 84,
            Barbadian = 85,
            Welsh = 86,
            Vietnamese = 87,
        }

        public enum PenaltyTypes : byte
        {
            DriveThrough,
            StopGo,
            GridPenalty,
            PenaltyReminder,
            TimePenalty,
            Warning,
            Disqualified,
            RemovedFromFormationLap,
            ParkedTooLongTimer,
            TyreRegulations,
            ThisLapInvalidated,
            ThisAndNextLapInvalidated,
            ThisLapInvalidatedWithoutReason,
            ThisAndNextLapInvalidatedWithoutReason,
            ThisAndPreviousLapInvalidated,
            ThisAndPreviousLapInvalidatedWithoutReason,
            Retired,
            BlackFlagTimer,
        }

        public enum InfringementTypes : byte
        {
            BlockingBySlowDriving,
            BlockingByWrongWayDriving,
            ReversingOffTheStartLine,
            BigCollision,
            SmallCollision,
            CollisionFailedToHandBackPositionSingle,
            CollisionFailedToHandBackPositionMultiple,
            CornerCuttingGainedTime,
            CornerCuttingOvertakeSingle,
            CornerCuttingOvertakeMultiple,
            CrossedPitExitLane,
            IgnoringBlueFlags,
            IgnoringYellowFlags,
            IgnoringDriveThrough,
            TooManyDriveThroughs,
            DriveThroughReminderServeWithinnLaps,
            DriveThroughReminderServeThisLap,
            PitLaneSpeeding,
            ParkedForTooLong,
            IgnoringTyreRegulations,
            TooManyPenalties,
            MultipleWarnings,
            ApproachingDisqualification,
            TyreRegulationsSelectSingle,
            TyreRegulationsSelectMultiple,
            LapInvalidatedCornerCutting,
            LapInvalidatedRunningWide,
            CornerCuttingRanWideGainedTimeMinor,
            CornerCuttingRanWideGainedTimeSignificant,
            CornerCuttingRanWideGainedTimeExtreme,
            LapInvalidatedWallRiding,
            LapInvalidatedFlashbackUsed,
            LapInvalidatedResetToTrack,
            BlockingThePitlane,
            JumpStart,
            SafetyCarToCarCollision,
            SafetyCarIllegalOvertake,
            SafetyCarExceedingAllowedPace,
            VirtualSafetyCarExceedingAllowedPace,
            FormationLapBelowAllowedSpeed,
            RetiredMechanicalFailure,
            RetiredTerminallyDamaged,
            SafetyCarFallingTooFarBack,
            BlackFlagTimer,
            UnservedStopGoPenalty,
            UnservedDriveThroughPenalty,
            EngineComponentChange,
            GearboxChange,
            LeagueGridPenalty,
            RetryPenalty,
            IllegalTimeGain,
            MandatoryPitstop,
        }

        public enum Gears : sbyte
        {
            Gear_R = -1,
            Gear_N = 0,
            Gear_1 = 1,
            Gear_2 = 2,
            Gear_3 = 3,
            Gear_4 = 4,
            Gear_5 = 5,
            Gear_6 = 6,
            Gear_7 = 7,
            Gear_8 = 8,
        }

        public enum Flags : sbyte
        {
            InvalidOrUnknown = -1,
            None = 0,
            Green = 1,
            blue = 2,
            yellow = 3,
            red = 4,
        }

        public enum SessionTypes : byte
        {
            UNKNOWN,
            P1,
            P2,
            P3,
            P_SHORT,
            Q1,
            Q2,
            Q3,
            Q_SHORT,
            Q_ONESHOT,
            RACE,
            RACE_TWO,
            TIME_TRIAL,
        }

        public enum DriverStatus : sbyte
        {
            Unknown = -1,
            In_Garage = 0,
            Flying_Lap = 1,
            In_Lap = 2,
            Out_Lap = 3,
            On_Track = 4,
        }

        public enum PitStatus : sbyte
        {
            Unknown = -1,
            None = 0,
            In_This_Lap = 1,
            In_Pit_Lane = 2,
        }

        public enum ResultStatus : sbyte
        {
            Unknown = -1,
            Invalid = 0,
            Inactive = 1,
            Active = 2,
            Finished = 3,
            DNF = 4,
            DSQ = 5,
            Not_Classified = 6,
            Retired = 7,
        }


        public enum TyreCompounds : byte
        {
            Hyper_Soft,
            Ultra_Soft,
            Super_Soft,
            Soft_,
            Medium_,
            Hard_,
            SuperHard_,
            Inter,
            Wet,
            ClassicDry,
            ClassicWet,
            F2SuperSoft,
            F2Soft,
            F2Medium,
            F2Hard,
            F2Wet,
            C5,
            C4,
            Soft,
            Medium,
            Hard,
        }

        public enum VisualTireCompounds : byte
        {                                    
            Inter = 7,
            Wet = 8,
            Soft = 16,
            Medium = 17,
            Hard = 18,
            F2_Wet = 15,
            F2_SuperSoft = 19,
            F2_Soft = 20,
            F2_Medium = 21,
            F2_Hard = 22
        }

        public enum BreakingAssistSettings
        {
            Unknown = -1,
            off = 0,
            Low = 1,
            Medium = 2,
            High = 3,
        }

        public enum ForecastAccuracy
        {
            Perfect,
            Approximate,
        }

        public enum WeatherTypes : byte
        {
            Clear,
            LightCloud,
            Overcast,
            LightRain,
            HeavyRain,
            Storm,
        }

        

        public enum Formulas : byte
        {
            F1_Realistic,
            F1_Classic,
            F2,
            F1_Equal,
        }

        public enum SafetyCarStatus : byte
        {
            Clear,
            SafetyCarActive,
            VirtualSafetyCar,
            FormationLap,
        }

        public enum GearboxAssists
        {
            Unknown = -1,
            Manual = 1,
            ManualAndSuggestedGear = 3,
            auto = 3,
        }

        public enum RacingLineStatus
        {
            Off,
            CornersOnly,
            Full,
        }

        public enum RacingLineTypes
        {
            Line_2D,
            Line_3D
        }

        

        public enum Sectors : sbyte
        {
            Unknown = -1,
            Sector1 = 0,
            Sector2 = 1,
            Sector3 = 2,
        }

        

        public enum EventTypes : byte
        {
            Unknown,
            SessionStarted,
            SessionEnded,
            FastestLap,
            Retirement,
            DRSenabled,
            DRSdisabled,
            TeamMateInPits,
            ChequeredFlag,
            RaceWinner,
            PenaltyIssued,
            SpeedTrapTriggered,
            StartLights,
            LightsOut,
            DriveThroughServed,
            StopGoServed,
            Flashback,
            ButtonStatus,
        }

        public enum TelemetrySettings : byte
        {
            Restricted,
            Public,
        }

        public enum ErsDeployMode : byte
        {
            None = 0,
            Medium = 1,
            HotLap = 2,
            Overtake = 3,
        }

        public enum TractionControlSettings : byte
        {
            Off,
            Medium,
            Full,
        }

        public enum FuelMix : byte
        {
            Lean,
            Standard,
            Rich,
            Max,
        }

        public enum DRSStatus : sbyte
        {
            Unknown = -1,
            NotAllowed = 0,
            Allowed = 1,
        }

        public enum ReadyStatus : byte
        {
            NotReady,
            Ready,
            Spectating,
        }

        public enum TemperatureChanges : byte
        {
            Up,
            Down,
            NoChange,
        }

        public enum MFDPanels
        {
            CarSetup = 0,
            Pits = 1,
            Demage = 2,
            Engine = 3,
            Temperatures = 4,
            Closed = 255,
        }
    }
}
