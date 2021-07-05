using System.Collections.Generic;

namespace Craigslist
{
    public class CraigslistSearchResults
    {
        internal CraigslistSearchResults(CraigslistSearchRequest request) => 
            Request = request;

        public string? Next { get; set; }

        public CraigslistSearchRequest Request { get; set; }

        public IEnumerable<CraigslistListing> Listings { get; set; } = new List<CraigslistListing>();
    }
}