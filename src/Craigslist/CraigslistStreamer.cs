using System;
using System.Collections.Generic;
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
            [EnumeratorCancellation] 
            CancellationToken cancellationToken = default)
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
            [EnumeratorCancellation] 
            CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;

                await foreach (var listing in GetListingsSince(request, now - refreshInterval, cancellationToken))
                {
                    yield return listing;
                }

                var wait = refreshInterval - (DateTime.UtcNow - now);
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
            bool endOfResults = false;
            while (!cancellationToken.IsCancellationRequested)
            {
                var searchResults = await _client.SearchAsync(request, cancellationToken);

                foreach (var listing in searchResults.Listings)
                {
                    if (listing.Date < since)
                    {
                        endOfResults = true;
                        break;
                    }
                    yield return listing;
                }

                if (endOfResults || searchResults.Listings.Count < 120)
                {
                    // no more results
                    break;
                }

                request = new CraigslistSearchRequest(request.Uri.ToString());
                request.Skip += 120;
            }
        }
    }
}