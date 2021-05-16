using Jobsity.StockChat.Application.Models;
using Jobsity.StockChat.Application.Services;
using MassTransit;
using System.Threading.Tasks;

namespace Jobsity.StockChat.Workers.Consumers
{
    public class StockCommandConsumer : IConsumer<CommandMessage>
    {
        private readonly IStockRequestService _stockRequestService;

        public StockCommandConsumer(IStockRequestService stockRequestService)
        {
            _stockRequestService = stockRequestService;
        }

        public Task Consume(ConsumeContext<CommandMessage> context)
        {
            var message = context.Message;

            return null;
        }
    }
}
