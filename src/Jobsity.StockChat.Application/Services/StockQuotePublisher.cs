using Jobsity.StockChat.Application.Constants;
using Jobsity.StockChat.Application.Infrastructure.MessageBroker;
using Jobsity.StockChat.Application.Models;
using System.Threading.Tasks;

namespace Jobsity.StockChat.Application.Services
{
    public class StockQuotePublisher
    {
        private readonly IPublisher _publisher;

        public StockQuotePublisher(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task PublishStockQuote(StockQuote stockQuote)
        {
            await _publisher.Publish(stockQuote, QueueNames.ReturnStockQuote);
        }
    }
}
