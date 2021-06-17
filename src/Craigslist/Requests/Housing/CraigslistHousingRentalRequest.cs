using System.Collections.Generic;

namespace Craigslist
{
    public class CraigslistHousingRentalRequest : CraigslistHousingRequest
    {
        private const string AREA_APA = "apa";

        public CraigslistHousingRentalRequest(string site) : base(site, AREA_APA) {}
        public CraigslistHousingRentalRequest(string site, string? area) : base(site, area, AREA_APA) {}
        internal CraigslistHousingRentalRequest(string site, string? area, IDictionary<string, object> parameterStore) : base(site, area, AREA_APA, parameterStore) {}
    }
}