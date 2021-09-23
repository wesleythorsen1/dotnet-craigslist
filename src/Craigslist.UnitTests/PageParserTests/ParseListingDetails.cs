using System.IO;
using Xunit;

namespace Craigslist.UnitTests
{
    public class ParseListingDetails
    {
        private readonly PageParser _sut;

        public ParseListingDetails()
        {
            _sut = new PageParser();
        }
        
        [Fact]
        public void Test1()
        {
            // arrange
            var request = new ListingRequest("https://site.craigslist.org/aaa/apa/123.html");
            var content = new FileStream("listing_detail_1.html", FileMode.Open);

            // act
            var result = _sut.ParseListingDetails(request, content);

            // assert
            Assert.NotNull(result);
            Assert.NotNull(result.Request);
            Assert.Equal("123", result.Id);
        }
    }
}
