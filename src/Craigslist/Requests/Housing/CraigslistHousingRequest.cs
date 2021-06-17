using System;
using System.Collections.Generic;
using System.Linq;

namespace Craigslist
{
    public abstract class CraigslistHousingRequest : CraigslistRequest
    {
        private const string QP_SRCHTYPE = "srchType";
        private const string QP_HASPIC = "hasPic";
        private const string QP_POSTEDTODAY = "postedToday";
        private const string QP_BUNDLEDUPLICATES = "bundleDuplicates";
        
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
        
        public CraigslistHousingRequest(string site, string category) : base(site, category) {}
        public CraigslistHousingRequest(string site, string? area, string category) : base(site, area, category) {}
        internal CraigslistHousingRequest(string site, string? area, string category, IDictionary<string, object> parameterStore) : base(site, area, category, parameterStore) {}
    }
}