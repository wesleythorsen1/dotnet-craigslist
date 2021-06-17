using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Craigslist
{
    public partial class CraigslistSearchRequest
    {
        private static readonly Regex _urlRegex = 
            new Regex(@"https?\://(\w+)\.craigslist\.org/search/(\w{3})(/(\w{3}))?(\?(\S*))", RegexOptions.Compiled);

        public string Site { get; set; }
        public string? Area { get; set; }
        public string Category { get; set; }

        public Uri Uri => CreateRequestUrl();

        private IDictionary<string, object> _queryParameters;

        public CraigslistSearchRequest(string url)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                throw new ArgumentException("Invalid url", nameof(url));

            var match = _urlRegex.Match(url);

            if (match.Success == false)
                throw new ArgumentException("Unable to parse url.", nameof(url));

            Site = match.Groups[1].Value;

            if (match.Groups[3].Success == false)
            {
                Category = match.Groups[2].Value;
            }
            else
            {
                Area = match.Groups[2].Value;
                Category = match.Groups[4].Value;
            }
            
            _queryParameters = match.Groups[6].Value
                .Split('&')
                .Select(p => p.Split('='))
                .ToDictionary(p => p[0], p => (object)p[1]);
        }

        public CraigslistSearchRequest(string site, string category)
            : this(site, default, category)
        {
        }

        public CraigslistSearchRequest(string site, string? area, string category)
            : this(site, area, category, new Dictionary<string, object>())
        {
        }

        internal CraigslistSearchRequest(string site, string? area, string category, IDictionary<string, object> parameterStore)
        {
            Site = site ?? throw new ArgumentNullException(nameof(site));
            Category = category ?? throw new ArgumentNullException(nameof(category));
            Area = area;

            _queryParameters = parameterStore;
        }

        protected void SetParameter<T>(string key, T value)
        {
            if (EqualityComparer<T>.Default.Equals(value, default(T)))
            {
                _queryParameters.Remove(key);
                return;
            }
            
            _queryParameters[key] = value!;
        }

        protected T? GetParameter<T>(string key)
        {
            if (_queryParameters.TryGetValue(key, out var value))
            {
                return (T)value;
            }
            return default;
        }

        private Uri CreateRequestUrl()
        {
            var builder = new UriBuilder()
            {
                Scheme = "http",
                Host = $"{Site}.craigslist.org"
            };

            builder.Path = "search";
            if (Area != default)
            {
                builder.Path += $"/{Area}";
            }
            builder.Path += $"/{Category}";

            builder.Query = string.Join('&', _queryParameters.Select(kvp => $"{kvp.Key}={ToClQpString(kvp.Value)}"));

            return builder.Uri;
        }

        private string ToClQpString(object value)
        {
            if (value is bool)
            {
                value = (bool)value ? "1" : "0";
            }
            var str = value.ToString();
            return Uri.EscapeUriString(str!);
        }
    }
}