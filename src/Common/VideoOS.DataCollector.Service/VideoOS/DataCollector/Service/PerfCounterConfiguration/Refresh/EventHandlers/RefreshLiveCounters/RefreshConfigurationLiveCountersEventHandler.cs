using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using VideoOS.DataCollector.Service.Entities;
using VideoOS.DataCollector.Service.PerfCounterConfiguration.Refresh.Events;
using VideoOS.DataCollector.Service.PerfCounterConfiguration.Refresh.Events.LiveCountersRefreshed;
using VideoOS.DataCollector.Service.Persistence;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Refresh.EventHandlers.RefreshLiveCounters;

public class RefreshConfigurationLiveCountersEventHandler : INotificationHandler<RefreshConfigurationEvent>
{
	private readonly ILiveCounterRepository _repository;

	private readonly IMediator _mediator;

	private readonly ILogger<RefreshConfigurationLiveCountersEventHandler> _logger;

	public RefreshConfigurationLiveCountersEventHandler(ILiveCounterRepository repository, IMediator mediator, ILogger<RefreshConfigurationLiveCountersEventHandler> logger)
	{
		_repository = repository;
		_mediator = mediator;
		_logger = logger;
	}

	public async Task Handle(RefreshConfigurationEvent notification, CancellationToken cancellationToken)
	{
		ConcurrentDictionary<string, PerformanceCounterCollector> counters = _repository.Counters;
		IEnumerable<KeyValuePair<string, PerformanceCounterCollector>> source = counters.Where(delegate(KeyValuePair<string, PerformanceCounterCollector> apc)
		{
			KeyValuePair<string, PerformanceCounterCollector> keyValuePair2 = apc;
			return !keyValuePair2.Value.Active;
		});
		KeyValuePair<string, PerformanceCounterCollector>[] array = source.ToArray();
		KeyValuePair<string, PerformanceCounterCollector>[] array2 = array;
		foreach (KeyValuePair<string, PerformanceCounterCollector> keyValuePair in array2)
		{
			_logger.LogInformation(string.Format(CultureInfo.InvariantCulture, "Removing Live Counter (Inactive): ({0})", keyValuePair));
			counters.TryRemove(keyValuePair);
		}
		await _mediator.Publish(new LiveRefreshedEvent(counters), cancellationToken);
	}
}
