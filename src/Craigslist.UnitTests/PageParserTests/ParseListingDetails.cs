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
            var request = new CraigslistListingRequest("https://seattle.craigslist.org/see/rts/d/lynnwood-hard-money-lender-fix-flips/7339959651.html");
            var content = new FileStream("7339959651.html", FileMode.Open);

            var result = _sut.ParseListingDetails(request, content);
        }
    }
}
