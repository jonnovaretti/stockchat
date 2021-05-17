using System.Collections.Generic;

namespace Jobsity.StockChat.Application.Services
{
    public interface IMessageAnalyser
    {
        IEnumerable<string>  GetCommands(string message);
    }
}