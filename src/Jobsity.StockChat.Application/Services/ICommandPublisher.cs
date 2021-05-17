using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jobsity.StockChat.Application.Services
{
    public interface ICommandPublisher
    {
        Task PublishCommands(IEnumerable<string> commands);
    }
}