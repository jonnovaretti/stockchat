using Jobsity.StockChat.Application.Constants;
using Jobsity.StockChat.Application.Models;
using MassTransit;
using MassTransit.Testing;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Jobsity.StockChat.Tests.IntegratedTests.Fixture
{
    public class RabbitMqFixture : MultiTestConsumer
    {
        private readonly ConsumeObserver _consumeObserver;

        public RabbitMqFixture(IBusControl busControl) : base(TimeSpan.FromSeconds(2), CancellationToken.None)
        {
            busControl.ConnectReceiveEndpoint(QueueNames.RequestStockPrice, x =>
            {
                x.PrefetchCount = 1;
                x.Consumer<ConsumerFixture>();
            });

            _consumeObserver = new ConsumeObserver();
            busControl.ConnectConsumeObserver(_consumeObserver);

            Connect(busControl);
        }

        public CommandMessage GetLastMessage()
        {
            return _consumeObserver.GetConsumedMessage();
        }
    }

    public class ConsumeObserver : IConsumeObserver
    {
        private CommandMessage commandMessage;

        public async Task ConsumeFault<T>(ConsumeContext<T> context, Exception exception) where T : class
        {
            await Task.CompletedTask;
        }

        public async Task PostConsume<T>(ConsumeContext<T> context) where T : class
        {
            await Task.CompletedTask;
        }

        public async Task PreConsume<T>(ConsumeContext<T> context) where T : class
        {
            commandMessage = context.Message as CommandMessage;
            await Task.CompletedTask;
        }

        public CommandMessage GetConsumedMessage()
        {
            return commandMessage;
        }
    }

    public class ConsumerFixture : IConsumer<CommandMessage>
    {
        public CommandMessage ReceivedMessage { get; set; }

        public async Task Consume(ConsumeContext<CommandMessage> context)
        {
            ReceivedMessage = context.Message;
        }
    }
}
