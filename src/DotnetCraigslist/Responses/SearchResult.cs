using System;

namespace DotnetCraigslist
{
    public class SearchResult
    {
        internal SearchResult(string id, string listingUrl, DateTime date, string title)
            : this(id, listingUrl, date, title, default, default) { }

        internal SearchResult(string id, string listingUrl, DateTime date, string title, string? price, string? hood) => 
            (Id, PostingUrl, Date, Title, Price, Hood) = 
            (id, listingUrl, date, title, price, hood);

        public string Id { get; }

        public string PostingUrl { get; }

        public DateTime Date { get; }

        public string Title { get; }

        public string? Price { get; }

        public string? Hood { get; }
    }
}