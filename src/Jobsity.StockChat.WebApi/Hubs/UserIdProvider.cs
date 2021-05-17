using Microsoft.AspNetCore.SignalR;

namespace Jobsity.StockChat.WebApi.Hubs
{
    public class UserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.Identity?.Name;
        }
    }
}
