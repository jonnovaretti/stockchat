using Jobsity.StockChat.Application.Constants;
using Jobsity.StockChat.Application.Infrastructure.MessageBroker;
using Jobsity.StockChat.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jobsity.StockChat.Application.Services
{
    public class CommandPublisher : ICommandPublisher
    {
        private readonly IPublisher _publisher;

        public CommandPublisher(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task PublishCommands(IEnumerable<string> commands)
        {
            foreach (var command in commands)
            {
                await _publisher.Publish(new CommandMessage(command), QueueNames.RequestStockPrice);
            }
        }
    }
}
