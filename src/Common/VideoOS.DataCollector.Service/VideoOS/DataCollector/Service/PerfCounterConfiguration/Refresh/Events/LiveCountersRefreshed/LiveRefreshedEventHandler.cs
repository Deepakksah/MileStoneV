using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using VideoOS.DataCollector.Service.PerfCounterConfiguration.Refresh.Events.ActivePerformanceCountersRefreshed;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Refresh.Events.LiveCountersRefreshed;

public class LiveRefreshedEventHandler : INotificationHandler<LiveRefreshedEvent>
{
	private readonly ILogger<ActiveRefreshedEventHandler> _logger;

	public LiveRefreshedEventHandler(ILogger<ActiveRefreshedEventHandler> logger)
	{
		_logger = logger;
	}

	public Task Handle(LiveRefreshedEvent notification, CancellationToken cancellationToken)
	{
		_logger.LogInformation(string.Format(CultureInfo.InvariantCulture, "Successfully maintained live counters; currently active live counters: {0}", notification.Item.Count));
		return Task.CompletedTask;
	}
}
