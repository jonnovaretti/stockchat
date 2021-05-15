namespace Jobsity.StockChat.WebApi.Services
{
    public interface ITokenService
    {
        string GenerateToken(string name, string role);
    }
}