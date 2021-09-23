using System.Collections.Generic;

namespace Craigslist
{
    public class SearchResults
    {
        internal SearchResults(SearchRequest request) => 
            Request = request;

        public string? Next { get; set; }

        public SearchRequest Request { get; set; }

        public IEnumerable<Listing> Listings { get; set; } = new List<Listing>();
    }
}