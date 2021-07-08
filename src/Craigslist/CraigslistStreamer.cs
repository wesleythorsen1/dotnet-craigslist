using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Craigslist
{
    public interface ICraigslistStreamer
    {
        IAsyncEnumerable<CraigslistListing> StreamListings(CraigslistSearchRequest request, CancellationToken cancellationToken = default);
        IAsyncEnumerable<CraigslistListingDetails> StreamListingDetails(CraigslistSearchRequest request, CancellationToken cancellationToken = default);
    }

    public class CraigslistStreamer : ICraigslistStreamer
    {
        private static readonly TimeSpan FIFTEEN_MINUTES = TimeSpan.FromMinutes(15);

        private readonly ICraigslistClient _client;

        public CraigslistStreamer() : this(new CraigslistClient()) { }

        public CraigslistStreamer(HttpClient httpClient) : this(new CraigslistClient(httpClient)) { }

        internal CraigslistStreamer(ICraigslistClient client)
        {
            _client = client;
        }

        public async IAsyncEnumerable<CraigslistListingDetails> StreamListingDetails(
            CraigslistSearchRequest request, 
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach(var listing in StreamListings(request, cancellationToken))
            {
                var listingRequest = new CraigslistListingRequest(listing.ListingUrl);
                yield return await _client.GetListingAsync(listingRequest, cancellationToken);
            }
        }

        public async IAsyncEnumerable<CraigslistListing> StreamListings(
            CraigslistSearchRequest request, 
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var start = DateTime.UtcNow;
            var lastListingDate = start - FIFTEEN_MINUTES;

            while (!cancellationToken.IsCancellationRequested)
            {
                await foreach (var listing in GetListingsSince(request, lastListingDate, cancellationToken))
                {
                    if (lastListingDate < listing.Date.ToUniversalTime())
                    {
                        lastListingDate = listing.Date.ToUniversalTime();
                    }
                    yield return listing;
                }

                var wait = FIFTEEN_MINUTES - (DateTime.UtcNow - start);
                if (wait > TimeSpan.Zero)
                {
                    await Task.Delay((int)wait.TotalMilliseconds, cancellationToken);
                }
            }
        }

        private async IAsyncEnumerable<CraigslistListing> GetListingsSince(
            CraigslistSearchRequest request, 
            DateTime since, 
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var searchResults = await _client.SearchAsync(request, cancellationToken);

                var listings = searchResults.Listings
                    .Where(l => l.Date.ToUniversalTime() > since)
                    .OrderBy(l => l.Date);

                foreach (var listing in listings)
                {
                    yield return listing;
                }

                if (listings.Count() < searchResults.Listings.Count() || searchResults.Next == default)
                {
                    // no more results
                    yield break;
                }

                request = new CraigslistSearchRequest(searchResults.Next);
            }
        }
    }
}