using System.Collections.Generic;

namespace Craigslist
{
    public class CraigslistCrawlResults
    {
        public IEnumerable<CraigslistSearchResults> SearchResultPages { get; set; } = new List<CraigslistSearchResults>();

        public IEnumerable<CraigslistListingDetails> ListingDetails { get; set; } = new List<CraigslistListingDetails>();
    }
}