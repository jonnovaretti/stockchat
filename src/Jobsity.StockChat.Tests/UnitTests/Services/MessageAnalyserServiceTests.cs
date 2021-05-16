using FluentAssertions;
using Jobsity.StockChat.Application.Services;
using Xunit;

namespace Jobsity.StockChat.Tests.UnitTests.Services
{
    [Trait("Unit tests", "Services")]
    public class MessageAnalyserServiceTests
    {
        private readonly IMessageAnalyserService _messageAnalyserService;

        public MessageAnalyserServiceTests()
        {
            _messageAnalyserService = new MessageAnalyserService();
        }

        [Fact]
        public void Given_A_Message_With_One_Stock_Command_When_Analyse_Message_Then_Return_One_Matched_Command()
        {
            //Arrange
            var message = "Hello, I want to know how much is /stock=aapl.us";

            //Act
            var commands = _messageAnalyserService.GetCommands(message);

            //Assert
            commands.Should().HaveCount(1);
        }

        [Fact]
        public void Given_A_Message_With_Two_Stock_Command_When_Analyse_Message_Then_Return_One_Matched_Command()
        {
            //Arrange
            var message = "Hello, I want to know how much are /stock=aapl.us and /stock=tsla.us";

            //Act
            var commands = _messageAnalyserService.GetCommands(message);

            //Assert
            commands.Should().HaveCount(2);
        }

        [Fact]
        public void Given_A_Message_With_One_Invalid_Stock_Command_When_Analyse_Message_Then_Doesnt_Return_One_Matched_Command()
        {
            //Arrange
            var message = "Hello, I want to know how much is /stock=";

            //Act
            var commands = _messageAnalyserService.GetCommands(message);

            //Assert
            commands.Should().HaveCount(0);
        }
    }
}
