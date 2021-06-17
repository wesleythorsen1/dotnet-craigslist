using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Craigslist
{
    public static class Util
    {
        private const string ALL_SITES_URL = "http://www.craigslist.org/about/sites";
        private const string SITE_URL_TEMPLATE = "http://{0}.craigslist.org";

        public readonly static HttpClient HttpClient = new HttpClient();

        public static HtmlDocument CreateDoc(Stream content)
        {
            var doc = new HtmlDocument();
            doc.Load(content);
            return doc;
        }

        public static IList<string> GetAllSites()
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, ALL_SITES_URL);
            using var response = HttpClient.Send(request);
            response.EnsureSuccessStatusCode();

            using var content = response.Content.ReadAsStream();

            var doc = CreateDoc(content);

            List<string> sites = new List<string>();

            foreach (var box in doc.DocumentNode.SelectNodes("//div[contains(@class, 'box')]"))
            {
                foreach (var a in box.SelectNodes(".//a"))
                {
                   var site = a.Attributes["href"].Value;
                   site = site.Split("//")[1].Split('.')[0];
                   sites.Add(site);
                }
            }

            return sites;
        }

        public static async Task<IList<string>> GetAllSitesAsync(CancellationToken token = default)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, ALL_SITES_URL);
            using var response = await HttpClient.SendAsync(request, token);
            response.EnsureSuccessStatusCode();

            using var content = await response.Content.ReadAsStreamAsync(token);

            var doc = CreateDoc(content);

            List<string> sites = new List<string>();

            foreach (var box in doc.DocumentNode.SelectNodes("//div[contains(@class, 'box')]"))
            {
                foreach (var a in box.SelectNodes(".//a"))
                {
                   var site = a.Attributes["href"].Value;
                   site = site.Split("//")[1].Split('.')[0];
                   sites.Add(site);
                }
            }

            return sites;
        }

        public static IEnumerable<string> GetAllAreas(string site)
        {
            var url = string.Format(SITE_URL_TEMPLATE, site);
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            using var response = HttpClient.Send(request);
            response.EnsureSuccessStatusCode();

            using var content = response.Content.ReadAsStream();

            var doc = CreateDoc(content);

            var raw = doc.DocumentNode.SelectNodes("//ul[contains(@class, 'sublinks')]/li/a");

            return raw.Select(a => a.Attributes["href"].Value.Split('/')[1]);
        }

        public static async Task<IEnumerable<string>> GetAllAreasAsync(string site, CancellationToken token = default)
        {
            var url = string.Format(SITE_URL_TEMPLATE, site);
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            using var response = await HttpClient.SendAsync(request, token);
            response.EnsureSuccessStatusCode();

            using var content = await response.Content.ReadAsStreamAsync(token);

            var doc = CreateDoc(content);

            var raw = doc.DocumentNode.SelectNodes("//ul[contains(@class, 'sublinks')]/li/a");

            return raw.Select(a => a.Attributes["href"].Value.Split('/')[1]);
        }

        public static async Task<IDictionary<string, ListFilter>> GetListFilters(string url, CancellationToken token = default)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            using var response = await HttpClient.SendAsync(request, token);
            response.EnsureSuccessStatusCode();

            using var content = await response.Content.ReadAsStreamAsync(token);

            var doc = CreateDoc(content);

            var listFilters = new Dictionary<string, ListFilter>();

            foreach (var listFilter in doc.DocumentNode.SelectNodes("//div[contains(@class, 'search-attribute')]"))
            {
                var filterKey = listFilter.Attributes["data-attr"];
                var filterLabels = listFilter.SelectNodes(".//label");

                // TODO: populate list filters
            }

            return listFilters;
        }

        public class ListFilter 
        {
            public string UrlKey { get; set; }
            public List<string> Options { get; set; } = new List<string>();
        }
    }
}
