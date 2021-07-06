using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Craigslist
{
    public interface ICraigslistCrawler
    {
        CraigslistCrawlResults Crawl(CraigslistSearchRequest request, int maxResults);
        Task<CraigslistCrawlResults> CrawlAsync(CraigslistSearchRequest request, int maxResults = 120, CancellationToken cancellationToken = default);
    }

    public class CraigslistCrawler : ICraigslistCrawler
    {
        private readonly ICraigslistClient _client;

        public CraigslistCrawler() : this(new CraigslistClient()) { }

        public CraigslistCrawler(HttpClient httpClient) : this(new CraigslistClient(httpClient)) { }

        internal CraigslistCrawler(ICraigslistClient client)
        {
            _client = client;
        }

        public CraigslistCrawlResults Crawl(CraigslistSearchRequest request, int maxResults = 120)
        {
            var searchResults = new List<CraigslistSearchResults>();
            var listingDetails = new List<CraigslistListingDetails>();

            var remainingResults = maxResults;

            while (remainingResults > 0)
            {
                var searchResult = _client.Search(request);

                searchResult.Listings = searchResult.Listings.Take(remainingResults);

                remainingResults -= searchResult.Listings.Count();

                searchResults.Add(searchResult);

                foreach (var listing in searchResult.Listings)
                {
                    var listingRequest = new CraigslistListingRequest(listing.ListingUrl);
                    var detail = _client.GetListing(listingRequest);
                    listingDetails.Add(detail);
                }

                if (searchResult.Next == default) break; // end of search results

                request = new CraigslistSearchRequest(searchResult.Next);
            }

            return new CraigslistCrawlResults
            {
                SearchResultPages = searchResults,
                ListingDetails = listingDetails,
            };
        }

        public async Task<CraigslistCrawlResults> CrawlAsync(CraigslistSearchRequest request, int maxResults = 120, CancellationToken cancellationToken = default)
        {
            var searchResults = new List<CraigslistSearchResults>();
            var listingDetails = new List<CraigslistListingDetails>();

            var remainingResults = maxResults;

            while (remainingResults > 0)
            {
                var searchResult = await _client.SearchAsync(request, cancellationToken);

                searchResult.Listings = searchResult.Listings.Take(remainingResults);

                remainingResults -= searchResult.Listings.Count();

                searchResults.Add(searchResult);

                var detailTasks = searchResult.Listings
                    .Select(l => new CraigslistListingRequest(l.ListingUrl))
                    .Select(r => _client.GetListingAsync(r, cancellationToken));

                var details = await Task.WhenAll(detailTasks);

                listingDetails.AddRange(details);

                if (searchResult.Next == default) break; // end of search results

                request = new CraigslistSearchRequest(searchResult.Next);
            }

            return new CraigslistCrawlResults
            {
                SearchResultPages = searchResults,
                ListingDetails = listingDetails,
            };
        }
    }
}