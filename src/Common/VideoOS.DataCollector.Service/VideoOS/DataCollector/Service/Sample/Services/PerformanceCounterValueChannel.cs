using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using VideoOS.DataCollector.Service.Configuration;
using VideoOS.DataCollector.Service.Entities.DataCollection;

namespace VideoOS.DataCollector.Service.Sample.Services;

public class PerformanceCounterValueChannel : IPerformanceCounterValueChannel
{
	private Channel<PerformanceCounterValue> _channel;

	public int ReaderCount => _channel.Reader.Count;

	public PerformanceCounterValueChannel(IOptions<PerformanceCounterOptions> perfCounterOptions)
	{
		PerformanceCounterOptions value = perfCounterOptions.Value;
		BoundedChannelOptions options = new BoundedChannelOptions(perfCounterOptions.Value.MaxQueueSize)
		{
			SingleWriter = true,
			SingleReader = true,
			FullMode = BoundedChannelFullMode.DropOldest
		};
		_channel = Channel.CreateBounded<PerformanceCounterValue>(options);
	}

	public IAsyncEnumerable<PerformanceCounterValue> ReadChannelValues(CancellationToken stoppingToken)
	{
		return _channel.Reader.ReadAllAsync(stoppingToken);
	}

	public bool ReaderTryPeek(out PerformanceCounterValue? value)
	{
		return _channel.Reader.TryPeek(out value);
	}

	public ValueTask WriteAsync(PerformanceCounterValue item, CancellationToken cancellationToken)
	{
		return _channel.Writer.WriteAsync(item, cancellationToken);
	}
}
