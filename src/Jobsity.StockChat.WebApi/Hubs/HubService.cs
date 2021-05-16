using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Jobsity.StockChat.WebApi.Hubs
{
    public class HubService : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("receiveMessage", user, message);
        }

        public async Task ReceiveMessage(string user, string message)
        {
            await Clients.All.SendAsync("receiveMessage", user, message);
        }
    }
}
