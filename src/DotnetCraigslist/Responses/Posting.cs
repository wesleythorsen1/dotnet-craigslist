using System;
using System.Collections.Generic;
using System.Globalization;

namespace DotnetCraigslist
{
    public class Posting
    {
        internal Posting(PostingRequest request) => 
            Request = request;

        public PostingRequest Request { get; init; }

        public string Id => Request.Id;

        public Uri PostingUri => Request.Uri;

        public DateTime Posted { get; init; }

        public DateTime? Updated { get; init; }

        public string? FullTitle { get; init; }

        public string? Title { get; init; }

        public string? Price { get; init; }

        public string? Description { get; init; }

        public GeoCoordinate? Location { get; init; }

        public IEnumerable<string> AdditionalAttributes { get; init; } = new string[0];
    }
}