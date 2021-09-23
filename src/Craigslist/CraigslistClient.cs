using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Craigslist
{
    public interface ICraigslistClient
    {
        SearchResults Search(SearchRequest request);
        Task<SearchResults> SearchAsync(SearchRequest request, CancellationToken cancellationToken = default);
        ListingDetails GetListing(ListingRequest request);
        Task<ListingDetails> GetListingAsync(ListingRequest request, CancellationToken cancellationToken = default);
    }

    public class CraigslistClient : ICraigslistClient
    {
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
            => (_httpClient, _pageParser) = (httpClient, pageParser);

        public SearchResults Search(SearchRequest request)
        {
            using var req = new HttpRequestMessage(HttpMethod.Get, request.Uri);
            using var response = _httpClient.Send(req);
            response.EnsureSuccessStatusCode();

            using var content = response.Content.ReadAsStream();

            return _pageParser.ParseSearchResults(request, content);
        }

        public async Task<SearchResults> SearchAsync(SearchRequest request, CancellationToken cancellationToken = default)
        {
            using var req = new HttpRequestMessage(HttpMethod.Get, request.Uri);
            using var response = await _httpClient.SendAsync(req, cancellationToken);
            response.EnsureSuccessStatusCode();

            using var content = await response.Content.ReadAsStreamAsync(cancellationToken);

            return _pageParser.ParseSearchResults(request, content);
        }

        public ListingDetails GetListing(ListingRequest request)
        {
            using var req = new HttpRequestMessage(HttpMethod.Get, request.Uri);
            using var response = _httpClient.Send(req);
            response.EnsureSuccessStatusCode();

            using var content = response.Content.ReadAsStream();

            return _pageParser.ParseListingDetails(request, content);
        }

        public async Task<ListingDetails> GetListingAsync(ListingRequest request, CancellationToken cancellationToken = default)
        {
            using var req = new HttpRequestMessage(HttpMethod.Get, request.Uri);
            using var response = await _httpClient.SendAsync(req, cancellationToken);
            response.EnsureSuccessStatusCode();

            using var content = await response.Content.ReadAsStreamAsync(cancellationToken);

            return _pageParser.ParseListingDetails(request, content);
        }
    }
}