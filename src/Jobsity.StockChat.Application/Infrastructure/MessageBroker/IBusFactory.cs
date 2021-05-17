using MassTransit;

namespace Jobsity.StockChat.Application.Infrastructure.MessageBroker
{
    public interface IBusFactory
    {
        IBusControl Create();
    }
}