using FluentAssertions;
using Jobsity.StockChat.Application.Services;
using Jobsity.StockChat.Application.Settings;
using NSubstitute;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Jobsity.StockChat.Tests.IntegratedTests.Services
{
    [Trait("Integrated tests", "Services")]
    public class StockRequestServiceTests
    {
        private readonly IStockRequester _stockRequest;

        public StockRequestServiceTests()
        {
            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            httpClientFactory.CreateClient().Returns(new HttpClient());

            _stockRequest = new StockRequester(httpClientFactory, new StooqSetting() { Url = "https://stooq.com/q/l/?f=sd2t2ohlcv&h&e=csv&s=" });
        }

        [Fact(DisplayName = "Given valid_symbol_when_request_get_quote then return stockquote_correctly")]
        public async Task Given_Valid_Symbol_When_Request_Get_Then_Return_StockQuote_Correctly()
        {
            //Arrange
            var symbol = "tsla.us";

            //Act
            var stockQuote = await _stockRequest.Request(symbol);

            //Assert
            stockQuote.Should().NotBeNull();
            stockQuote.Symbol.Should().Be(symbol.ToUpper());
        }

        [Fact(DisplayName = "Given an invalid symbol when request get quote then return invalid stock quote")]
        public async Task Given_Invalid_Symbol_When_Request_Get_Then_Return_Invalid_StockQuote()
        {
            //Arrange
            var symbol = "tststs.us";

            //Act
            var stockQuote = await _stockRequest.Request(symbol);

            //Assert
            stockQuote.Should().NotBeNull();
            stockQuote.OutputMessage.Should().Contain(symbol.ToUpper());
        }
    }
}
