using System;
using System.Collections.Generic;

namespace DotnetCraigslist
{
    public class SearchGigsRequest : SearchRequest
    {
        public static class Categories
        {
            public const string All = "ggg";
            public const string ComputerGigs = "cpg";
            public const string CreativeGigs = "crg";
            public const string CrewGigs = "cwg";
            public const string DomesticGigs = "dmg";
            public const string EventGigs = "evg";
            public const string LaborGigs = "lbg";
            public const string TalentGigs = "tlg";
            public const string WritingGigs = "wrg";
        }
        
        public SearchGigsRequest(string site, string category) : base(site, category) {}
        public SearchGigsRequest(string site, string? area, string category) : base(site, area, category) {}
        internal SearchGigsRequest(string site, string? area, string category, IDictionary<string, object> parameterStore) : base(site, area, category, parameterStore) {}
    }
}