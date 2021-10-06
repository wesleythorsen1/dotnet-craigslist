using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;


namespace DotnetCraigslist
{
    internal class RequestLimitingHandler : DelegatingHandler
    {
        public TimeSpan RequestDelay { get; }

        private DateTime _lastRequest;
        private SemaphoreSlim _semaphore;

        internal RequestLimitingHandler(TimeSpan requestDelay)
        {
            RequestDelay = requestDelay;
            _lastRequest = DateTime.MinValue;
            _semaphore = new SemaphoreSlim(1, 1);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                await _semaphore.WaitAsync();

                var wait = RequestDelay - (DateTime.UtcNow - _lastRequest);
                wait = wait > TimeSpan.Zero ? wait : TimeSpan.Zero;

                await Task.Delay(wait);

                return await base.SendAsync(request, cancellationToken);
            }
            finally
            {
                _lastRequest = DateTime.UtcNow;
                _semaphore.Release();
            }
        }

        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                _semaphore.Wait();

                var wait = RequestDelay - (DateTime.UtcNow - _lastRequest);
                wait = wait > TimeSpan.Zero ? wait : TimeSpan.Zero;

                Thread.Sleep(wait);

                return base.Send(request, cancellationToken);
            }
            finally
            {
                _lastRequest = DateTime.UtcNow;
                _semaphore.Release();
            }
        }
    }
}