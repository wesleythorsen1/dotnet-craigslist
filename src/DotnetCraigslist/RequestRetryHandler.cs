using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;


namespace DotnetCraigslist
{
    internal class RequestRetryHandler : DelegatingHandler
    {
        public int MaxRetry { get; }

        internal RequestRetryHandler(int maxRetry)
        {
            MaxRetry = maxRetry;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var remainingRetries = MaxRetry;

            while (true)
            {
                try
                {
                    return await base.SendAsync(request, cancellationToken);
                }
                catch(Exception) when (shouldRetry())
                {
                    // only catch when there are remaining retries
                }
            }

            bool shouldRetry() => remainingRetries-- > 0;
        }

        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var remainingRetries = MaxRetry;

            while (true)
            {
                try
                {
                    return base.Send(request, cancellationToken);
                }
                catch(Exception) when (shouldRetry())
                {
                    // only catch when there are remaining retries
                }
            }

            bool shouldRetry() => remainingRetries-- > 0;
        }
    }
}