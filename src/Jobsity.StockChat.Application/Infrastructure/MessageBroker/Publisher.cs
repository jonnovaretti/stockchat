using Jobsity.StockChat.Application.Settings;
using System;
using System.Threading.Tasks;

namespace Jobsity.StockChat.Application.Infrastructure.MessageBroker
{
    public class Publisher : IPublisher
    {
        private readonly IBusFactory _busFactory;
        private readonly IMessageBrokerSetting _messageBrokerSetting;

        public Publisher(IBusFactory busFactory, IMessageBrokerSetting messageBrokerSetting)
        {
            _busFactory = busFactory;
            _messageBrokerSetting = messageBrokerSetting;
        }

        public async Task Publish<T>(T message, string queue)
             where T : class
        {
            var bus = _busFactory.Create();
            var address = new Uri($"{_messageBrokerSetting.Protocol}{ _messageBrokerSetting.Host }{_messageBrokerSetting.Vhost}{queue}");
            var endpoint = await bus.GetSendEndpoint(address);

            await endpoint.Send(message);
        }
    }
}
