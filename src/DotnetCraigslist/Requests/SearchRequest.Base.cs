using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DotnetCraigslist
{
    public partial class SearchRequest
    {
        private static readonly Regex _urlRegex = 
            new Regex(@"https?\://(\w+)\.craigslist\.org/search/(\w{3})(/(\w{3}))?(\?(\S*))?", RegexOptions.Compiled);

        public string Site { get; }
        public string? Area { get; }
        public string Category { get; }

        public Uri Uri => CreateRequestUri();

        private IDictionary<string, object> _queryParameters;

        public SearchRequest(string url)
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

            if (match.Groups[6].Success)
            {
                _queryParameters = match.Groups[6].Value
                    .Split('&')
                    .Select(p => p.Split('='))
                    .ToDictionary(p => p[0], p => (object)p[1]);
            }
            else
            {
                _queryParameters = new Dictionary<string, object>();
            }
        }

        public SearchRequest(string site, string category)
            : this(site, default, category)
        {
        }

        public SearchRequest(string site, string? area, string category)
            : this(site, area, category, new Dictionary<string, object>())
        {
        }

        public SearchRequest(string site, string? area, string category, IEnumerable<KeyValuePair<string, object>> queryParameters)
        {
            Site = site ?? throw new ArgumentNullException(nameof(site));
            Category = category ?? throw new ArgumentNullException(nameof(category));
            Area = area;

            _queryParameters = queryParameters?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ?? throw new ArgumentNullException(nameof(queryParameters));
        }

        public void SetParameter<T>(string key, T value)
        {
            if (EqualityComparer<T>.Default.Equals(value, default(T)))
            {
                _queryParameters.Remove(key);
                return;
            }
            
            _queryParameters[key] = value!;
        }

        public T? GetParameter<T>(string key)
        {
            if (_queryParameters.TryGetValue(key, out var value))
            {
                return (T)value;
            }
            return default;
        }

        private Uri CreateRequestUri()
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

            builder.Query = CreateQueryString();

            return builder.Uri;
        }

        private string CreateQueryString()
        {
            var parts = new List<string>();

            foreach (var kvp in _queryParameters)
            {
                if (kvp.Value is not string && kvp.Value is IEnumerable innerValues)
                {
                    foreach (var innerValue in innerValues)
                    {
                        parts.Add($"{kvp.Key}={EscapeQpValue(innerValue)}");
                    }
                }
                else
                {
                    parts.Add($"{kvp.Key}={EscapeQpValue(kvp.Value)}");
                }
            }

            return string.Join('&', parts);

        }

        private string EscapeQpValue(object value)
        {
            if (value is bool)
            {
                value = (bool)value ? "1" : "0";
            }
            else if (value is DateTime dt)
            {
                value = dt.ToString("yyyy-MM-dd");
            }
            else if (value is Enum)
            {
                value = (int)value;
            }
            var str = value.ToString();
            return Uri.EscapeUriString(str!);
        }
    }
}