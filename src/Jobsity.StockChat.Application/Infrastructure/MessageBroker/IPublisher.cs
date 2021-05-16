using System.Threading.Tasks;

namespace Jobsity.StockChat.Application.Infrastructure.MessageBroker
{
    public interface IPublisher
    {
        Task Publish<T>(T message, string queue);
    }
}