using Jobsity.StockChat.Application.Services;
using Jobsity.StockChat.WebApi.Consumers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Jobsity.StockChat.WebApi.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HubService : Hub, IConsumerObserver
    {
        private const string ClientMethodName = "receiveMessageFromServer";
        private readonly IMessageAnalyserService _messageAnalyserService;
        private readonly ICommandPublisher _commandPublisher;
        private readonly IConsumerObservable _consumerObservable;

        public HubService(IMessageAnalyserService messageAnalyserService, ICommandPublisher commandPublisher, IConsumerObservable consumerObservable)
        {
            _messageAnalyserService = messageAnalyserService;
            _commandPublisher = commandPublisher;
            _consumerObservable = consumerObservable;
        }

        public override async Task OnConnectedAsync()
        {
            _consumerObservable.Attach(Clients);
            await Task.CompletedTask;
        }

        public async Task Update(string message)
        {
            await SendMessageToClient("robot", message);
        }

        public async Task ReceiveMessageFromClient(string message)
        {
            var commands = _messageAnalyserService.GetCommands(message);
            await _commandPublisher.PublishCommands(commands);

            await SendMessageToClient(Context.UserIdentifier, message);
        }

        private async Task SendMessageToClient(string user, string message)
        {
            await Clients.All.SendAsync(ClientMethodName, user, message);
        }
    }
}