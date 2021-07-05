using System.Collections.Generic;

namespace Craigslist
{
    public class CraigslistCrawlResults
    {
        internal CraigslistCrawlResults(string url) => 
            Url = url;

        public string Url { get; set; }

        public IEnumerable<CraigslistSearchResults> SearchResultPages { get; set; } = new List<CraigslistSearchResults>();

        public IEnumerable<CraigslistListingDetails> ListingDetails { get; set; } = new List<CraigslistListingDetails>();
    }
}