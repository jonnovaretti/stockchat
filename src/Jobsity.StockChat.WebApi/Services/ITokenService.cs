using Jobsity.StockChat.WebApi.Models;

namespace Jobsity.StockChat.WebApi.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}