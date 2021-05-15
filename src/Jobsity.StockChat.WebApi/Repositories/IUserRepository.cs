using Jobsity.StockChat.WebApi.Models;

namespace Jobsity.StockChat.WebApi.Repositories
{
    public interface IUserRepository
    {
        User Get(string username, string password);
    }
}