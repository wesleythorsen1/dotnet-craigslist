using System.Collections.Generic;

namespace DotnetCraigslist
{
    public class SearchForSaleRequest : SearchRequest
    {
        private const string QP_PURVEYOR = "purveyor";
        private const string QP_MINPRICE = "min_price";
        private const string QP_MAXPRICE = "max_price";
        private const string QP_MAKEMODEL = "auto_make_model";
#region cars and trucks
        private const string QP_MINMODELYEAR = "min_auto_year";
        private const string QP_MAXMODELYEAR = "max_auto_year";
        private const string QP_MINODOMETER = "min_auto_miles";
        private const string QP_MAXODOMETER = "max_auto_miles";
        private const string QP_CONDITION = "condition";
        private const string QP_AUTOCYLINDERS = "auto_cylinders";
        private const string QP_DRIVETRAIN = "auto_drivetrain";
        private const string QP_FUELTYPE = "auto_fuel_type";
        private const string QP_PAINTCOLOR = "auto_paint";
        private const string QP_AUTOSIZE = "auto_size";
        private const string QP_TITLESTATUS = "auto_title_status";
        private const string QP_TRANSMISSION = "auto_transmission";
        private const string QP_BODYTYPE = "auto_bodytype";
#endregion
#region bikes
        private const string QP_BICYCLETYPE = "bicycle_type";
        private const string QP_BICYCLEFRAMEMATERIAL = "bicycle_frame_material";
        private const string QP_BICYCLEWHEELSIZE = "bicycle_wheel_size";
        private const string QP_BICYCLESUSPENSION = "bicycle_suspension";
        private const string QP_BICYCLEBRAKETYPE = "bicycle_brake_type";
        private const string QP_BICYCLEHANDLEBARTYPE = "bicycle_handlebar_type";
        private const string QP_BICYCLEELECTRICASSIST = "bicycle_electric_assist";
#endregion
#region boats
        private const string QP_BOATPROPULSIONTYPE = "boat_propulsion_type";
#endregion    
#region cell phones
        private const string QP_MOBILEOS = "mobile_os";
#endregion
#region rvs
        private const string QP_RVTYPE = "rv_type";
#endregion
#region motorcycles
        private const string QP_MOTORCYCLESTREETLEGAL = "motorcycle_street_legal";
        private const string QP_MOTORCYCLEMINDISPLACEMENT = "min_engine_displacement_cc";
        private const string QP_MOTORCYCLEMAXDISPLACEMENT = "max_engine_displacement_cc";
        private const string QP_MOTORCYCLETYPE = "motorcycle_type";
#endregion
        private const string QP_CRYPTOCURRENCYOK = "crypto_currency_ok";
        private const string QP_DELIVERYAVAILABLE = "delivery_available";
        private const string QP_LANGUAGE = "language";

        public static class Categories
        {
            public const string All = "sss";
            public const string Antiques = "ata";
            public const string Appliances = "ppa";
            public const string ArtsAndCrafts = "ara";
            public const string AtvsUtvsSnowmobiles = "sna";
            public const string AutoParts = "pta";
            public const string AutoWheelsAndTires = "wta";
            public const string Aviation = "ava";
            public const string BabyAndKidStuff = "baa";
            public const string Barter = "bar";
            public const string BicycleParts = "bip";
            public const string Bicycles = "bia";
            public const string BoatPartsAndAccessories = "bpa";
            public const string Boats = "boo";
            public const string BooksAndMagazines = "bka";
            public const string Business = "bfa";
            public const string CarsAndTrucks = "cta";
            public const string CdsDvdsVhs = "ema";
            public const string CellPhones = "moa";
            public const string ClothingAndAccessories = "cla";
            public const string Collectibles = "cba";
            public const string ComputerParts = "syp";
            public const string Computers = "sya";
            public const string Electronics = "ela";
            public const string FarmAndGarden = "gra";
            public const string FreeStuff = "zip";
            public const string Furniture = "fua";
            public const string GarageMovingSales = "gms";
            public const string GeneralForSale = "foa";
            public const string HealthAndBeauty = "haa";
            public const string HeavyEquipment = "hva";
            public const string HouseholdItems = "hsa";
            public const string Jewelry = "jwa";
            public const string Materials = "maa";
            public const string MotorcyclePartsAndAccessories = "mpa";
            public const string MotorcyclesScooters = "mca";
            public const string MusicalInstruments = "msa";
            public const string PhotoVideo = "pha";
            public const string RecreationalVehicles = "rva";
            public const string SportingGoods = "sga";
            public const string Tickets = "tia";
            public const string Tools = "tla";
            public const string ToysAndGames = "taa";
            public const string Trailers = "tra";
            public const string VideoGaming = "vga";
            public const string Wanted = "waa";
        }

        public SearchForSaleRequest(string site) : base(site, SearchForSaleRequest.Categories.All) {}
        public SearchForSaleRequest(string site, string category) : base(site, category) {}
        public SearchForSaleRequest(string site, string? area, string category) : base(site, area, category) {}
        internal SearchForSaleRequest(string site, string? area, string category, IDictionary<string, object> parameterStore) : base(site, area, category, parameterStore) {}

        public enum Purveyors
        {
            [QueryStringValue("all")]
            All = 0,

            [QueryStringValue("owner")]
            Owner,

            [QueryStringValue("dealer")]
            Dealer,
        }
        
        public enum Condition
        {
            New = 10,
            LikeNew = 20,
            Excellent = 30,
            Good = 40,
            Fair = 50,
            Salvage = 60,
        }

#region cars and trucks

        public enum Cylinders
        {
            ThreeCylinders = 1,
            FourCylinders = 2,
            FiveCylinders = 3,
            SixCylinders = 4,
            EightCylinders = 5,
            TenCylinders = 6,
            TwelveCylinders = 7,
            Other = 8,
        }

        public enum Drive
        {
            FWD = 1,
            RWD = 2,
            _4WD = 3,
        }

        public enum Fuel
        {
            Gas = 1,
            Diesel = 2,
            Hybrid = 3,
            Electric = 4,
            Other = 5,
        }

        public enum PaintColor
        {
            Black = 1,
            Blue = 2,
            Green = 3,
            Grey = 4,
            Orange = 5,
            Purple = 6,
            Red = 7,
            Silver = 8,
            White = 9,
            Yellow = 10,
            Custom = 11,
            Brown = 20,
        }

        public enum Size
        {
            Compact = 1,
            FullSize = 2,
            MidSize = 3,
            SubCompact = 4,
        }

        public enum TitleStatus
        {
            Clean = 1,
            Salvage = 2,
            Rebuilt = 3,
            PartsOnly = 4,
            Lien = 5,
            Missing = 6,
        }

        public enum Transmission
        {
            Manual = 1,
            Automatic = 2,
            Other = 3,
        }

        public enum BodyType
        {
            Bus = 1,
            Convertible = 2,
            Coupe = 3,
            Hatchback = 4,
            MiniVan = 5,
            Offroad = 6,
            Pickup = 7,
            Sedan = 8,
            Truck = 9,
            SUV = 10,
            Wagon = 11,
            Van = 12,
            Other = 13,
        }

#endregion

#region bikes

        public enum BicycleType
        {
            Bmx = 1,
            CargoPedicab = 2,
            Cruiser = 3,
            Cyclocross = 4,
            Folding = 5,
            HybridComfort = 6,
            Kids = 7,
            Mountain = 8,
            RecumbentTrike = 9,
            Road = 10,
            Tandem = 11,
            Track = 12,
            Unicycle = 13,
            Other = 14,
            Gravel = 15,
        }

        public enum BicycleFrameMaterial
        {
            Alloy = 1,
            Aluminum = 2,
            CarbonFiber = 3,
            Composite = 4,
            Scandium = 5,
            Steel = 6,
            Titanium = 7,
            OtherUnknown = 8,
        }

        public enum WheelSize
        {
            _10In = 1,
            _12In = 2,
            _14In = 3,
            _16In = 4,
            _18In = 5,
            _20In = 6,
            _24In = 7,
            _25In = 8,
            _26In = 9,
            _26_5In = 10,
            _27In = 11,
            _27_5In = 12,
            _28In = 13,
            _29In = 14,
            _650B = 15,
            _650C = 16,
            _700C = 17,
            OtherUnknown = 18,
        }

        public enum Suspension
        {
            None_Rigid = 1,
            SuspensionFork_Hardtail = 2,
            FrameAndFork_FullSuspension = 3,
            OtherUnknown = 4,
        }

        public enum BrakeType
        {
            Caliper = 1,
            Cantilever = 2,
            Coaster = 3,
            DiscHydraulic = 4,
            DiscMechanical = 5,
            Drum = 6,
            GyroBmx = 7,
            HydraulicRimBrakes = 8,
            None = 9,
            UBrakes = 10,
            VBrakes = 11,
            OtherUnknown = 12,
        }

        public enum HandlebarType
        {
            Aero = 1,
            Bmx = 2,
            Bullhorn = 3,
            Cruiser = 4,
            Downhill = 5,
            Drop = 6,
            Flat = 7,
            Riser = 8,
            Triathlon = 9,
            OtherUnknown = 10,
        }

        public enum ElectricAssist
        {
            None = 1,
            PedalAssist = 2,
            Throttle = 3,
            Other = 4,
        }

#endregion

#region boats

        public enum PropulsionType
        {
            Sail = 1,
            Power = 2,
            Human = 3,
        }

#endregion

#region cell phones

        public enum MobileOS
        {
            Android = 1,
            AppleIos = 2,
            Blackberry = 3,
            WindowsMobile = 4,
            Other = 5,
        }

#endregion

#region rvs

        public enum RvType
        {
            ClassA = 1,
            ClassB = 2,
            ClassC = 3,
            FifthWheelTrailer = 4,
            TravelTrailer = 5,
            HybridTrailer = 6,
            FoldingPopupTrailer = 7,
            TeardropCompactTrailer = 8,
            ToyHauler = 9,
            TruckCamper = 10,
            Other = 11,
        }

#endregion

#region motorcycles

        public enum MotorcycleType
        {
            Bobber = 1,
            CafeRace = 2,
            Chopper = 3,
            Cruiser = 4,
            Dirtbike = 5,
            DualSport = 6,
            Moped = 7,
            Scooter = 8,
            SportBike = 9,
            SportTouring = 10,
            Standard = 11,
            Street = 12,
            Touring = 13,
            Trike = 14,
            Other = 15
        }

#endregion

        public enum Language
        {
            Afrikaans = 1,
            Català = 2,
            Dansk = 3,
            Deutsch = 4,
            English = 5,
            Español = 6,
            Suomi = 7,
            Français = 8,
            Italiano = 9,
            Nederlands = 10,
            Norsk = 11,
            Português = 12,
            Svenska = 13,
            Filipino = 14,
            Türkçe = 15,
            中文 = 16,
            العربية = 17,
            日本語 = 18,
            한국말 = 19,
            Pусский = 20,
            TiếngViệt = 21,
        }

        public Purveyors Purveyor
        {
            get => GetParameter<Purveyors>(QP_PURVEYOR);
            set => SetParameter(QP_PURVEYOR, value);
        }

        public int? MinPrice
        {
            get => GetParameter<int?>(QP_MINPRICE);
            set => SetParameter(QP_MINPRICE, value);
        }

        public int? MaxPrice
        {
            get => GetParameter<int?>(QP_MAXPRICE);
            set => SetParameter(QP_MAXPRICE, value);
        }

        public string? MakeModel
        {
            get => GetParameter<string?>(QP_MAKEMODEL);
            set => SetParameter(QP_MAKEMODEL, value);
        }

#region cars and trucks

        public int? MinModelYear
        {
            get => GetParameter<int?>(QP_MINMODELYEAR);
            set => SetParameter(QP_MINMODELYEAR, value);
        }

        public int? MaxModelYear
        {
            get => GetParameter<int?>(QP_MAXMODELYEAR);
            set => SetParameter(QP_MAXMODELYEAR, value);
        }

        public int? MinOdometer
        {
            get => GetParameter<int?>(QP_MINODOMETER);
            set => SetParameter(QP_MINODOMETER, value);
        }

        public int? MaxOdometer
        {
            get => GetParameter<int?>(QP_MAXODOMETER);
            set => SetParameter(QP_MAXODOMETER, value);
        }

        public IEnumerable<Condition>? Conditions
        {
            get => GetParameter<IEnumerable<Condition>>(QP_CONDITION);
            set => SetParameter(QP_CONDITION, value);
        }

        public IEnumerable<Cylinders>? AutoCylinders
        {
            get => GetParameter<IEnumerable<Cylinders>>(QP_AUTOCYLINDERS);
            set => SetParameter(QP_AUTOCYLINDERS, value);
        }

        public IEnumerable<Drive>? DriveTrains
        {
            get => GetParameter<IEnumerable<Drive>>(QP_DRIVETRAIN);
            set => SetParameter(QP_DRIVETRAIN, value);
        }

        public IEnumerable<Fuel>? FuelTypes
        {
            get => GetParameter<IEnumerable<Fuel>>(QP_FUELTYPE);
            set => SetParameter(QP_FUELTYPE, value);
        }

        public IEnumerable<PaintColor>? PaintColors
        {
            get => GetParameter<IEnumerable<PaintColor>>(QP_PAINTCOLOR);
            set => SetParameter(QP_PAINTCOLOR, value);
        }

        public IEnumerable<Size>? AutoSizes
        {
            get => GetParameter<IEnumerable<Size>>(QP_AUTOSIZE);
            set => SetParameter(QP_AUTOSIZE, value);
        }

        public IEnumerable<TitleStatus>? TitleStatuses
        {
            get => GetParameter<IEnumerable<TitleStatus>>(QP_TITLESTATUS);
            set => SetParameter(QP_TITLESTATUS, value);
        }

        public IEnumerable<Transmission>? Transmissions
        {
            get => GetParameter<IEnumerable<Transmission>>(QP_TRANSMISSION);
            set => SetParameter(QP_TRANSMISSION, value);
        }

        public IEnumerable<BodyType>? BodyTypes
        {
            get => GetParameter<IEnumerable<BodyType>>(QP_BODYTYPE);
            set => SetParameter(QP_BODYTYPE, value);
        }

#endregion cars and trucks

#region bikes

        public IEnumerable<BicycleType>? BicycleTypes
        {
            get => GetParameter<IEnumerable<BicycleType>>(QP_BICYCLETYPE);
            set => SetParameter(QP_BICYCLETYPE, value);
        }

        public IEnumerable<BicycleFrameMaterial>? BicycleFrameMaterials
        {
            get => GetParameter<IEnumerable<BicycleFrameMaterial>>(QP_BICYCLEFRAMEMATERIAL);
            set => SetParameter(QP_BICYCLEFRAMEMATERIAL, value);
        }

        public IEnumerable<WheelSize>? BicycleWheelSizes
        {
            get => GetParameter<IEnumerable<WheelSize>>(QP_BICYCLEWHEELSIZE);
            set => SetParameter(QP_BICYCLEWHEELSIZE, value);
        }

        public IEnumerable<Suspension>? BicycleSuspensions
        {
            get => GetParameter<IEnumerable<Suspension>>(QP_BICYCLESUSPENSION);
            set => SetParameter(QP_BICYCLESUSPENSION, value);
        }

        public IEnumerable<BrakeType>? BicycleBrakeTypes
        {
            get => GetParameter<IEnumerable<BrakeType>>(QP_BICYCLEBRAKETYPE);
            set => SetParameter(QP_BICYCLEBRAKETYPE, value);
        }

        public IEnumerable<HandlebarType>? BicycleHandlebarTypes
        {
            get => GetParameter<IEnumerable<HandlebarType>>(QP_BICYCLEHANDLEBARTYPE);
            set => SetParameter(QP_BICYCLEHANDLEBARTYPE, value);
        }

        public IEnumerable<ElectricAssist>? BicycleElectricAssists
        {
            get => GetParameter<IEnumerable<ElectricAssist>>(QP_BICYCLEELECTRICASSIST);
            set => SetParameter(QP_BICYCLEELECTRICASSIST, value);
        }

#endregion

#region boats

        public IEnumerable<PropulsionType>? BoatPropulsionTypes
        {
            get => GetParameter<IEnumerable<PropulsionType>>(QP_BOATPROPULSIONTYPE);
            set => SetParameter(QP_BOATPROPULSIONTYPE, value);
        }

#endregion

#region cell phones

        public IEnumerable<MobileOS>? MobileOSs
        {
            get => GetParameter<IEnumerable<MobileOS>>(QP_MOBILEOS);
            set => SetParameter(QP_MOBILEOS, value);
        }

#endregion

#region rvs

        public IEnumerable<RvType>? RvTypes
        {
            get => GetParameter<IEnumerable<RvType>>(QP_RVTYPE);
            set => SetParameter(QP_RVTYPE, value);
        }

#endregion

#region motorcycles

        public bool StreetLegal
        {
            get => GetParameter<bool>(QP_MOTORCYCLESTREETLEGAL);
            set => SetParameter(QP_MOTORCYCLESTREETLEGAL, value);
        }

        public int? MinEngineDisplacement
        {
            get => GetParameter<int?>(QP_MOTORCYCLEMINDISPLACEMENT);
            set => SetParameter(QP_MOTORCYCLEMINDISPLACEMENT, value);
        }

        public int? MaxEngineDisplacement
        {
            get => GetParameter<int?>(QP_MOTORCYCLEMAXDISPLACEMENT);
            set => SetParameter(QP_MOTORCYCLEMAXDISPLACEMENT, value);
        }

        public IEnumerable<MotorcycleType>? MotorcycleTypes
        {
            get => GetParameter<IEnumerable<MotorcycleType>>(QP_MOTORCYCLETYPE);
            set => SetParameter(QP_MOTORCYCLETYPE, value);
        }

#endregion

        public bool CryptoCurrencyOk
        {
            get => GetParameter<bool>(QP_CRYPTOCURRENCYOK);
            set => SetParameter(QP_CRYPTOCURRENCYOK, value);
        }

        public bool DeliveryAvailable
        {
            get => GetParameter<bool>(QP_DELIVERYAVAILABLE);
            set => SetParameter(QP_DELIVERYAVAILABLE, value);
        }

        public IEnumerable<Language>? Languages
        {
            get => GetParameter<IEnumerable<Language>>(QP_LANGUAGE);
            set => SetParameter(QP_LANGUAGE, value);
        }
    }
}