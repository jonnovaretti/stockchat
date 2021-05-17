using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Jobsity.StockChat.Application.Services
{
    public class MessageAnalyser : IMessageAnalyser
    {
        public IEnumerable<string> GetCommands(string message)
        {
            var regex = new Regex("stock=([^&#]+)");
            var words = message.Split(' ');

            foreach (var word in words)
            {
                if (regex.IsMatch(word))
                    yield return word;
            }
        }
    }
}
