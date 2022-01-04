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
        IAsyncEnumerable<SearchResult> TakeSearchResults(
            SearchRequest request, 
            int count, 
            CancellationToken cancellationToken = default);
        IAsyncEnumerable<SearchResult> TakeSearchResultsWhile(
            SearchRequest request, 
            Func<SearchResult, bool> predicate, 
            int maxResults, 
            CancellationToken cancellationToken = default);
        IAsyncEnumerable<SearchResult> TakeSearchResultsWhile(
            SearchRequest request, 
            Func<SearchResult, bool> predicate, 
            CancellationToken cancellationToken = default);
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
            var previousResults = new CircularSet<string>(5);
            var maxResults = 5;

            while (!cancellationToken.IsCancellationRequested)
            {
                var start = DateTime.UtcNow;

                var results = TakeSearchResultsWhile(
                    request, 
                    r => !previousResults.Contains(r.Id), 
                    maxResults, 
                    cancellationToken);

                await foreach (var r in results.Reverse())
                {
                    previousResults.Add(r.Id);
                    yield return r;
                }

                maxResults = 3000;

                var wait = FIVE_MINUTES - (DateTime.UtcNow - start);
                wait = wait > TimeSpan.Zero ? wait : TimeSpan.Zero;
                await Task.Delay(wait, cancellationToken);
            }

            cancellationToken.ThrowIfCancellationRequested();
        }

        public IAsyncEnumerable<SearchResult> TakeSearchResults(
            SearchRequest request, 
            int count, 
            CancellationToken cancellationToken = default)
        {
            int results = 0;
            return TakeSearchResultsWhile(
                request, 
                r => results++ < count,
                cancellationToken);
        }

        public IAsyncEnumerable<SearchResult> TakeSearchResultsWhile(
            SearchRequest request, 
            Func<SearchResult, bool> predicate, 
            int maxResults, 
            CancellationToken cancellationToken = default)
        {
            int results = 0;
            return TakeSearchResultsWhile(
                request, 
                r => predicate(r) && (results++ < maxResults),
                cancellationToken);
        }

        public async IAsyncEnumerable<SearchResult> TakeSearchResultsWhile(
            SearchRequest request, 
            Func<SearchResult, bool> predicate, 
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var searchResults = await _client.SearchAsync(request, cancellationToken);

            bool shouldContinue = true;

            var results = searchResults.Results
                .TakeWhile(r => (shouldContinue = predicate(r)));

            foreach (var r in results) yield return r;

            if (shouldContinue && 
                searchResults.NextPageUrl != default)
            {
                var nextPageRequest = new SearchRequest(searchResults.NextPageUrl);
                var nextResults = TakeSearchResultsWhile(nextPageRequest, predicate, cancellationToken);
                await foreach (var r in nextResults) yield return r;
            }
        }
    }
}