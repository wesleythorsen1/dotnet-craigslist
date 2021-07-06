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
            var request = new CraigslistListingRequest("https://seattle.craigslist.org/see/rts/d/lynnwood-hard-money-lender-fix-flips/7339959651.html");
            var content = new FileStream("7339959651.html", FileMode.Open);

            // act
            var result = _sut.ParseListingDetails(request, content);

            // assert
            Assert.NotNull(result);
            Assert.NotNull(result.Request);
            Assert.Equal("7339959651", result.Id);
        }
        
        [Fact]
        public void Test2()
        {
            // arrange
            var request = new CraigslistListingRequest("https://seattle.craigslist.org/see/apa/d/seattle-recycling-controlled-access/7344465455.html");
            var content = new FileStream("7344465455.html", FileMode.Open);

            // act
            var result = _sut.ParseListingDetails(request, content);

            // assert
            Assert.NotNull(result);
            Assert.NotNull(result.Request);
            Assert.Equal("7344465455", result.Id);
        }
    }
}
