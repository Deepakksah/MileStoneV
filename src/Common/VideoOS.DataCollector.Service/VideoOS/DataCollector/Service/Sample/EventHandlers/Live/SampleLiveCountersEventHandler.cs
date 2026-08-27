using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VideoOS.DataCollector.Service.Configuration;
using VideoOS.DataCollector.Service.Entities;
using VideoOS.DataCollector.Service.Persistence;
using VideoOS.DataCollector.Service.Sample.Events;
using VideoOS.DataCollector.Service.Sample.Services;

namespace VideoOS.DataCollector.Service.Sample.EventHandlers.Live;

public class SampleLiveCountersEventHandler : INotificationHandler<CollectDataEvent>
{
	private readonly ILiveCounterRepository _repository;

	private readonly PerformanceCounterReaderService _reader;

	private readonly ILogger<SampleLiveCountersEventHandler> _logger;

	private readonly PerformanceCounterOptions _options;

	public SampleLiveCountersEventHandler(ILiveCounterRepository repository, PerformanceCounterReaderService reader, ILogger<SampleLiveCountersEventHandler> logger, IOptions<PerformanceCounterOptions> options)
	{
		_repository = repository;
		_reader = reader;
		_logger = logger;
		_options = options.Value;
	}

	public Task Handle(CollectDataEvent notification, CancellationToken cancellationToken)
	{
		ConcurrentDictionary<string, PerformanceCounterCollector> counters = _repository.Counters;
		DateTime item = notification.Item;
		Dictionary<string, HashSet<string>> cache = null;
		foreach (PerformanceCounterCollector value in counters.Values)
		{
			if (value.Active)
			{
				if (value.LastLiveRequest <= item.Subtract(TimeSpan.FromSeconds(_options.LiveCounterInactivityTimeoutInSeconds)))
				{
					_logger.LogInformation(string.Format(CultureInfo.InvariantCulture, "Scheduling live counter for removal due to inactivity: ({0})", value.Path));
					value.Active = false;
				}
				else if (value.ShouldSample(item) && (value.CategoryType != PerformanceCounterCategoryType.MultiInstance || value.CheckInstanceExists(ref cache)))
				{
					value.SampleData(item, _reader);
				}
			}
		}
		return Task.CompletedTask;
	}
}
