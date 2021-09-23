using System;
using System.Collections.Generic;
using System.Globalization;

namespace Craigslist
{
    public class ListingDetails
    {
        internal ListingDetails(ListingRequest request) => 
            Request = request;

        public ListingRequest Request { get; init; }

        public string Id => Request.Id;

        public Uri ListingUri => Request.Uri;

        public DateTime Posted { get; init; }

        public DateTime? Updated { get; init; }

        public string? FullTitle { get; init; }

        public string? Title { get; init; }

        public decimal? Price { get; init; }

        public string? PriceText => Price?.ToString("C", CultureInfo.CurrentCulture);

        public string? Description { get; init; }

        public GeoCoordinate? Location { get; init; }

        public IEnumerable<string> AdditionalAttributes { get; init; } = new string[0];
    }
}