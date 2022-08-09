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
            new Regex(@"https?\://(\w+)\.craigslist\.org(/d/[-\w]+)?/search/(\w{3})(/(\w{3}))?(\?(\S*))?", RegexOptions.Compiled);

        public string Site { get; }
        public string? Area { get; }
        public string Category { get; }

        public Uri Url => CreateRequestUrl();

        private IDictionary<string, object> _queryParameters;

        public SearchRequest(string url)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                throw new ArgumentException("Invalid url", nameof(url));

            var match = _urlRegex.Match(url);

            if (!match.Success)
                throw new ArgumentException("Unable to parse url.", nameof(url));

            Site = match.Groups[1].Value;

            if (!match.Groups[4].Success)
            {
                Category = match.Groups[3].Value;
            }
            else
            {
                Area = match.Groups[3].Value;
                Category = match.Groups[5].Value;
            }

            if (match.Groups[7].Success)
            {
                _queryParameters = match.Groups[7].Value
                    .Split('&')
                    .Select(p => p.Split('='))
                    .ToDictionary(p => p[0], p => (object)Uri.UnescapeDataString(p[1]));
            }
            else
            {
                _queryParameters = new Dictionary<string, object>();
            }
        }

        public SearchRequest(string site, string category)
            : this(site, default, category) { }

        public SearchRequest(string site, string? area, string category)
            : this(site, area, category, new Dictionary<string, object>()) { }

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
            string str;
            if (value is bool)
            {
                str = (bool)value ? "1" : "0";
            }
            else if (value is DateTime dt)
            {
                str = dt.ToString("yyyy-MM-dd");
            }
            else if (value is Enum enumValue)
            {
                str = GetEnumQueryStringValue(enumValue) ?? ((int)value).ToString();
            }
            else
            {
                str = value?.ToString() ?? "";
            }
            return Uri.EscapeUriString(str);
        }

        private string? GetEnumQueryStringValue(Enum value)
        {
            var enumType = value.GetType();
            var name = Enum.GetName(enumType, value);
            var strValue = enumType.GetField(name!)?
                .GetCustomAttributes(false)
                .OfType<QueryStringValueAttribute>()
                .SingleOrDefault()?
                .Value;
            return strValue;
        }
    }
}