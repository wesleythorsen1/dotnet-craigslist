using System.IO;
using Xunit;

namespace Craigslist.UnitTests
{
    public class ParseSearchResults
    {
        private readonly PageParser _sut;

        public ParseSearchResults()
        {
            _sut = new PageParser();
        }
        
        [Fact]
        public void CanParseSearchContent()
        {
            var request = new CraigslistSearchRequest("https://site.craigslist.org/search/area/sss");
            var content = new FileStream("search_results_1.html", FileMode.Open);

            var result = _sut.ParseSearchResults(request, content);
            
            Assert.NotEmpty(result.Listings);
            Assert.NotNull(result.Request);
        }
    }
}
