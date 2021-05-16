using FluentAssertions;
using Jobsity.StockChat.Application.Infrastructure.MessageBroker;
using Jobsity.StockChat.Application.Models;
using Jobsity.StockChat.Application.Services;
using Jobsity.StockChat.Application.Settings;
using Jobsity.StockChat.Tests.IntegratedTests.Fixture;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Jobsity.StockChat.Tests.IntegratedTests.Services
{
    public class CommandPublisherTests
    {
        private readonly ICommandPublisher _commandPublisher;
        private readonly IPublisher _publisher;
        private readonly IBusFactory _busFactory;
        private readonly RabbitMqFixture _rabbitMqFixture;

        public CommandPublisherTests()
        {
            var messageBrokerSetting = new MessageBrokerSetting() { Protocol = "rabbitmq://", Host = "localhost", Vhost = "/", Username = "guest", Password = "guest" };

            _busFactory = new BusFactory(messageBrokerSetting);
            _publisher = new Publisher(_busFactory, messageBrokerSetting);
            _commandPublisher = new CommandPublisher(_publisher);
            _rabbitMqFixture = new RabbitMqFixture(_busFactory.Create());
        }

        [Fact]
        public async Task Given_Command_When_Publish_Queue_Then_Message_Should_Be_Pusblished()
        {
            //Arrange
            var command = "/stock=tlsa.us";
            var commands = new List<string>() { command };

            //Act
            await _commandPublisher.PublishCommands(commands);

            //Arrange
            _rabbitMqFixture.Received.Select<CommandMessage>().Any();
            var commandMessage = _rabbitMqFixture.GetLastMessage();
            commandMessage.Command.Should().Be(command);
        }
    }
}
