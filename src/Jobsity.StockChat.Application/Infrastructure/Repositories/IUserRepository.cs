using Jobsity.StockChat.Application.Models;

namespace Jobsity.StockChat.Application.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        User Get(string username, string password);
    }
}