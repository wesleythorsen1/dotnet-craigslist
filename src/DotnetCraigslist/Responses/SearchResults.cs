using System.Collections.Generic;
using System.Linq;

namespace DotnetCraigslist
{
    public class SearchResults
    {
        internal SearchResults(SearchRequest request) => 
            Request = request;

        public SearchRequest Request { get; }

        public string? NextPageUrl { get; init; }

        public IEnumerable<SearchResult> Results { get; init; } = Enumerable.Empty<SearchResult>();
    }
}