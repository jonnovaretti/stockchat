namespace Jobsity.StockChat.WebApi.ViewModels
{
    public class AuthenticateResponse
    {
        public string Username { get; }
        public string Token { get; }

        public AuthenticateResponse(string username, string token)
        {
            Username = username;
            Token = token;
        }
    }
}
