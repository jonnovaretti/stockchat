namespace Jobsity.StockChat.Application.Settings
{
    public interface IMessageBrokerSetting
    {
        string Protocol { get; set; }
        string Host { get; set; }
        string Vhost { get; set; }
        string Password { get; set; }
        string Username { get; set; }
    }
}