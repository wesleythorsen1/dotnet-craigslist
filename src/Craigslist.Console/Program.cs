using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Craigslist.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var httpClient = new HttpClient();
            var client = new CraigslistClient(httpClient);

            var request = new CraigslistHousingRentalRequest("seattle")
            {
                SearchText = "loft",
                PostedToday = true
            };

            var results = await client.SearchAsync(request);

        }
    }
}
