using System;
using System.Linq;
using System.Threading.Tasks;

namespace Craigslist.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var request = new CraigslistSearchRequest("seattle", "see", "apa")
            {
                SearchText = "loft",
                PostedToday = true
            };

            var client = new CraigslistClient();
            var results = await client.SearchAsync(request);

            var listingRequests = results.Listings.Select(l => new CraigslistListingRequest(l.ListingUrl));

        }
    }
}
