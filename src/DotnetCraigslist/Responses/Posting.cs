using System;
using System.Collections.Generic;
using System.Linq;

namespace DotnetCraigslist
{
    public class Posting
    {
        internal Posting(PostingRequest request, string id, Uri postingUrl, DateTime postedOn) => 
            (Request, Id, PostingUrl, PostedOn) = (request, id, postingUrl, postedOn);

        public PostingRequest Request { get; }

        public string Id { get; }

        public Uri PostingUrl { get; }

        public DateTime PostedOn { get; }

        public DateTime? UpdatedOn { get; init; }

        public string? FullTitle { get; init; }

        public string? Title { get; init; }

        public string? Price { get; init; }

        public string? Description { get; init; }

        public GeoCoordinate? Location { get; init; }

        public IEnumerable<string> AdditionalAttributes { get; init; } = Enumerable.Empty<string>();
    }
}