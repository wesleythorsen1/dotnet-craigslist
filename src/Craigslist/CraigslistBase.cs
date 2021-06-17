using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craigslist
{
    public abstract class CraigslistBase
    {
        private static readonly IList<string> AllSites = Util.GetAllSites();

        private const int RESULTS_PER_REQUEST = 100;

        protected const string URL_TEMPLATE_BASE = "http://{0}.craigslist.org";
        protected const string URL_TEMPLATE_NO_AREA = "http://{0}.craigslist.org/search/{1}";
        protected const string URL_TEMPLATE_AREA = "http://{0}.craigslist.org/search/{1}/{2}";

        private const string defaultSite = "seattle";

        private string _site;
        private string _area;
        private string _category;
        private string _url;

        public CraigslistBase(
            string site = default, 
            string area = default,
            string category = default,
            string filters = default)
        {
            _site = site ?? defaultSite;
            if (!AllSites.Contains(_site))
                throw new ArgumentException("Not a valid site", nameof(site));
            
            var allAreas = Util.GetAllAreas(_site);
            if (!string.IsNullOrWhiteSpace(area))
            {
                if (!allAreas.Contains(area))
                    throw new ArgumentException("Not a valid area", nameof(area));
            }
            _area = area;

            _category = category;

            if (_area == default)
            {
                _url = string.Format(URL_TEMPLATE_NO_AREA, _site, _category);
            }
            else
            {
                _url = string.Format(URL_TEMPLATE_AREA, _site, _area, _category);
            }
        }


    }
}
