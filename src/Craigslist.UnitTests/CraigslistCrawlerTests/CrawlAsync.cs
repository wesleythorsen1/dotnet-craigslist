using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Craigslist.UnitTests
{
    public class CrawlAsync
    {
        private readonly Mock<ICraigslistClient> _craigslistClient;
        private readonly CraigslistCrawler _sut;

        public CrawlAsync()
        {
            _craigslistClient = new Mock<ICraigslistClient>();
            _sut = new CraigslistCrawler(_craigslistClient.Object);
        }
        
        [Fact]
        public async Task WhenZeroMaxResults_DoesNotSearch()
        {
            // arrange
            // act
            await _sut.CrawlAsync(default, 0);

            // assert
            _craigslistClient.VerifyNoOtherCalls();
        }
        
        [Fact]
        public async Task WhenNoSearchResults_DoesNotSearchListingDetails()
        {
            // arrange
            var request = new CraigslistSearchRequest("site", "category");
            _craigslistClient
                .Setup(c => c.SearchAsync(It.IsAny<CraigslistSearchRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CraigslistSearchResults(request));

            // act
            await _sut.CrawlAsync(request, 120);

            // assert
            _craigslistClient.Verify(c => c.SearchAsync(request, default), Times.Once);
            _craigslistClient.Verify(c => c.GetListingAsync(It.IsAny<CraigslistListingRequest>(), default), Times.Never);
            _craigslistClient.VerifyNoOtherCalls();
        }
        
        [Fact]
        public async Task WhenSearchResults_SearchesListingDetails()
        {
            // arrange
            var request = new CraigslistSearchRequest("site", "category");
            var listing = new CraigslistListing("id", "https://seattle.craigslist.org/see/rts/123456.html", default, "title");
            _craigslistClient
                .Setup(c => c.SearchAsync(It.IsAny<CraigslistSearchRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CraigslistSearchResults(request)
                {
                    Listings = new [] { listing },
                });

            // act
            await _sut.CrawlAsync(request);

            // assert
            _craigslistClient.Verify(c => c.SearchAsync(request, default), Times.Once);
            _craigslistClient.Verify(c => c.GetListingAsync(It.IsAny<CraigslistListingRequest>(), default), Times.Once);
            _craigslistClient.VerifyNoOtherCalls();
        }
        
        [Fact]
        public async Task WhenSearchResults_SearchesMultiplePages()
        {
            // arrange
            var request = new CraigslistSearchRequest("site", "category");
            var listing1 = new CraigslistListing("12345", "https://seattle.craigslist.org/see/rts/12345.html", default, "title");
            var listing2 = new CraigslistListing("67890", "https://seattle.craigslist.org/see/rts/67890.html", default, "title");
            _craigslistClient
                .SetupSequence(c => c.SearchAsync(It.IsAny<CraigslistSearchRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CraigslistSearchResults(request)
                {
                    Listings = new [] { listing1 },
                    Next = "https://seattle.craigslist.org/search/see/sss",
                })
                .ReturnsAsync(new CraigslistSearchResults(request)
                {
                    Listings = new [] { listing2 },
                });

            // act
            await _sut.CrawlAsync(request);

            // assert
            _craigslistClient.Verify(c => c.SearchAsync(It.IsAny<CraigslistSearchRequest>(), default), Times.Exactly(2));
            _craigslistClient.Verify(c => c.GetListingAsync(It.Is<CraigslistListingRequest>(r => r.Id == listing1.Id), default), Times.Once);
            _craigslistClient.Verify(c => c.GetListingAsync(It.Is<CraigslistListingRequest>(r => r.Id == listing2.Id), default), Times.Once);
            _craigslistClient.VerifyNoOtherCalls();
        }
    }
}
