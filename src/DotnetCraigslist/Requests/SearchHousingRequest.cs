using System;
using System.Collections.Generic;

namespace DotnetCraigslist
{
    public class SearchHousingRequest : SearchRequest
    {
        private const string QP_MINPRICE = "min_price";
        private const string QP_MAXPRICE = "max_price";
        private const string QP_MINBEDROOMS = "min_bedrooms";
        private const string QP_MAXBEDROOMS = "max_bedrooms";
        private const string QP_MINBATHROOMS = "min_bathrooms";
        private const string QP_MAXBATHROOMS = "max_bathrooms";
        private const string QP_MINSQFT = "minSqft";
        private const string QP_MAXSQFT = "maxSqft";
        private const string QP_AVAILABILITYMODE = "availabilityMode";
        private const string QP_PRIVATEROOM = "private_room";
        private const string QP_CATSOK = "pets_cat";
        private const string QP_PRIVATEBATH = "private_bath";
        private const string QP_DOGSOK = "pets_dog";
        private const string QP_FURNISHED = "is_furnished";
        private const string QP_NOSMOKING = "no_smoking";
        private const string QP_WHEELCHAIRACCESS = "wheelchaccess";
        private const string QP_AIRCONDITIONING = "airconditioning";
        private const string QP_EVCHARGING = "ev_charging";
        private const string QP_NOAPPLICATIONFEE = "application_fee";
        private const string QP_NOBROKERFEE = "broker_fee";
        private const string QP_HOUSINGTYPE = "housing_type";
        private const string QP_LAUNDRY = "laundry";
        private const string QP_PARKING = "parking";
        private const string QP_RENTPERIOD = "rent_period";
        private const string QP_SALEDATE = "sale_date";

        public static class Categories
        {
            public const string All = "hhh";
            public const string ApartmentsHousingForRent = "apa";
            public const string HousingSwap = "swp";
            public const string OfficeAndCommercial = "off";
            public const string ParkingAndStorage = "prk";
            public const string RealEstateByBroker = "reb";
            public const string RealEstateByOwner = "reo";
            public const string RoomsAndShares = "roo";
            public const string SubletsAndTemporary = "sub";
            public const string VacationRentals = "vac";
            public const string WantedApartments = "hou";
            public const string WantedRealEstate = "rew";
            public const string WantedRoomShare = "sha";
            public const string WantedSubletTemp = "sbw";
        }
        
        public SearchHousingRequest(string site) : base(site, SearchHousingRequest.Categories.All) {}
        public SearchHousingRequest(string site, string category) : base(site, category) {}
        public SearchHousingRequest(string site, string? area, string category) : base(site, area, category) {}
        internal SearchHousingRequest(string site, string? area, string category, IDictionary<string, object> parameterStore) : base(site, area, category, parameterStore) {}

        public enum AvailabilityMode
        {
            AllDates = 0,
            Within30Days = 1,
            Beyond30Days = 2,
        }

        public enum HousingType
        {
            Apartment = 1,
            Condo = 2,
            CottageCabin = 3,
            Duplex = 4,
            Flat = 5,
            House = 6,
            InLaw = 7,
            Loft = 8,
            Townhouse = 9,
            Manufactured = 10,
            AssistedLiving = 11,
            Land = 12
        }

        public enum Laundry
        {
            InUnit = 1,
            Hookups = 2,
            InBuilding = 3,
            OnSite = 4,
            NoneOnSite = 5
        }

        public enum Parking
        {
            Carport = 1,
            AttachedGarage = 2,
            DetachedGarage = 3,
            OffStreet = 4,
            Street = 5,
            Valet = 6,
            None = 7
        }

        public enum RentPeriod
        {
            Daily = 1,
            Weekly = 2,
            Monthly = 3,
            Yearly = 4
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

        public int? MinBedrooms
        {
            get => GetParameter<int?>(QP_MINBEDROOMS);
            set => SetParameter(QP_MINBEDROOMS, value);
        }

        public int? MaxBedrooms
        {
            get => GetParameter<int?>(QP_MAXBEDROOMS);
            set => SetParameter(QP_MAXBEDROOMS, value);
        }

        public int? MinBathrooms
        {
            get => GetParameter<int?>(QP_MINBATHROOMS);
            set => SetParameter(QP_MINBATHROOMS, value);
        }

        public int? MaxBathrooms
        {
            get => GetParameter<int?>(QP_MAXBATHROOMS);
            set => SetParameter(QP_MAXBATHROOMS, value);
        }

        public int? MinSquareFeet
        {
            get => GetParameter<int?>(QP_MINSQFT);
            set => SetParameter(QP_MINSQFT, value);
        }

        public int? MaxSquareFeet
        {
            get => GetParameter<int?>(QP_MAXSQFT);
            set => SetParameter(QP_MAXSQFT, value);
        }

        public AvailabilityMode? Availability
        {
            get => GetParameter<AvailabilityMode?>(QP_AVAILABILITYMODE);
            set => SetParameter(QP_AVAILABILITYMODE, value);
        }

        public bool PrivateRoom
        {
            get => GetParameter<bool>(QP_PRIVATEROOM);
            set => SetParameter(QP_PRIVATEROOM, value);
        }

        public bool CatsOk
        {
            get => GetParameter<bool>(QP_CATSOK);
            set => SetParameter(QP_CATSOK, value);
        }

        public bool PrivateBath
        {
            get => GetParameter<bool>(QP_PRIVATEBATH);
            set => SetParameter(QP_PRIVATEBATH, value);
        }

        public bool DogsOk
        {
            get => GetParameter<bool>(QP_DOGSOK);
            set => SetParameter(QP_DOGSOK, value);
        }

        public bool Furnished
        {
            get => GetParameter<bool>(QP_FURNISHED);
            set => SetParameter(QP_FURNISHED, value);
        }

        public bool NoSmoking
        {
            get => GetParameter<bool>(QP_NOSMOKING);
            set => SetParameter(QP_NOSMOKING, value);
        }

        public bool WheelchairAccess
        {
            get => GetParameter<bool>(QP_WHEELCHAIRACCESS);
            set => SetParameter(QP_WHEELCHAIRACCESS, value);
        }

        public bool AirConditioning
        {
            get => GetParameter<bool>(QP_AIRCONDITIONING);
            set => SetParameter(QP_AIRCONDITIONING, value);
        }

        public bool EVCharging
        {
            get => GetParameter<bool>(QP_EVCHARGING);
            set => SetParameter(QP_EVCHARGING, value);
        }

        public bool NoApplicationFee
        {
            get => GetParameter<bool>(QP_NOAPPLICATIONFEE);
            set => SetParameter(QP_NOAPPLICATIONFEE, value);
        }

        public bool NoBrokerFee
        {
            get => GetParameter<bool>(QP_NOBROKERFEE);
            set => SetParameter(QP_NOBROKERFEE, value);
        }

        public IEnumerable<HousingType>? HousingTypes
        {
            get => GetParameter<IEnumerable<HousingType>>(QP_HOUSINGTYPE);
            set => SetParameter(QP_HOUSINGTYPE, value);
        }

        public IEnumerable<Laundry>? LaundryOptions
        {
            get => GetParameter<IEnumerable<Laundry>>(QP_LAUNDRY);
            set => SetParameter(QP_LAUNDRY, value);
        }

        public IEnumerable<Parking>? ParkingOptions
        {
            get => GetParameter<IEnumerable<Parking>>(QP_PARKING);
            set => SetParameter(QP_PARKING, value);
        }

        public IEnumerable<RentPeriod>? RentPeriods
        {
            get => GetParameter<IEnumerable<RentPeriod>>(QP_RENTPERIOD);
            set => SetParameter(QP_RENTPERIOD, value);
        }

        public DateTime? OpenHouseDate
        {
            get => GetParameter<DateTime?>(QP_SALEDATE);
            set => SetParameter(QP_SALEDATE, value);
        }
    }
}