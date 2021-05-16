namespace Jobsity.StockChat.Application.Models
{
    public class CommandMessage
    {
        public string Command { get; }

        public CommandMessage(string command)
        {
            Command = command;
        }
    }
}
