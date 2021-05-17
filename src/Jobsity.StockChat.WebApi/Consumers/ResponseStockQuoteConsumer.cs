using Jobsity.StockChat.Application.Models;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Jobsity.StockChat.WebApi.Consumers
{
    public class ResponseStockQuoteConsumer : IConsumer<StockQuote>, IConsumerObservable
    {
        private IHubCallerClients _clients;
        private const string ClientMethodName = "receiveMessageFromServer";

        public async Task Consume(ConsumeContext<StockQuote> context)
        {
            await Notify(context.Message.OutputMessage);
        }

        public async Task Notify(string message)
        {
            await _clients.All.SendAsync(ClientMethodName, "robot", message);
        }

        public void Attach(IHubCallerClients observer)
        {
            _clients = observer;
        }
    }
}
