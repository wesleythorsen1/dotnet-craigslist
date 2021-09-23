using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using HtmlAgilityPack;

namespace Craigslist
{
    internal interface IPageParser
    {
        SearchResults ParseSearchResults(SearchRequest request, Stream content);
        Posting ParsePosting(PostingRequest request, Stream content);
    }

    internal class PageParser : IPageParser
    {
        public SearchResults ParseSearchResults(SearchRequest request, Stream content)
        {
            var doc = new HtmlDocument();
            doc.Load(content);
            
            var rows = doc.DocumentNode
                .SelectSingleNode("//ul[@id='search-results']")
                .SelectNodes(".//li[contains(@class, 'result-row')]");

            var next = doc.DocumentNode
                .SelectSingleNode("//head/link[contains(@rel, 'next')]")
                ?.Attributes["href"]
                ?.Value;
                
            return new SearchResults(request)
            {
                Next = next,
                Results = rows?.Select(r => ParseRow(r)).ToList() ?? Enumerable.Empty<SearchResult>(),
            };
        }

        public Posting ParsePosting(PostingRequest request, Stream content)
        {
            var doc = new HtmlDocument();
            doc.Load(content);
            
            var posted = doc.DocumentNode
                .SelectSingleNode("//p[@id='display-date']/time")
                .Attributes["datetime"]
                .Value;
                
            var updated = doc.DocumentNode
                .SelectSingleNode("//div[contains(@class, 'postinginfos')]")
                ?.SelectNodes(".//p[contains(@class, 'postinginfo')]")
                ?.SingleOrDefault(n => n.InnerText.Contains("updated:", StringComparison.InvariantCultureIgnoreCase))
                ?.SelectSingleNode(".//time")
                ?.Attributes["datetime"]
                ?.Value;

            var fullTitle = string.Join("", doc.DocumentNode
                .SelectNodes("//span[contains(@class, 'postingtitletext')]//text()")
                .Cast<HtmlTextNode>()
                .Select(n => n.Text))
                .Trim();
            fullTitle = HttpUtility.HtmlDecode(fullTitle);

            var title = doc.DocumentNode
                .SelectSingleNode("//span[@id='titletextonly']")
                .InnerText
                .Trim();
            title = HttpUtility.HtmlDecode(title);

            var price = doc.DocumentNode
                .SelectSingleNode("//span[contains(@class, 'price')]")
                ?.InnerText;

            var descriptionParts = doc.DocumentNode
                .SelectNodes("//section[@id='postingbody']//text()")
                .Cast<HtmlTextNode>()
                .Select(n => HttpUtility.HtmlDecode(n.Text))
                .Where(t => !string.IsNullOrWhiteSpace(t));
            var description = string.Join(" ", descriptionParts);

            var map = doc.DocumentNode
                .SelectSingleNode("//div[@id='map']");
            var latitude =  map.Attributes["data-latitude"]?.Value;
            var longitude = map.Attributes["data-longitude"]?.Value;
            var accuracy = map.Attributes["data-accuracy"]?.Value;

            GeoCoordinate? location = default;
            if (latitude != default && longitude != default)
            {
                location = new GeoCoordinate
                {
                    Latitude = double.Parse(latitude),
                    Longitude = double.Parse(longitude),
                    Accuracy = accuracy == default ? default(int?) : int.Parse(accuracy)
                };
            }

            var attributes = doc.DocumentNode
                .SelectNodes("//p[contains(@class, 'attrgroup')]/span")
                .Select(n => string.Join("", n
                    .SelectNodes(".//text()")
                    .Cast<HtmlTextNode>()
                    .Select(t => t.Text)))
                .ToHashSet();

            return new Posting(request)
            {
                Posted = DateTime.Parse(posted),
                Updated = updated == default ? default(DateTime?) : DateTime.Parse(updated),
                FullTitle = fullTitle,
                Title = title,
                Price = string.IsNullOrWhiteSpace(price) ? default(decimal?) : decimal.Parse(price, NumberStyles.Any),
                Description = description,
                Location = location,
                AdditionalAttributes = attributes,
            };
        }

        private SearchResult ParseRow(HtmlNode row)
        {
            var id = row.Attributes["data-pid"].Value;
            var link = row.SelectSingleNode(".//a[contains(@class, 'hdrlnk')]");
            var title = HttpUtility.HtmlDecode(link.InnerText);
            var url = link.Attributes["href"].Value;
            var time = row.SelectSingleNode(".//time");

            var dateTime = default(DateTime);

            if (time != default)
            {
                dateTime = DateTime.Parse(time.Attributes["datetime"].Value);
            }

            var price = row.SelectSingleNode(".//span[contains(@class, 'result-price')]")?.InnerText;
            var hood = row.SelectSingleNode(".//span[contains(@class, 'result-hood')]")?.InnerText?.Trim(' ', '(', ')');

            return new SearchResult(id, url, dateTime, title, price, hood);
        }
    }
}