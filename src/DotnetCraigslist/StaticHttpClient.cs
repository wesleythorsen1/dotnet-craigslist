using System;
using System.Net.Http;

namespace DotnetCraigslist
{
    internal static class StaticHttpClient
    {
        private static Lazy<HttpClient> _client = 
            new Lazy<HttpClient>(() => 
            {
                var handler = new RequestRetryHandler(3)
                {
                    InnerHandler = new RequestRateLimitingHandler(TimeSpan.FromSeconds(5))
                    {
                        InnerHandler = new HttpClientHandler(),
                    }
                };
                var client = new HttpClient(handler);
                client.DefaultRequestHeaders.ConnectionClose = true;
                return client;
            });
        
        internal static HttpClient DefaultClient => _client.Value;
    }
}