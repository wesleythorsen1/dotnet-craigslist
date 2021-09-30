using System.Collections.Generic;

namespace DotnetCraigslist
{
    public class SearchServicesRequest : SearchRequest
    {
        public static class Categories
        {
            public const string All = "bbb";
            public const string BeautyServices = "bts";
            public const string CellPhoneMobileServices = "cms";
            public const string ComputerServices = "cps";
            public const string CreativeServices = "crs";
            public const string CycleServices = "cys";
            public const string EventServices = "evs";
            public const string FarmAndGardenServices = "fgs";
            public const string FinancialServices = "fns";
            public const string HealthWellnessServices = "hws";
            public const string HouseholdServices = "hss";
            public const string LaborHaulingMoving = "lbs";
            public const string LegalServices = "lgs";
            public const string LessonsAndTutoring = "lss";
            public const string MarineServices = "mas";
            public const string PetServices = "pas";
            public const string RealEstateServices = "rts";
            public const string SkilledTradeServices = "sks";
            public const string SmallBizAds = "biz";
            public const string TravelVacationServices = "trv";
            public const string WritingEditingTranslation = "wet";
        }
        
        public SearchServicesRequest(string site) : base(site, SearchServicesRequest.Categories.All) {}
        public SearchServicesRequest(string site, string category) : base(site, category) {}
        public SearchServicesRequest(string site, string? area, string category) : base(site, area, category) {}
        internal SearchServicesRequest(string site, string? area, string category, IDictionary<string, object> parameterStore) : base(site, area, category, parameterStore) {}
    }
}