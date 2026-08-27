using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using VideoOS.DataCollector.Service.Common.ExtensionMethods;
using VideoOS.DataCollector.Service.Common.ResultResponse;
using VideoOS.DataCollector.Service.Entities;
using VideoOS.DataCollector.Service.Entities.DataCollection;
using VideoOS.DataCollector.Service.PerfCounterConfiguration.Commands.RegisterDataCollector;
using VideoOS.DataCollector.Service.PerfCounterConfiguration.Refresh.Events;
using VideoOS.DataCollector.Service.PerfCounterConfiguration.Refresh.Events.ActivePerformanceCountersRefreshed;
using VideoOS.DataCollector.Service.Persistence;
using VideoOS.DataCollector.Service.Services;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Refresh.EventHandlers.RefreshActivePerformanceCounters;

public class RefreshConfigurationActivePerformanceCountersEventHandler : INotificationHandler<RefreshConfigurationEvent>
{
	private readonly IActivePerformanceCounterRepository _repository;

	private readonly IConfigurationCache _configurationCache;

	private readonly IPerformanceCounterInstancesService _performanceCounterInstancesService;

	private readonly IWarningLoggedStateService _warningLogged;

	private readonly IMediator _mediator;

	private readonly ILogger<RefreshConfigurationActivePerformanceCountersEventHandler> _logger;

	public RefreshConfigurationActivePerformanceCountersEventHandler(IActivePerformanceCounterRepository repository, IConfigurationCache configurationCache, IPerformanceCounterInstancesService performanceCounterInstancesService, IWarningLoggedStateService warningLogged, IMediator mediator, ILogger<RefreshConfigurationActivePerformanceCountersEventHandler> logger)
	{
		_repository = repository;
		_configurationCache = configurationCache;
		_performanceCounterInstancesService = performanceCounterInstancesService;
		_warningLogged = warningLogged;
		_mediator = mediator;
		_logger = logger;
	}

	public async Task Handle(RefreshConfigurationEvent notification, CancellationToken cancellationToken)
	{
		ConcurrentDictionary<string, PerformanceCounterCollector> activePerformanceCounters = _repository.Counters;
		_logger.LogInformation(string.Format(CultureInfo.InvariantCulture, "Active counters before applying new configuration: {0}", activePerformanceCounters.Count));
		Dictionary<string, PerformanceCounterInstance> allCounterInstances = GetAllCounterInstances(notification.Item, _performanceCounterInstancesService, _warningLogged);
		List<(string, PerformanceCounterCollector)> newPerformanceCounterCollectors = new List<(string, PerformanceCounterCollector)>();
		List<PerformanceCounterValue> newPerformanceCounterValues = new List<PerformanceCounterValue>();
		foreach (PerformanceCounterInstance value2 in allCounterInstances.Values)
		{
			if (activePerformanceCounters.TryGetValue(value2.Path, out PerformanceCounterCollector value))
			{
				UpdateExistingCounter(value2, value);
				continue;
			}
			_logger.LogInformation(string.Format(CultureInfo.InvariantCulture, "Adding Counter: ({0}), Instance name: {1} Frequency: {2}, History enabled= {3}", value2.Path, value2.InstanceName, value2.SampleFrequency, value2.EnableHistory));
			value = new PerformanceCounterCollector(value2);
			newPerformanceCounterCollectors.Add((value2.Path, value));
			newPerformanceCounterValues.Add(new PerformanceCounterValue(value.InstanceName, value.CounterName, value.CategoryName)
			{
				CalculatedValue = 0f,
				CollectionTime = DateTime.UtcNow,
				RawValue = 0L
			}.FormatInstanceNameWithId());
		}
		KeyValuePair<string, PerformanceCounterCollector>[] notifyServerObsoleteInstances = RemoveObsoleteCounters(activePerformanceCounters, allCounterInstances).ToArray();
		if (newPerformanceCounterValues.Any())
		{
			IMediator mediator = _mediator;
			await mediator.Send((IRequest<Result<Unit>>)new AddPerformanceCounterDataWithConfigRefreshServerCommand(await _configurationCache.GetOrAddDataCollectorIdAsync(cancellationToken), newPerformanceCounterValues), cancellationToken);
		}
		foreach (var item2 in newPerformanceCounterCollectors)
		{
			activePerformanceCounters.TryAdd(item2.Item1, item2.Item2);
		}
		if (notifyServerObsoleteInstances.Any())
		{
			await _mediator.Send((IRequest<Result<Unit>>)new MarkPerformanceCounterDataAsObsoleteServerCommand(notifyServerObsoleteInstances.Select((KeyValuePair<string, PerformanceCounterCollector> i) => i.Key).ToArray()), cancellationToken);
		}
		KeyValuePair<string, PerformanceCounterCollector>[] array = notifyServerObsoleteInstances;
		foreach (KeyValuePair<string, PerformanceCounterCollector> item in array)
		{
			activePerformanceCounters.TryRemove(item);
		}
		if (newPerformanceCounterValues.Any() || notifyServerObsoleteInstances.Any())
		{
			await _mediator.Publish(new ActiveRefreshedEvent(activePerformanceCounters), cancellationToken);
		}
	}

	private List<KeyValuePair<string, PerformanceCounterCollector>> RemoveObsoleteCounters(ConcurrentDictionary<string, PerformanceCounterCollector> activePerformanceCounters, Dictionary<string, PerformanceCounterInstance> allCounterInstances)
	{
		IEnumerable<KeyValuePair<string, PerformanceCounterCollector>> source = from _003C_003Eh__TransparentIdentifier0 in Enumerable.GroupJoin(activePerformanceCounters, allCounterInstances, delegate(KeyValuePair<string, PerformanceCounterCollector> apc)
			{
				KeyValuePair<string, PerformanceCounterCollector> keyValuePair = apc;
				return keyValuePair.Key;
			}, delegate(KeyValuePair<string, PerformanceCounterInstance> n)
			{
				KeyValuePair<string, PerformanceCounterInstance> keyValuePair = n;
				return keyValuePair.Key;
			}, (KeyValuePair<string, PerformanceCounterCollector> apc, IEnumerable<KeyValuePair<string, PerformanceCounterInstance>> outer) => new { apc, outer })
			from n in outer.DefaultIfEmpty()
			where n.Key == null
			select apc;
		List<KeyValuePair<string, PerformanceCounterCollector>> list = source.ToList();
		List<KeyValuePair<string, PerformanceCounterCollector>> list2 = list;
		foreach (KeyValuePair<string, PerformanceCounterCollector> item in list)
		{
			_logger.LogInformation(string.Format(CultureInfo.InvariantCulture, "Removing Counter: ({0})", item.Key));
			activePerformanceCounters.TryRemove(item);
		}
		source = activePerformanceCounters.Where<KeyValuePair<string, PerformanceCounterCollector>>(delegate(KeyValuePair<string, PerformanceCounterCollector> apc)
		{
			KeyValuePair<string, PerformanceCounterCollector> keyValuePair = apc;
			return !keyValuePair.Value.Active;
		});
		list = source.ToList();
		list2.AddRange(list);
		foreach (KeyValuePair<string, PerformanceCounterCollector> item2 in list)
		{
			_logger.LogInformation(string.Format(CultureInfo.InvariantCulture, "Removing Counter (Inactive): ({0})", item2.Key));
		}
		return list2;
	}

	private void UpdateExistingCounter(PerformanceCounterInstance newInstance, PerformanceCounterCollector collector)
	{
		if (collector.SampleFrequency != newInstance.SampleFrequency || collector.EnableHistory != newInstance.EnableHistory)
		{
			_logger.LogInformation(string.Format(CultureInfo.InvariantCulture, "Updating Counter: ({0}). Current Frequency: {1}, New Frequency: {2}, History enabled= {3}", newInstance.Path, collector.SampleFrequency, newInstance.SampleFrequency, newInstance.EnableHistory));
			collector.SampleFrequency = newInstance.SampleFrequency;
			collector.EnableHistory = newInstance.EnableHistory;
		}
		if (collector.InstanceName != newInstance.InstanceName)
		{
			_logger.LogInformation(string.Format(CultureInfo.InvariantCulture, "Updating Counter: ({0}). Current instance name : {1}, New instance name: {2}", newInstance.Path, collector.InstanceName, newInstance.InstanceName));
			collector.InstanceName = newInstance.InstanceName;
		}
	}

	private Dictionary<string, PerformanceCounterInstance> GetAllCounterInstances(DataCollectorConfiguration configuration, IPerformanceCounterInstancesService performanceCounterInstancesService, IWarningLoggedStateService warningLogged)
	{
		Dictionary<string, PerformanceCounterInstance> dictionary = new Dictionary<string, PerformanceCounterInstance>(StringComparer.OrdinalIgnoreCase);
		for (int i = 0; i < configuration.ConfiguredCounters.Length; i++)
		{
			string categoryName = configuration.ConfiguredCounters[i].CounterInstance.CategoryName;
			PerformanceCounterCategory performanceCounterCategory = new PerformanceCounterCategory(configuration.ConfiguredCounters[i].CounterInstance.CategoryName);
			try
			{
				if (performanceCounterCategory.CounterExists(configuration.ConfiguredCounters[i].CounterInstance.CounterName))
				{
					Dictionary<string, PerformanceCounterInstance> configuredPerformanceCounterInstances = performanceCounterInstancesService.GetConfiguredPerformanceCounterInstances(performanceCounterCategory, configuration.ConfiguredCounters[i]);
					dictionary = dictionary.Union(configuredPerformanceCounterInstances).ToDictionary((KeyValuePair<string, PerformanceCounterInstance> pair) => pair.Key, (KeyValuePair<string, PerformanceCounterInstance> pair) => pair.Value);
					if (warningLogged.IsLogged(categoryName, value: false))
					{
						warningLogged.Add(categoryName, value: false);
					}
				}
				else
				{
					_logger.LogWarning(string.Format(CultureInfo.InvariantCulture, "The category named: '{0}' does not contain the counter: '{1}'", configuration.ConfiguredCounters[i].CounterInstance.CategoryName, configuration.ConfiguredCounters[i].CounterInstance.CounterName));
				}
			}
			catch (InvalidOperationException ex)
			{
				if (!warningLogged.IsLogged(categoryName, value: true))
				{
					warningLogged.Add(categoryName, value: true);
					_logger.LogWarning(string.Format(CultureInfo.InvariantCulture, "An exception happened during loading counter '{0}' from category '{1}'. {2}", configuration.ConfiguredCounters[i].CounterInstance.CounterName, configuration.ConfiguredCounters[i].CounterInstance.CategoryName, ex.ToString()));
				}
			}
		}
		return dictionary;
	}
}
