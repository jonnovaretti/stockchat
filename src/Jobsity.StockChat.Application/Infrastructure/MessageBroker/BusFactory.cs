using Jobsity.StockChat.Application.Settings;
using MassTransit;
using RabbitMQ.Client;

namespace Jobsity.StockChat.Application.Infrastructure.MessageBroker
{
    public class BusFactory : IBusFactory
    {
        private readonly IMessageBrokerSetting _messageBrokerSetting;

        public BusFactory(IMessageBrokerSetting messageBrokerSetting)
        {
            _messageBrokerSetting = messageBrokerSetting;
        }

        public IBusControl Create()
        {
            return Bus.Factory.CreateUsingRabbitMq(config =>
            {
                config.Host(_messageBrokerSetting.Host, _messageBrokerSetting.Vhost, acc =>
                 {
                     acc.Username(_messageBrokerSetting.Username);
                     acc.Password(_messageBrokerSetting.Password);
                 });

                config.ExchangeType = ExchangeType.Direct;
            });
        }
    }
}
