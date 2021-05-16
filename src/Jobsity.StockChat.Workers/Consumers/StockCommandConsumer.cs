using Jobsity.StockChat.Application.Models;
using Jobsity.StockChat.Application.Services;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Jobsity.StockChat.Workers.Consumers
{
    public class StockCommandConsumer : ConsumerBase, IConsumer<CommandMessage>
    {
        private readonly IStockRequestService _stockRequestService;
        private readonly IStockQuotePublisher _stockQuotePublisher;

        public StockCommandConsumer(IStockRequestService stockRequestService, IStockQuotePublisher stockQuotePublisher, ILogger<ConsumerBase> logger)
            : base(logger)
        {
            _stockRequestService = stockRequestService;
            _stockQuotePublisher = stockQuotePublisher;
        }

        public async Task Consume(ConsumeContext<CommandMessage> context)
        {
            var message = context.Message;

            var symbol = message.Command[(message.Command.IndexOf('=') + 1)..];
            var stockQuote = await _stockRequestService.Request(symbol);

            await _stockQuotePublisher.PublishStockQuote(stockQuote);
        }
    }
}
