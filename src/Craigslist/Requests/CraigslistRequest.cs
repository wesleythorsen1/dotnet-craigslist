using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable

namespace Craigslist
{
    public class CraigslistRequest
    {
        private const string QP_QUERY = "query";

        public string Site { get; set; }
        public string? Area { get; set; }
        public string Category { get; set; }

        public string? SearchText
        {
            get => GetParameter(QP_QUERY);
            set => SetParameter(QP_QUERY, value);
        }
        
        // public string BaseUrl => Area == default ?
        //     $"http://{Site}.craigslist.org/search/{Category}" :
        //     $"http://{Site}.craigslist.org/search/{Area}/{Category}";

        public Uri Uri => CreateRequestUrl();

        private IDictionary<string, string> _queryParameters;

        public CraigslistRequest(string site, string category)
            : this(site, default, category)
        {
        }

        public CraigslistRequest(string site, string? area, string category)
            : this(site, area, category, new Dictionary<string, string>())
        {
        }

        internal CraigslistRequest(string site, string? area, string category, IDictionary<string, string> parameterStore)
        {
            Site = site ?? throw new ArgumentNullException(nameof(site));
            Category = category ?? throw new ArgumentNullException(nameof(category));
            Area = area;

            _queryParameters = parameterStore;
        }

        protected void SetParameter(string key, object? value)
        {
            if (value == default)
            {
                _queryParameters.Remove(key);
                return;
            }
            _queryParameters[key] = value.ToString();
        }

        protected string? GetParameter(string key)
        {
            if (_queryParameters.TryGetValue(QP_QUERY, out var value))
            {
                return value;
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

            if (Area == default)
            {
                builder.Path = $"search/{Category}";
            }
            else
            {
                builder.Path = $"search/{Area}/{Category}";
            }

            builder.Query = string.Join('&', _queryParameters.Select(kvp => $"{kvp.Key}={Uri.EscapeUriString(kvp.Value)}"));

            return builder.Uri;
        }
    }
}