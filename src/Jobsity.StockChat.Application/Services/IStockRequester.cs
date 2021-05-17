using Jobsity.StockChat.Application.Models;
using System.Threading.Tasks;

namespace Jobsity.StockChat.Application.Services
{
    public interface IStockRequester
    {
        Task<StockQuote> Request(string symbol);
    }
}