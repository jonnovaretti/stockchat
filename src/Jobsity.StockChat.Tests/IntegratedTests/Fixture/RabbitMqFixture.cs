using MassTransit;
using MassTransit.Testing;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Jobsity.StockChat.Tests.IntegratedTests.Fixture
{
    public class RabbitMqFixture<E> : MultiTestConsumer
        where E : class
    {
        private readonly ConsumeObserver<E> _consumeObserver;

        public RabbitMqFixture(IBusControl busControl, string queueName) : base(TimeSpan.FromSeconds(2), CancellationToken.None)
        {
            busControl.ConnectReceiveEndpoint(queueName, x =>
            {
                x.PrefetchCount = 1;
                x.Consumer<ConsumerFixture<E>>();
            });

            _consumeObserver = new ConsumeObserver<E>();
            busControl.ConnectConsumeObserver(_consumeObserver);

            Connect(busControl);
        }

        public E GetLastMessage()
        {
            return _consumeObserver.GetConsumedMessage();
        }
    }

    public class ConsumeObserver<E> : IConsumeObserver
        where E : class
    {
        private E commandMessage;

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
            commandMessage = context.Message as E;
            await Task.CompletedTask;
        }

        public E GetConsumedMessage()
        {
            return commandMessage;
        }
    }

    public class ConsumerFixture<E> : IConsumer<E>
        where E : class
    {
        public E ReceivedMessage { get; set; }

        public async Task Consume(ConsumeContext<E> context)
        {
            ReceivedMessage = context.Message;
            await Task.CompletedTask;
        }
    }
}
