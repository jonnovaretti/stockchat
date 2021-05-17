namespace Jobsity.StockChat.Application.Settings
{
    public class MessageBrokerSetting : IMessageBrokerSetting
    {
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Vhost { get; set; }
        public string Protocol { get; set; }
    }
}