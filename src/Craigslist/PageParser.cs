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
        CraigslistSearchResults ParseSearchResults(CraigslistSearchRequest request, Stream content);
        CraigslistListingDetails ParseListingDetails(CraigslistListingRequest request, Stream content);
    }

    internal class PageParser : IPageParser
    {
        public CraigslistSearchResults ParseSearchResults(CraigslistSearchRequest request, Stream content)
        {
            var doc = new HtmlDocument();
            doc.Load(content);
            
            var rows = doc.DocumentNode
                .SelectSingleNode("//ul[contains(@class, 'rows')]")
                .SelectNodes(".//li[contains(@class, 'result-row')]");

            var next = doc.DocumentNode
                .SelectSingleNode("//head/link[contains(@rel, 'next')]")
                ?.Attributes["href"]
                ?.Value;
                
            return new CraigslistSearchResults(request)
            {
                Next = next,
                Listings = rows.Select(r => ParseRow(r)).ToList(),
            };
        }

        public CraigslistListingDetails ParseListingDetails(CraigslistListingRequest request, Stream content)
        {
            var doc = new HtmlDocument();
            doc.Load(content);
            
            var posted = doc.DocumentNode
                .SelectSingleNode("//p[@id='display-date']/time")
                .Attributes["datetime"]
                .Value;
                
            var updated = doc.DocumentNode
                .SelectSingleNode("//div[contains(@class, 'postinginfos')]")
                ?.SelectNodes(".//p[contains(@class, 'postinginfo reveal')]")
                ?.SingleOrDefault(n => n.InnerText.Contains("updated:", StringComparison.InvariantCultureIgnoreCase))
                ?.SelectSingleNode(".//time")
                ?.Attributes["datetime"]
                ?.Value;

            var fullTitle = string.Join("", doc.DocumentNode
                .SelectNodes("//span[@class='postingtitletext']//text()")
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
                .SelectSingleNode("//span[@class='price']")
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
                .SelectNodes("//p[@class='attrgroup']/span")
                .Select(n => string.Join("", n
                    .SelectNodes(".//text()")
                    .Cast<HtmlTextNode>()
                    .Select(t => t.Text)))
                .ToArray();

            return new CraigslistListingDetails(request)
            {
                Posted = DateTime.Parse(posted),
                Updated = updated == default ? default(DateTime?) : DateTime.Parse(updated),
                FullTitle = fullTitle,
                Title = title,
                Price = price == default ? default(decimal?) : decimal.Parse(price, NumberStyles.Any),
                Description = description,
                Location = location,
                AdditionalAttributes = attributes,
            };
        }

        private CraigslistListing ParseRow(HtmlNode row)
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
            else
            {
                var pl = row.SelectSingleNode(".//span[contains(@class, 'pl')]");
                if (pl != default)
                {
                    dateTime = DateTime.Parse(pl.InnerText.Split(':')[0].Trim());
                }
            }

            var price = row.SelectSingleNode(".//span[contains(@class, 'result-price')]")?.InnerText;
            var hood = row.SelectSingleNode(".//span[contains(@class, 'result-hood')]")?.InnerText?.Trim(' ', '(', ')');

            return new CraigslistListing(id, url, dateTime, title, price, hood);
        }
    }
}