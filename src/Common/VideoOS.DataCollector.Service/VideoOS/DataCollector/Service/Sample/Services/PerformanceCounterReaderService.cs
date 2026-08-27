using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VideoOS.DataCollector.Service.Configuration;
using VideoOS.DataCollector.Service.Entities.DataCollection;

namespace VideoOS.DataCollector.Service.Sample.Services;

public class PerformanceCounterReaderService : IDisposable
{
	private readonly ConcurrentDictionary<string, PerformanceCounterReaderCategory> _categories;

	private readonly ILogger<PerformanceCounterReaderService> _logger;

	private readonly RuleOptions _options;

	public PerformanceCounterReaderService(ILogger<PerformanceCounterReaderService> logger, IOptions<RuleOptions> options)
	{
		_categories = new ConcurrentDictionary<string, PerformanceCounterReaderCategory>();
		_logger = logger;
		_options = options.Value;
	}

	public virtual void DoSample(string categoryName)
	{
		PerformanceCounterReaderCategory category = GetCategory(categoryName);
		category.DoSample();
	}

	public virtual PerformanceCounterValue GetValue(string categoryName, string counterName, string instanceName)
	{
		PerformanceCounterReaderCategory category = GetCategory(categoryName);
		return category.GetValue(counterName, instanceName);
	}

	private PerformanceCounterReaderCategory GetCategory(string categoryName)
	{
		if (_categories.TryGetValue(categoryName, out PerformanceCounterReaderCategory value))
		{
			return value;
		}
		PerformanceCounterReaderCategory performanceCounterReaderCategory = new PerformanceCounterReaderCategory(categoryName, _logger, _options);
		value = _categories.GetOrAdd(categoryName, performanceCounterReaderCategory);
		if (!value.Equals(performanceCounterReaderCategory))
		{
			performanceCounterReaderCategory.Dispose();
		}
		return value;
	}

	public void Dispose()
	{
		foreach (KeyValuePair<string, PerformanceCounterReaderCategory> category in _categories)
		{
			category.Value.Dispose();
		}
	}
}
