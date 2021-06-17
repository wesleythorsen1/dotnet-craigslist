using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Craigslist
{
    public interface ICraigslistClient
    {
        CraigslistSearchResults Search(CraigslistSearchRequest request);
        Task<CraigslistSearchResults> SearchAsync(CraigslistSearchRequest request, CancellationToken cancellationToken = default);
        CraigslistListingDetails GetListing(CraigslistListingRequest request);
        Task<CraigslistListingDetails> GetListingAsync(CraigslistListingRequest request, CancellationToken cancellationToken = default);
        IAsyncEnumerable<CraigslistListingDetails> GetListingDetailsAsync(CraigslistSearchRequest request, CancellationToken cancellationToken = default);
        IAsyncEnumerable<CraigslistListingDetails> GetListingDetailsAsync(IEnumerable<CraigslistListingRequest> listings, CancellationToken cancellationToken = default);
    }

    public class CraigslistClient : ICraigslistClient
    {
        /// Only set once, when user does not provide an HttpClient
        private static HttpClient? _staticHttpClient;
        private readonly HttpClient _httpClient;
        private readonly IPageParser _pageParser;

        public CraigslistClient()
            : this(default!)
        {
            if (_staticHttpClient == default)
            {
                _staticHttpClient = new HttpClient();
            }
            _httpClient = _staticHttpClient;
        }

        public CraigslistClient(HttpClient httpClient)
            : this(httpClient, new PageParser()) { }

        internal CraigslistClient(HttpClient httpClient, IPageParser pageParser)
        {
            _httpClient = httpClient;
            _pageParser = pageParser;
        }

        public CraigslistListingDetails GetListing(CraigslistListingRequest request)
        {
            using var req = new HttpRequestMessage(HttpMethod.Get, request.Uri);
            using var response = _httpClient.Send(req);
            response.EnsureSuccessStatusCode();

            using var content = response.Content.ReadAsStream();

            return _pageParser.ParseListing(request, content);
        }

        public async Task<CraigslistListingDetails> GetListingAsync(CraigslistListingRequest request, CancellationToken cancellationToken = default)
        {
            using var req = new HttpRequestMessage(HttpMethod.Get, request.Uri);
            using var response = await _httpClient.SendAsync(req, cancellationToken);
            response.EnsureSuccessStatusCode();

            using var content = await response.Content.ReadAsStreamAsync(cancellationToken);

            return _pageParser.ParseListing(request, content);
        }

        public async IAsyncEnumerable<CraigslistListingDetails> GetListingDetailsAsync(CraigslistSearchRequest request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var searchResults = await SearchAsync(request);
            foreach (var listing in searchResults.Listings)
            {
                var listingRequest = new CraigslistListingRequest(listing.ListingUrl);
                yield return await GetListingAsync(listingRequest);
            }
        }

        public async IAsyncEnumerable<CraigslistListingDetails> GetListingDetailsAsync(IEnumerable<CraigslistListingRequest> listingRequests, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            foreach (var request in listingRequests)
            {
                yield return await GetListingAsync(request);
            }
        }

        public CraigslistSearchResults Search(CraigslistSearchRequest request)
        {
            using var req = new HttpRequestMessage(HttpMethod.Get, request.Uri);
            using var response = _httpClient.Send(req);
            response.EnsureSuccessStatusCode();

            using var content = response.Content.ReadAsStream();

            return _pageParser.ParseSearchResults(request, content);
        }

        public async Task<CraigslistSearchResults> SearchAsync(CraigslistSearchRequest request, CancellationToken cancellationToken = default)
        {
            using var req = new HttpRequestMessage(HttpMethod.Get, request.Uri);
            using var response = await _httpClient.SendAsync(req, cancellationToken);
            response.EnsureSuccessStatusCode();

            using var content = await response.Content.ReadAsStreamAsync(cancellationToken);

            return _pageParser.ParseSearchResults(request, content);
        }
    }
}