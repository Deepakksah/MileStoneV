using System;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Commands.RegisterDataCollector;

public interface IConfigurationCache : IDisposable
{
	Task<int> GetOrAddDataCollectorIdAsync(CancellationToken cancellationToken);
}
