using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VideoOS.DataCollector.Service.Configuration;
using VideoOS.DataCollector.Service.Entities;
using VideoOS.DataCollector.Service.Entities.DataCollection;
using VideoOS.DataCollector.Service.Persistence;
using VideoOS.DataCollector.Service.Sample.Events;
using VideoOS.DataCollector.Service.Sample.Services;

namespace VideoOS.DataCollector.Service.Sample.EventHandlers.Historical;

public class SampleActivePerformanceCountersEventHandler : INotificationHandler<CollectDataEvent>
{
	private readonly IActivePerformanceCounterRepository _repository;

	private readonly PerformanceCounterReaderService _reader;

	private readonly IPerformanceCounterValueChannel _channel;

	private readonly ILogger<SampleActivePerformanceCountersEventHandler> _logger;

	private readonly PerformanceCounterOptions _options;

	public SampleActivePerformanceCountersEventHandler(IActivePerformanceCounterRepository repository, PerformanceCounterReaderService reader, IPerformanceCounterValueChannel channel, ILogger<SampleActivePerformanceCountersEventHandler> logger, IOptions<PerformanceCounterOptions> options)
	{
		_repository = repository;
		_reader = reader;
		_channel = channel;
		_logger = logger;
		_options = options.Value;
	}

	public async Task Handle(CollectDataEvent notification, CancellationToken cancellationToken)
	{
		ConcurrentDictionary<string, PerformanceCounterCollector> counters = _repository.Counters;
		DateTime item = notification.Item;
		Dictionary<string, HashSet<string>> cache = null;
		List<PerformanceCounterValue> list = new List<PerformanceCounterValue>();
		foreach (PerformanceCounterCollector value3 in counters.Values)
		{
			if (!value3.InitValuesSent)
			{
				if (value3.IsCounterActive(ref cache))
				{
					value3.SampleData(item, _reader);
					PerformanceCounterValue value = value3.GetValue(_reader);
					if (!(value is UnknownPerformanceCounterValue))
					{
						list.Add(value);
						value3.InitValuesSent = true;
					}
				}
			}
			else
			{
				if (!value3.ShouldSample(item) || !value3.IsCounterActive(ref cache))
				{
					continue;
				}
				value3.SampleData(item, _reader);
				if (value3.EnableHistory)
				{
					PerformanceCounterValue value2 = value3.GetValue(_reader);
					if (!(value2 is UnknownPerformanceCounterValue))
					{
						list.Add(value2);
					}
				}
			}
		}
		bool loggedOnce = false;
		int processedItems = list.Count;
		foreach (PerformanceCounterValue item2 in list)
		{
			if (_channel.ReaderCount == _options.MaxQueueSize && !loggedOnce)
			{
				_logger.LogError($"Max queue size of {_options.MaxQueueSize} items reached - {processedItems} items more will be enqueue deleting oldest items already in queue");
				loggedOnce = true;
			}
			await _channel.WriteAsync(item2, cancellationToken);
			processedItems--;
		}
	}
}
