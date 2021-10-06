using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetCraigslist
{
    public interface ICraigslistStreamingClient
    {
        IAsyncEnumerable<Posting> StreamPostings(SearchRequest request, CancellationToken cancellationToken = default);
        IAsyncEnumerable<SearchResult> StreamSearchResults(SearchRequest request, CancellationToken cancellationToken = default);
    }

    public class CraigslistStreamingClient : ICraigslistStreamingClient
    {
        private static readonly TimeSpan FIVE_MINUTES = TimeSpan.FromMinutes(5);

        private readonly ICraigslistClient _client;

        public CraigslistStreamingClient()
            : this(StaticHttpClient.DefaultClient) { }

        public CraigslistStreamingClient(HttpClient httpClient)
            : this(new CraigslistClient(httpClient)) { }

        internal CraigslistStreamingClient(ICraigslistClient client)
        {
            _client = client;
        }

        public async IAsyncEnumerable<Posting> StreamPostings(
            SearchRequest request, 
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach(var searchResult in StreamSearchResults(request, cancellationToken))
            {
                var postingRequest = new PostingRequest(searchResult);
                yield return await _client.GetPostingAsync(postingRequest, cancellationToken);
            }
        }

        public async IAsyncEnumerable<SearchResult> StreamSearchResults(
            SearchRequest request, 
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var previousResults = new FifoHashSet<string>(5);
            var maxResults = 5;

            while (!cancellationToken.IsCancellationRequested)
            {
                var start = DateTime.UtcNow;

                var results = GetNewSearchResults(
                    request, 
                    id => previousResults.Contains(id), 
                    maxResults, 
                    cancellationToken);

                await foreach (var r in results)
                {
                    previousResults.Add(r.Id);
                    yield return r;
                }

                maxResults = 3000;

                var wait = FIVE_MINUTES - (DateTime.UtcNow - start);
                wait = wait > TimeSpan.Zero ? wait : TimeSpan.Zero;
                await Task.Delay(wait, cancellationToken);
            }
        }

        private async IAsyncEnumerable<SearchResult> GetNewSearchResults(
            SearchRequest request, 
            Predicate<string> isEncounteredResult, 
            int maxResults,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var searchResults = await _client.SearchAsync(request, cancellationToken);

            bool containsEncounteredResult = false;

            var results = searchResults.Results
                .Take(maxResults)
                .TakeWhile(r => !(containsEncounteredResult = isEncounteredResult(r.Id)))
                .Reverse()
                .ToArray();

            if (!containsEncounteredResult && 
                maxResults - results.Length > 0 && 
                searchResults.NextPageUrl != default)
            {
                var nextPageRequest = new SearchRequest(searchResults.NextPageUrl);
                var nextResults = GetNewSearchResults(nextPageRequest, isEncounteredResult, maxResults - results.Length, cancellationToken);
                await foreach (var r in nextResults) yield return r;
            }

            foreach (var r in results) yield return r;
        }
    }
}