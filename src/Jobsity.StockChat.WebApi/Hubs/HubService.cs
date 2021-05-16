using Jobsity.StockChat.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;

namespace Jobsity.StockChat.WebApi.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HubService : Hub
    {
        private const string ClientMethodName = "receiveMessageFromServer";
        private readonly IMessageAnalyserService _messageAnalyserService;

        public HubService(IMessageAnalyserService messageAnalyserService)
        {
            _messageAnalyserService = messageAnalyserService;
        }

        public async Task SendMessage(string user, string message)
        {
            await SendMessageToClient(user, message);
        }

        public async Task ReceiveMessageFromClient(string message)
        {
            var commands = _messageAnalyserService.GetCommands(message);

            await SendMessageToClient(Context.UserIdentifier, message);
        }

        private async Task SendMessageToClient(string user, string message)
        {
            await Clients.All.SendAsync(ClientMethodName, user, message);
        }
    }
}