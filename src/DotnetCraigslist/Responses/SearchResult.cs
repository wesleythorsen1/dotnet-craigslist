using System;

namespace DotnetCraigslist
{
    public class SearchResult
    {
        internal SearchResult(string id, Uri postingUrl, DateTime date, string title)=> 
            (Id, PostingUrl, Date, Title) = 
            (id, postingUrl, date, title);

        public string Id { get; }

        public Uri PostingUrl { get; }

        public DateTime Date { get; }

        public string Title { get; }

        public string? Price { get; init; }

        public string? Hood { get; init; }
    }
}