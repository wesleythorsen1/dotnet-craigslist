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
            var throttle = new ThrottlingHandler(1)
            {
                InnerHandler = new HttpClientHandler(),
            };
            using var httpClient = new HttpClient(throttle);

            httpClient.Timeout = TimeSpan.FromHours(1); 
            
            // var client = new CraigslistClient(httpClient);

            // var request = new CraigslistHousingRentalRequest("seattle")
            // {
            //     SearchText = "loft",
            //     PostedToday = true
            // };

            //var results = await client.SearchAsync(request);

            var crawler = new CraigslistCrawler(httpClient);
            var request = new CraigslistSearchRequest("https://seattle.craigslist.org/search/apa?query=loft&sort=rel&postedToday=1&min_bedrooms=3&availabilityMode=0&sale_date=all+dates");
            var crawl = await crawler.CrawlAsync(request, 4);
            //var crawl = crawler.Crawl(request, 120);


        }
    }

    public class ThrottlingHandler : DelegatingHandler
    {
        public int MaxDegreesOfParellelism { get; }

        private Random _random = new Random((int)DateTime.Now.Ticks);
        private SemaphoreSlim _semaphore;

        public ThrottlingHandler(int maxDegreesOfParellelism)
        {
            MaxDegreesOfParellelism = maxDegreesOfParellelism;
            _semaphore = new SemaphoreSlim(MaxDegreesOfParellelism, MaxDegreesOfParellelism);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                await _semaphore.WaitAsync();

                await Task.Delay(TimeSpan.FromSeconds(_random.Next(10, 20)));

                return await base.SendAsync(request, cancellationToken);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                _semaphore.Wait();

                Thread.Sleep(TimeSpan.FromSeconds(_random.Next(10, 20)));

                return base.Send(request, cancellationToken);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
