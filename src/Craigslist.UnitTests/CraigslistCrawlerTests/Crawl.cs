using Moq;
using Xunit;

namespace Craigslist.UnitTests
{
    public class Crawl
    {
        private readonly Mock<ICraigslistClient> _craigslistClient;
        private readonly CraigslistCrawler _sut;

        public Crawl()
        {
            _craigslistClient = new Mock<ICraigslistClient>();
            _sut = new CraigslistCrawler(_craigslistClient.Object);
        }
        
        [Fact]
        public void WhenZeroMaxResults_DoesNotSearch()
        {
            // arrange
            // act
            _sut.Crawl(default, 0);

            // assert
            _craigslistClient.VerifyNoOtherCalls();
        }
        
        [Fact]
        public void WhenNoSearchResults_DoesNotSearchListingDetails()
        {
            // arrange
            var request = new CraigslistSearchRequest("site", "category");
            _craigslistClient
                .Setup(c => c.Search(It.IsAny<CraigslistSearchRequest>()))
                .Returns(new CraigslistSearchResults(request));

            // act
            _sut.Crawl(request);

            // assert
            _craigslistClient.Verify(c => c.Search(request), Times.Once);
            _craigslistClient.Verify(c => c.GetListing(It.IsAny<CraigslistListingRequest>()), Times.Never);
            _craigslistClient.VerifyNoOtherCalls();
        }
        
        [Fact]
        public void WhenSearchResults_SearchesListingDetails()
        {
            // arrange
            var request = new CraigslistSearchRequest("site", "category");
            var listing = new CraigslistListing("id", "https://seattle.craigslist.org/see/rts/123456.html", default, "title");
            _craigslistClient
                .Setup(c => c.Search(It.IsAny<CraigslistSearchRequest>()))
                .Returns(new CraigslistSearchResults(request)
                {
                    Listings = new [] { listing },
                });

            // act
            _sut.Crawl(request);

            // assert
            _craigslistClient.Verify(c => c.Search(request), Times.Once);
            _craigslistClient.Verify(c => c.GetListing(It.IsAny<CraigslistListingRequest>()), Times.Once);
            _craigslistClient.VerifyNoOtherCalls();
        }
        
        [Fact]
        public void WhenSearchResults_SearchesMultiplePages()
        {
            // arrange
            var request = new CraigslistSearchRequest("site", "category");
            var listing1 = new CraigslistListing("12345", "https://seattle.craigslist.org/see/rts/12345.html", default, "title");
            var listing2 = new CraigslistListing("67890", "https://seattle.craigslist.org/see/rts/67890.html", default, "title");
            _craigslistClient
                .SetupSequence(c => c.Search(It.IsAny<CraigslistSearchRequest>()))
                .Returns(new CraigslistSearchResults(request)
                {
                    Listings = new [] { listing1 },
                    Next = "https://seattle.craigslist.org/search/see/sss",
                })
                .Returns(new CraigslistSearchResults(request)
                {
                    Listings = new [] { listing2 },
                });

            // act
            _sut.Crawl(request);

            // assert
            _craigslistClient.Verify(c => c.Search(It.IsAny<CraigslistSearchRequest>()), Times.Exactly(2));
            _craigslistClient.Verify(c => c.GetListing(It.Is<CraigslistListingRequest>(r => r.Id == listing1.Id)), Times.Once);
            _craigslistClient.Verify(c => c.GetListing(It.Is<CraigslistListingRequest>(r => r.Id == listing2.Id)), Times.Once);
            _craigslistClient.VerifyNoOtherCalls();
        }
    }
}
