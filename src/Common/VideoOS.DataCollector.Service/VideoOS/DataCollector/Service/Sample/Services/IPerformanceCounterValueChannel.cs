using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.DataCollector.Service.Entities.DataCollection;

namespace VideoOS.DataCollector.Service.Sample.Services;

public interface IPerformanceCounterValueChannel
{
	int ReaderCount { get; }

	ValueTask WriteAsync(PerformanceCounterValue item, CancellationToken cancellationToken);

	IAsyncEnumerable<PerformanceCounterValue> ReadChannelValues(CancellationToken stoppingToken);

	bool ReaderTryPeek(out PerformanceCounterValue? value);
}
