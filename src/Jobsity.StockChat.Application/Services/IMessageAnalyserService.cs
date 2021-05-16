using System.Collections.Generic;

namespace Jobsity.StockChat.Application.Services
{
    public interface IMessageAnalyserService
    {
        IEnumerable<string>  GetCommands(string message);
    }
}