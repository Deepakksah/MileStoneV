using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Management.Server;

public interface IScheduledActionConfigurationProvider<T>
{
	Task<T> GetActionConfigurationAsync(CancellationToken token);
}
