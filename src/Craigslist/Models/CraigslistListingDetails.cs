using System;

namespace Craigslist
{
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
}