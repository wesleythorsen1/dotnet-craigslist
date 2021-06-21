using System;
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

            return new CraigslistSearchResults(request)
            {
                Listings = rows.Select(r => ParseRow(r)).ToList(),
            };
        }

        public CraigslistListingDetails ParseListingDetails(CraigslistListingRequest request, Stream content)
        {
            var doc = new HtmlDocument();
            doc.Load(content);
            throw new NotImplementedException();
        }

        private CraigslistListing ParseRow(HtmlNode row)
        {
            var id = row.Attributes["data-pid"].Value;
            var link = row.SelectSingleNode(".//a[contains(@class, 'hdrlnk')]");
            var title = HttpUtility.UrlDecode(link.InnerText);
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
            var hood = row.SelectSingleNode(".//span[contains(@class, 'result-hood')]")?.InnerText?.Trim('(', ')');

            return new CraigslistListing(id, url, dateTime, title, price, hood);
        }
    }
}