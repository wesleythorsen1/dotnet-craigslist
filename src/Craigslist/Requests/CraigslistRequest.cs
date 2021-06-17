using System;
using System.Collections.Generic;
using System.Linq;

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
            get => GetParameter<string>(QP_QUERY);
            set => SetParameter(QP_QUERY, value);
        }
        
        public Uri Uri => CreateRequestUrl();

        private IDictionary<string, object> _queryParameters;

        public CraigslistRequest(string site, string category)
            : this(site, default, category)
        {
        }

        public CraigslistRequest(string site, string? area, string category)
            : this(site, area, category, new Dictionary<string, object>())
        {
        }

        internal CraigslistRequest(string site, string? area, string category, IDictionary<string, object> parameterStore)
        {
            Site = site ?? throw new ArgumentNullException(nameof(site));
            Category = category ?? throw new ArgumentNullException(nameof(category));
            Area = area;

            _queryParameters = parameterStore;
        }

        protected void SetParameter<T>(string key, T value)
        {
            if (EqualityComparer<T>.Default.Equals(value, default(T)) || value == null)
            {
                _queryParameters.Remove(key);
                return;
            }
            
            _queryParameters[key] = value;
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

            if (Area == default)
            {
                builder.Path = $"search/{Category}";
            }
            else
            {
                builder.Path = $"search/{Area}/{Category}";
            }

            builder.Query = string.Join('&', _queryParameters.Select(kvp => $"{kvp.Key}={Uri.EscapeUriString(kvp.Value.ToString() ?? "")}"));

            return builder.Uri;
        }
    }
}