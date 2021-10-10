using System;
using System.IO;
using System.Linq;
using System.Web;
using HtmlAgilityPack;

namespace DotnetCraigslist
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
                .ChildNodes
                .TakeWhile(n => !n.HasClass("nearby"))
                .Where(n => n.HasClass("result-row"));

            var next = doc.DocumentNode
                .SelectSingleNode("//head/link[contains(@rel, 'next')]")
                ?.Attributes["href"]
                ?.Value;
            next = HttpUtility.HtmlDecode(next);
                
            return new SearchResults(request)
            {
                NextPageUrl = next,
                Results = rows?.Select(r => ParseRow(r)).ToArray() ?? Enumerable.Empty<SearchResult>(),
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
            var postedDT = DateTime.Parse(posted);
                
            var updated = doc.DocumentNode
                .SelectSingleNode("//div[contains(@class, 'postinginfos')]")
                ?.SelectNodes(".//p[contains(@class, 'postinginfo')]")
                ?.SingleOrDefault(n => n.InnerText.Contains("updated:", StringComparison.InvariantCultureIgnoreCase))
                ?.SelectSingleNode(".//time")
                ?.Attributes["datetime"]
                ?.Value;
            var updatedDT = updated == default ? default(DateTime?) : DateTime.Parse(updated);

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
                .SelectNodes("//p[contains(@class, 'attrgroup')]/span")?
                .Select(n => string.Join("", n
                    .SelectNodes(".//text()")
                    .Cast<HtmlTextNode>()
                    .Select(t => t.Text)))
                .ToHashSet();

            return new Posting(request, request.Id, request.Url, postedDT)
            {
                UpdatedOn = updatedDT,
                FullTitle = fullTitle,
                Title = title,
                Price = price,
                Description = description,
                Location = location,
                AdditionalAttributes = attributes ?? Enumerable.Empty<string>(),
            };
        }

        private static SearchResult ParseRow(HtmlNode row)
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

            return new SearchResult(id, new Uri(url), dateTime, title)
            {
                Price = price,
                Hood = hood,
            };
        }
    }
}