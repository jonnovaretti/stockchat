using Jobsity.StockChat.Application.Models;
using MassTransit;
using System.Threading.Tasks;

namespace Jobsity.StockChat.WebApi.Consumers
{
    public class ResponseStockQuoteConsumer : IConsumer<StockQuote>, IConsumerObservable
    {
        private IConsumerObserver _consumerObserver;

        public async Task Consume(ConsumeContext<StockQuote> context)
        {
            await Notify(context.Message.OutputMessage);
        }

        public async Task Notify(string message)
        {
            await _consumerObserver.Update(message);
        }

        public void Attach(IConsumerObserver observer)
        {
            _consumerObserver = observer;
        }
    }
}
