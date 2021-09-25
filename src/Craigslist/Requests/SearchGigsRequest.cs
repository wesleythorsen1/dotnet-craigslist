using System;
using System.Collections.Generic;

namespace Craigslist
{
    public class SearchGigsRequest : SearchRequest
    {
        public static class Categories
        {
            private const string All = "ggg";
            private const string ComputerGigs = "cpg";
            private const string CreativeGigs = "crg";
            private const string CrewGigs = "cwg";
            private const string DomesticGigs = "dmg";
            private const string EventGigs = "evg";
            private const string LaborGigs = "lbg";
            private const string TalentGigs = "tlg";
            private const string WritingGigs = "wrg";
        }
        
        public SearchGigsRequest(string site, string category) : base(site, category) {}
        public SearchGigsRequest(string site, string? area, string category) : base(site, area, category) {}
        internal SearchGigsRequest(string site, string? area, string category, IDictionary<string, object> parameterStore) : base(site, area, category, parameterStore) {}
    }
}