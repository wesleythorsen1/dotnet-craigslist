namespace DotnetCraigslist
{
    public partial class SearchRequest
    {
        private const string QP_SKIP = "s";
        private const string QP_SORT = "sort";
        private const string QP_QUERY = "query";
        private const string QP_SRCHTYPE = "srchType";
        private const string QP_HASPIC = "hasPic";
        private const string QP_POSTEDTODAY = "postedToday";
        private const string QP_BUNDLEDUPLICATES = "bundleDuplicates";
        private const string QP_SEARCHNEARBY = "searchNearby";
        private const string QP_SEARCHDISTANCE = "search_distance";
        private const string QP_POSTALCODE = "postal";

        public enum SortOrder
        {
            [QueryStringValue("upcoming")]
            Upcoming,

            [QueryStringValue("date")]
            Newest,

            [QueryStringValue("dateoldest")]
            Oldest,

            [QueryStringValue("priceasc")]
            PriceAscending,

            [QueryStringValue("pricedsc")]
            PriceDescending,

            [QueryStringValue("dist")]
            Distance,
        }

        public int Skip
        {
            get => GetParameter<int>(QP_SKIP);
            set => SetParameter(QP_SKIP, value);
        }
        
        public SortOrder? Sort
        {
            get => GetParameter<SortOrder?>(QP_SORT);
            set => SetParameter(QP_SORT, value);
        }

        public string? SearchText
        {
            get => GetParameter<string?>(QP_QUERY);
            set => SetParameter(QP_QUERY, value);
        }
        
        public bool SearchTitlesOnly
        {
            get => GetParameter<bool>(QP_SRCHTYPE);
            set => SetParameter(QP_SRCHTYPE, value);
        }
        
        public bool HasImage
        {
            get => GetParameter<bool>(QP_HASPIC);
            set => SetParameter(QP_HASPIC, value);
        }
        
        public bool PostedToday
        {
            get => GetParameter<bool>(QP_POSTEDTODAY);
            set => SetParameter(QP_POSTEDTODAY, value);
        }
        
        public bool BundleDuplicates
        {
            get => GetParameter<bool>(QP_BUNDLEDUPLICATES);
            set => SetParameter(QP_BUNDLEDUPLICATES, value);
        }
        
        public bool IncludeNearbyAreas
        {
            get => GetParameter<bool>(QP_SEARCHNEARBY);
            set => SetParameter(QP_SEARCHNEARBY, value);
        }

        public float? SearchDistance
        {
            get => GetParameter<float?>(QP_SEARCHDISTANCE);
            set => SetParameter(QP_SEARCHDISTANCE, value);
        }

        public string? PostalCode
        {
            get => GetParameter<string?>(QP_POSTALCODE);
            set => SetParameter(QP_POSTALCODE, value);
        }
    }
}