using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Craigslist
{
    internal interface IPageParser
    {
        CraigslistSearchResults ParseSearchResults(CraigslistSearchRequest request, Stream content);
        CraigslistListingDetails ParseListing(CraigslistListingRequest request, Stream content);
    }

    internal class PageParser : IPageParser
    {
        public CraigslistSearchResults ParseSearchResults(CraigslistSearchRequest request, Stream content)
        {
            var doc = new HtmlDocument();
            doc.Load(content);
            throw new NotImplementedException();
        }

        public CraigslistListingDetails ParseListing(CraigslistListingRequest request, Stream content)
        {
            var doc = new HtmlDocument();
            doc.Load(content);
            throw new NotImplementedException();
        }
    }
}