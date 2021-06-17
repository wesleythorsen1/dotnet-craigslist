using System;
using System.Collections.Generic;

namespace Craigslist
{
    public class CraigslistSearchResults
    {
        public CraigslistSearchResults(string url) => 
            Url = url;

        public string Url { get; }

        public List<CraigslistListing> Listings { get; set; } = new List<CraigslistListing>();
    }

    public class CraigslistListing
    {
        public CraigslistListing(string id, string listingUrl, DateTime date, string title)
            : this(id, listingUrl, date, title, default, default) { }

        public CraigslistListing(string id, string listingUrl, DateTime date, string title, string? price, string? hood) => 
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
        public CraigslistListingDetails(string id, string listingUrl, DateTime date, string title)
            : this(id, listingUrl, date, title, default, default) { }

        public CraigslistListingDetails(string id, string listingUrl, DateTime date, string title, string? price, string? description) => 
            (Id, ListingUrl, Date, Title, Price, Description) = 
            (id, listingUrl, date, title, price, description);

        public string Id { get; }

        public string ListingUrl { get; }

        public DateTime Date { get; }

        public string Title { get; }

        public string? Price { get; }

        public string? Description { get; }

        //TODO: Add other attributes
    }

    public class CraigslistCrawlResults
    {
        public CraigslistCrawlResults(string url) => 
            Url = url;

        public string Url { get; set; }

        public List<CraigslistSearchResults> SearchResultPages { get; set; } = new List<CraigslistSearchResults>();

        public List<CraigslistListingDetails> ListingDetails { get; set; } = new List<CraigslistListingDetails>();
    }
}