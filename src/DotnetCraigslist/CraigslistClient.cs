using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetCraigslist
{
    public interface ICraigslistClient
    {
        SearchResults Search(SearchRequest request, CancellationToken cancellationToken = default);
        Task<SearchResults> SearchAsync(SearchRequest request, CancellationToken cancellationToken = default);
        Posting GetPosting(PostingRequest request, CancellationToken cancellationToken = default);
        Task<Posting> GetPostingAsync(PostingRequest request, CancellationToken cancellationToken = default);
    }

    public class CraigslistClient : ICraigslistClient
    {
        private readonly HttpClient _httpClient;
        private readonly IPageParser _pageParser;

        public CraigslistClient()
            : this(StaticHttpClient.DefaultClient) { }

        public CraigslistClient(HttpClient httpClient)
            : this(httpClient, new PageParser()) { }

        internal CraigslistClient(HttpClient httpClient, IPageParser pageParser)
            => (_httpClient, _pageParser) = (httpClient, pageParser);

        public SearchResults Search(SearchRequest request, CancellationToken cancellationToken = default)
        {
            using var req = new HttpRequestMessage(HttpMethod.Get, request.Url);
            using var response = _httpClient.Send(req, cancellationToken);
            response.EnsureSuccessStatusCode();

            using var content = response.Content.ReadAsStream(cancellationToken);

            return _pageParser.ParseSearchResults(request, content);
        }

        public async Task<SearchResults> SearchAsync(SearchRequest request, CancellationToken cancellationToken = default)
        {
            using var req = new HttpRequestMessage(HttpMethod.Get, request.Url);
            using var response = await _httpClient.SendAsync(req, cancellationToken);
            response.EnsureSuccessStatusCode();

            using var content = await response.Content.ReadAsStreamAsync(cancellationToken);

            return _pageParser.ParseSearchResults(request, content);
        }

        public Posting GetPosting(PostingRequest request, CancellationToken cancellationToken = default)
        {
            using var req = new HttpRequestMessage(HttpMethod.Get, request.Url);
            using var response = _httpClient.Send(req, cancellationToken);
            response.EnsureSuccessStatusCode();

            using var content = response.Content.ReadAsStream(cancellationToken);

            return _pageParser.ParsePosting(request, content);
        }

        public async Task<Posting> GetPostingAsync(PostingRequest request, CancellationToken cancellationToken = default)
        {
            using var req = new HttpRequestMessage(HttpMethod.Get, request.Url);
            using var response = await _httpClient.SendAsync(req, cancellationToken);
            response.EnsureSuccessStatusCode();

            using var content = await response.Content.ReadAsStreamAsync(cancellationToken);

            return _pageParser.ParsePosting(request, content);
        }
    }
}