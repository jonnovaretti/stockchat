using Jobsity.StockChat.Application.Constants;
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
        private readonly IMessageAnalyser _messageAnalyser;
        private readonly ICommandPublisher _commandPublisher;
        private readonly IConsumerObservable _consumerObservable;
        private IHubCallerClients _clients;

        public HubService(IMessageAnalyser messageAnalyser, ICommandPublisher commandPublisher, IConsumerObservable consumerObservable)
        {
            _messageAnalyser = messageAnalyser;
            _commandPublisher = commandPublisher;
            _consumerObservable = consumerObservable;
        }

        public override async Task OnConnectedAsync()
        {
            _clients = Clients;
            _consumerObservable.Attach(this);
            await Task.CompletedTask;
        }

        public async Task Update(string message)
        {
            await _clients.All.SendAsync(ClientMethods.ClientMethodName, "robot", message);
        }

        public async Task ReceiveMessageFromClient(string message)
        {
            var commands = _messageAnalyser.GetCommands(message);
            await _commandPublisher.PublishCommands(commands);

            await Clients.All.SendAsync(ClientMethods.ClientMethodName, Context.UserIdentifier, message);
        }
    }
}