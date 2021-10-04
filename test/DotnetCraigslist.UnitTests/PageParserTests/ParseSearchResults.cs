using System.IO;
using System.Linq;
using Xunit;

namespace DotnetCraigslist.UnitTests
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
            var request = new SearchRequest("https://site.craigslist.org/search/area/sss");
            var content = new FileStream("search_results_1.html", FileMode.Open);

            var result = _sut.ParseSearchResults(request, content);
            
            Assert.NotNull(result.Request);
            Assert.NotEmpty(result.Results);
        }
        
        [Fact]
        public void DoesNotReturnNearbyResults()
        {
            var request = new SearchRequest("https://site.craigslist.org/search/area/sss");
            var content = new FileStream("search_results_1.html", FileMode.Open);

            var result = _sut.ParseSearchResults(request, content);
            
            Assert.Equal(2, result.Results.Count());
        }
    }
}
