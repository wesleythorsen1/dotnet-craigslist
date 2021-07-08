using System.Collections.Generic;

namespace Craigslist
{
    public class CraigslistCrawlResults
    {
        public IEnumerable<CraigslistSearchResults> SearchResultPages { get; init; } = new List<CraigslistSearchResults>();

        public IEnumerable<CraigslistListingDetails> ListingDetails { get; init; } = new List<CraigslistListingDetails>();
    }
}