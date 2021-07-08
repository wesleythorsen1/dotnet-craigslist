using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;


namespace Craigslist
{
    public class RequestLimitingHandler : DelegatingHandler
    {
        public int DegreesOfParellelism { get; }

        private Random _random = new Random((int)DateTime.Now.Ticks);
        private SemaphoreSlim _semaphore;

        public RequestLimitingHandler(int degreesOfParellelism)
        {
            DegreesOfParellelism = degreesOfParellelism;
            _semaphore = new SemaphoreSlim(DegreesOfParellelism, DegreesOfParellelism);
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