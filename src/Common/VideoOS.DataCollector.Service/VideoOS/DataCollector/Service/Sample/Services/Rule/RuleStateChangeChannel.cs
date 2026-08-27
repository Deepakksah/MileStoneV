using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using VideoOS.DataCollector.Service.Configuration;
using VideoOS.DataCollector.Service.Rule.Entities.State;

namespace VideoOS.DataCollector.Service.Sample.Services.Rule;

public class RuleStateChangeChannel : IRuleStateChangeChannel
{
	private Channel<StateChangeData> _channel;

	public RuleStateChangeChannel(IOptions<PerformanceCounterOptions> perfCounterOptions)
	{
		BoundedChannelOptions options = new BoundedChannelOptions(perfCounterOptions.Value.MaxQueueSize)
		{
			SingleWriter = true,
			SingleReader = true,
			FullMode = BoundedChannelFullMode.DropOldest
		};
		_channel = Channel.CreateBounded<StateChangeData>(options);
	}

	public void TryWrite(StateChangeData counterStateChangeData)
	{
		_channel.Writer.TryWrite(counterStateChangeData);
	}

	public ValueTask<StateChangeData> ReadAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return _channel.Reader.ReadAsync(cancellationToken);
	}
}
