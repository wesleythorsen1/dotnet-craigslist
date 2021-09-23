using System;

namespace Craigslist
{
    public class Listing
    {
        internal Listing(string id, string listingUrl, DateTime date, string title)
            : this(id, listingUrl, date, title, default, default) { }

        internal Listing(string id, string listingUrl, DateTime date, string title, string? price, string? hood) => 
            (Id, ListingUrl, Date, Title, Price, Hood) = 
            (id, listingUrl, date, title, price, hood);

        public string Id { get; }

        public string ListingUrl { get; }

        public DateTime Date { get; }

        public string Title { get; }

        public string? Price { get; }

        public string? Hood { get; }
    }
}