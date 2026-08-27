using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Common.EnvironmentServices;

public interface IPeriodicTimerService
{
	Task<bool> WaitForNextTickAsync();

	Task<bool> WaitForNextTickAsync(CancellationToken cancellationToken);
}
