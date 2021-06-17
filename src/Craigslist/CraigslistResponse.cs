using System;
using System.Collections.Generic;

namespace Craigslist
{
    public class CraigslistSearchResults
    {
        internal CraigslistSearchResults(CraigslistSearchRequest request) => 
            Request = request;

        public CraigslistSearchRequest Request { get; set; }

        public List<CraigslistListing> Listings { get; set; } = new List<CraigslistListing>();
    }

    public class CraigslistListing
    {
        internal CraigslistListing(string id, string listingUrl, DateTime date, string title)
            : this(id, listingUrl, date, title, default, default) { }

        internal CraigslistListing(string id, string listingUrl, DateTime date, string title, string? price, string? hood) => 
            (Id, ListingUrl, Date, Title, Price, Hood) = 
            (id, listingUrl, date, title, price, hood);

        public string Id { get; }

        public string ListingUrl { get; }

        public DateTime Date { get; }

        public string Title { get; }

        public string? Price { get; }

        public string? Hood { get; }
    }

    public class CraigslistListingDetails
    {
        internal CraigslistListingDetails(CraigslistListingRequest request, DateTime date, string title)
            : this(request, date, title, default, default) { }

        internal CraigslistListingDetails(CraigslistListingRequest request, DateTime date, string title, string? price, string? description) => 
            (Request, Date, Title, Price, Description) = 
            (request, date, title, price, description);

        public CraigslistListingRequest Request { get; set; }

        public string Id => Request.Id;

        public string ListingUrl => Request.Uri.ToString();

        public DateTime Date { get; }

        public string Title { get; }

        public string? Price { get; }

        public string? Description { get; }

        //TODO: Add other attributes
    }

    public class CraigslistCrawlResults
    {
        internal CraigslistCrawlResults(string url) => 
            Url = url;

        public string Url { get; set; }

        public List<CraigslistSearchResults> SearchResultPages { get; set; } = new List<CraigslistSearchResults>();

        public List<CraigslistListingDetails> ListingDetails { get; set; } = new List<CraigslistListingDetails>();
    }
}