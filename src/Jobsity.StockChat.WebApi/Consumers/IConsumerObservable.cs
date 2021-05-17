using System.Threading.Tasks;

namespace Jobsity.StockChat.WebApi.Consumers
{
    public interface IConsumerObservable
    {
        void Attach(IConsumerObserver observer);
        Task Notify(string message);
    }
}
