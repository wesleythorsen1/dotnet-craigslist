using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Craigslist
{
    public interface ICraigslistStreamer
    {
        IAsyncEnumerable<CraigslistListing> StreamListings(CraigslistSearchRequest request, TimeSpan refreshInterval, CancellationToken cancellationToken = default);
        IAsyncEnumerable<CraigslistListingDetails> StreamListingDetails(CraigslistSearchRequest request, TimeSpan refreshInterval, CancellationToken cancellationToken = default);
    }

    public class CraigslistStreamer : ICraigslistStreamer
    {
        private readonly ICraigslistClient _client;

        public CraigslistStreamer() : this (new CraigslistClient()) { }

        internal CraigslistStreamer(ICraigslistClient client)
        {
            _client = client;
        }

        public async IAsyncEnumerable<CraigslistListingDetails> StreamListingDetails(
            CraigslistSearchRequest request, 
            TimeSpan refreshInterval, 
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;

                await foreach (var listing in GetListingsSince(request, now - refreshInterval, cancellationToken))
                {
                    var listingRequest = new CraigslistListingRequest(listing.ListingUrl);
                    yield return await _client.GetListingAsync(listingRequest, cancellationToken);
                }

                var wait = refreshInterval - (DateTime.UtcNow - now);
                if (wait > TimeSpan.Zero)
                {
                    await Task.Delay((int)wait.TotalMilliseconds, cancellationToken);
                }
            }
        }

        public async IAsyncEnumerable<CraigslistListing> StreamListings(
            CraigslistSearchRequest request, 
            TimeSpan refreshInterval, 
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                var lastListingDate = now - TimeSpan.FromMinutes(60);

                await foreach (var listing in GetListingsSince(request, lastListingDate, cancellationToken))
                {
                    if (lastListingDate < listing.Date)
                    {
                        lastListingDate = listing.Date;
                    }
                    yield return listing;
                }

                var wait = refreshInterval - (DateTime.Now - now);
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

                var listings = searchResults.Listings.Reverse().Where(l => l.Date >= since);

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

        private async IAsyncEnumerable<CraigslistListing> TakeListingsWhile(
            CraigslistSearchRequest request, 
            Func<CraigslistListing, bool> shouldContinue,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var searchResults = await _client.SearchAsync(request, cancellationToken);

                foreach (var listing in searchResults.Listings.Reverse())
                {
                    if (!shouldContinue(listing))
                    {
                        yield break;
                    }
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