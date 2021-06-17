using System;
using System.Collections.Generic;
using System.Linq;

namespace Craigslist
{
    public abstract class CraigslistHousingRequest : CraigslistSearchRequest
    {
        public CraigslistHousingRequest(string site, string category) : base(site, category) {}
        public CraigslistHousingRequest(string site, string? area, string category) : base(site, area, category) {}
        internal CraigslistHousingRequest(string site, string? area, string category, IDictionary<string, object> parameterStore) : base(site, area, category, parameterStore) {}
    }
}