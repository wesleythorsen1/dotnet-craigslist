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
        private static readonly TimeSpan FIFTEEN_MINUTES = TimeSpan.FromMinutes(15);

        private readonly ICraigslistClient _client;

        public CraigslistStreamingClient() : this(new CraigslistClient()) { }

        public CraigslistStreamingClient(HttpClient httpClient) : this(new CraigslistClient(httpClient)) { }

        internal CraigslistStreamingClient(ICraigslistClient client)
        {
            _client = client;
        }

        public async IAsyncEnumerable<Posting> StreamPostings(
            SearchRequest request, 
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach(var listing in StreamSearchResults(request, cancellationToken))
            {
                var postingRequest = new PostingRequest(listing);
                yield return await _client.GetPostingAsync(postingRequest, cancellationToken);
            }
        }

        public async IAsyncEnumerable<SearchResult> StreamSearchResults(
            SearchRequest request, 
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var previousResults = new FifoHashSet<string>(10);
            var maxResults = 10;

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

                var wait = FIFTEEN_MINUTES - (DateTime.UtcNow - start);
                if (wait > TimeSpan.Zero)
                {
                    await Task.Delay((int)wait.TotalMilliseconds, cancellationToken);
                }
            }
        }

        private async IAsyncEnumerable<SearchResult> GetNewSearchResults(
            SearchRequest request, 
            Predicate<string> isSeenResult, 
            int maxResults,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var searchResults = await _client.SearchAsync(request, cancellationToken);

            bool containsPreviousResult = false;

            var results = searchResults.Results
                .Take(maxResults)
                .TakeWhile(r => !(containsPreviousResult = isSeenResult(r.Id)))
                .Reverse()
                .ToArray();

            if (!containsPreviousResult && 
                maxResults - results.Length > 0 && 
                searchResults.Next != default)
            {
                var nextPageRequest = new SearchRequest(searchResults.Next);
                var nextResults = GetNewSearchResults(nextPageRequest, isSeenResult, maxResults - results.Length, cancellationToken);
                await foreach (var r in nextResults) yield return r;
            }

            foreach (var r in results) yield return r;
        }
    }
}