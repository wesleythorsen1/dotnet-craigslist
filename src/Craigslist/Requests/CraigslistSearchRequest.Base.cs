namespace Craigslist
{
    public partial class CraigslistSearchRequest
    {
        private const string QP_SKIP = "s";
        private const string QP_QUERY = "query";
        private const string QP_SRCHTYPE = "srchType";
        private const string QP_HASPIC = "hasPic";
        private const string QP_POSTEDTODAY = "postedToday";
        private const string QP_BUNDLEDUPLICATES = "bundleDuplicates";
        private const string QP_SEARCHNEARBY = "searchNearby";
        
        public int? Skip
        {
            get => GetParameter<int>(QP_SKIP);
            set => SetParameter(QP_SKIP, value);
        }

        public string? SearchText
        {
            get => GetParameter<string>(QP_QUERY);
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
    }
}