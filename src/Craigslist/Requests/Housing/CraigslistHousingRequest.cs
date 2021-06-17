using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable

namespace Craigslist
{
    internal class CraigslistHousingRequest : CraigslistRequest
    {
        //private const string CATEGORY_ALL_HOUSING = "hhh";

        public CraigslistHousingRequest(string site, string category) : base(site, category)
        {
        }

        public CraigslistHousingRequest(string site, string? area, string category) : base(site, area, category)
        {
        }

        internal CraigslistHousingRequest(string site, string? area, string category, IDictionary<string, string> parameterStore) : base(site, area, category, parameterStore)
        {
        }
    }
}