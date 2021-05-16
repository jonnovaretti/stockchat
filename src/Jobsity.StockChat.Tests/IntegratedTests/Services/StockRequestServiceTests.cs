using FluentAssertions;
using Jobsity.StockChat.Application.Services;
using Jobsity.StockChat.Application.Settings;
using NSubstitute;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Jobsity.StockChat.Tests.IntegratedTests.Services
{
    public class StockRequestServiceTests
    {
        private readonly IStockRequestService _stockRequestService;

        public StockRequestServiceTests()
        {
            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            httpClientFactory.CreateClient().Returns(new HttpClient());

            _stockRequestService = new StockRequestService(httpClientFactory, new StooqSetting() { Url = "https://stooq.com/q/l/?f=sd2t2ohlcv&h&e=csv&s=" });
        }

        [Fact]
        public async Task Given_Valid_Symbol_When_Request_Get_Then_Return_StockQuote_Correctly()
        {
            //Arrange
            var symbol = "tsla.us";

            //Act
            var stockQuote = await _stockRequestService.Request(symbol);

            //Assert
            stockQuote.Should().NotBeNull();
            stockQuote.Symbol.Should().Be(symbol.ToUpper());
        }

        [Fact]
        public async Task Given_Invalid_Symbol_When_Request_Get_Then_Return_StockQuote_Invalid()
        {
            //Arrange
            var symbol = "tststs.us";

            //Act
            var stockQuote = await _stockRequestService.Request(symbol);

            //Assert
            stockQuote.Should().NotBeNull();
            stockQuote.OutputMessage.Should().Contain(symbol.ToUpper());
        }
    }
}
