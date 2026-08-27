using System.Threading.Tasks;
using VideoOS.IdentityServer.EventLogs;

namespace VideoOS.IdentityServer.Services.Interfaces;

public interface IEventLogService
{
	Task Log(EventLogModel logModel);
}
