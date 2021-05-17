using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Jobsity.StockChat.WebApi.Consumers
{
    public interface IConsumerObservable
    {
        void Attach(IHubCallerClients observer);
        Task Notify(string message);
    }
}
