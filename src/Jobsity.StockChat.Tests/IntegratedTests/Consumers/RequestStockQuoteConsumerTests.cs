using FluentAssertions;
using Jobsity.StockChat.Application.Constants;
using Jobsity.StockChat.Application.Infrastructure.MessageBroker;
using Jobsity.StockChat.Application.Models;
using Jobsity.StockChat.Application.Services;
using Jobsity.StockChat.Application.Settings;
using Jobsity.StockChat.Tests.IntegratedTests.Fixture;
using Jobsity.StockChat.Workers.Consumers;
using MassTransit;
using NSubstitute;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Jobsity.StockChat.Tests.IntegratedTests.Consumers
{
    [Trait("Integrated tests", "Consumers")]
    public class RequestStockQuoteConsumerTests
    {
        private readonly RequestStockQuoteConsumer _requestStockQuoteConsumer;
        private readonly RabbitMqFixture<StockQuote> _rabbitMqFixture;
        private readonly IPublisher _publisher;

        public RequestStockQuoteConsumerTests()
        {
            IStockRequester stockRequest;

            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            httpClientFactory.CreateClient().Returns(new HttpClient());

            var messageBrokerSetting = new MessageBrokerSetting() { Protocol = "rabbitmq://", Host = "localhost", Vhost = "/", Username = "guest", Password = "guest" };

            var busFactory = new BusFactory(messageBrokerSetting);
            _publisher = new Publisher(busFactory, messageBrokerSetting);

            _rabbitMqFixture = new RabbitMqFixture<StockQuote>(busFactory.Create(), QueueNames.ResponseStockQuote);

            stockRequest = new StockRequester(httpClientFactory, new StooqSetting() { Url = "https://stooq.com/q/l/?f=sd2t2ohlcv&h&e=csv&s=" });
            _requestStockQuoteConsumer = new RequestStockQuoteConsumer(stockRequest, new StockQuotePublisher(_publisher));
        }

        [Fact(DisplayName ="Given a valid symbol when published on message broker then request stock quote and publish")]
        public async Task Given_Valid_Symbol_When_Published_Message_Broker_Then_Request_Stock_Quote_And_Publish()
        {
            //Arrange
            var symbol = "/stock=tsla.us";
            var consumeContext = Substitute.For<ConsumeContext<CommandMessage>>();

            consumeContext.Message.Returns(new CommandMessage() { Command = symbol });

            //Act
            await _requestStockQuoteConsumer.Consume(consumeContext);

            //Assert
            _rabbitMqFixture.Received.Select<StockQuote>().Any();
            var commandMessage = _rabbitMqFixture.GetLastMessage();
            commandMessage.Symbol.Should().Be("TSLA.US");
        }
    }
}
