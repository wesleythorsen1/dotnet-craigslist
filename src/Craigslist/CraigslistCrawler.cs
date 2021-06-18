using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Craigslist
{
    public interface ICraigslistCrawler
    {
        CraigslistCrawlResults Crawl(CraigslistSearchRequest request, int maxResults = 20);
        Task<CraigslistCrawlResults> CrawlAsync(CraigslistSearchRequest request, int maxResults = 20, CancellationToken cancellationToken = default);
    }

    public class CraigslistCrawler : ICraigslistCrawler
    {
        private readonly ICraigslistClient _client;

        public CraigslistCrawler() : this (new CraigslistClient()) { }

        internal CraigslistCrawler(ICraigslistClient client)
        {
            _client = client;
        }

        public CraigslistCrawlResults Crawl(CraigslistSearchRequest request, int maxResults = 120)
        {
            throw new System.NotImplementedException();
        }

        public Task<CraigslistCrawlResults> CrawlAsync(CraigslistSearchRequest request, int maxResults = 120, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}