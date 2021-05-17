namespace Jobsity.StockChat.Application.Services
{
    public interface ITokenService
    {
        string GenerateToken(string name, string role);
    }
}