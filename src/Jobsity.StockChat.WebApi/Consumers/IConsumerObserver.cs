using System.Threading.Tasks;

namespace Jobsity.StockChat.WebApi.Consumers
{
    public interface IConsumerObserver
    {
        Task Update(string message);
    }
}
