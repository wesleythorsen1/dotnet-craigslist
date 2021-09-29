using System.Collections.Generic;

namespace DotnetCraigslist
{
    public class SearchCommunityRequest : SearchRequest
    {
        private const string QP_LOSTORFOUNDTYPE = "lost_and_found_type";
        private const string QP_RIDESHARETYPE = "rideshare_type";

        public static class Categories
        {
            public const string All = "ccc";
            public const string ActivityPartners = "act";
            public const string Artists = "ats";
            public const string Childcare = "kid";
            public const string GeneralCommunity = "com";
            public const string Groups = "grp";
            public const string LocalNewsAndViews = "vnn";
            public const string LostAndFound = "laf";
            public const string MissedConnections = "mis";
            public const string Musicians = "muc";
            public const string Pets = "pet";
            public const string Politics = "pol";
            public const string RantsAndRaves = "rnr";
            public const string Rideshare = "rid";
            public const string Volunteers = "vol";
        }
        
        public SearchCommunityRequest(string site) : base(site, SearchCommunityRequest.Categories.All) {}
        public SearchCommunityRequest(string site, string category) : base(site, category) {}
        public SearchCommunityRequest(string site, string? area, string category) : base(site, area, category) {}
        internal SearchCommunityRequest(string site, string? area, string category, IDictionary<string, object> parameterStore) : base(site, area, category, parameterStore) {}

        public enum LostOrFound
        {
            Lost = 1,
            Found = 2,
        }

        public enum RideshareType
        {
            RideOffered = 1,
            RideWanted = 2,
        }

        public IEnumerable<LostOrFound>? LostOrFoundTypes
        {
            get => GetParameter<IEnumerable<LostOrFound>>(QP_LOSTORFOUNDTYPE);
            set => SetParameter(QP_LOSTORFOUNDTYPE, value);
        }

        public IEnumerable<RideshareType>? RideshareTypes
        {
            get => GetParameter<IEnumerable<RideshareType>>(QP_RIDESHARETYPE);
            set => SetParameter(QP_RIDESHARETYPE, value);
        }
    }
}