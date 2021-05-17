using Jobsity.StockChat.Application.Models;
using System.Threading.Tasks;

namespace Jobsity.StockChat.Application.Services
{
    public interface IStockQuotePublisher
    {
        Task PublishStockQuote(StockQuote stockQuote);
    }
}