using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Globalization;
using System.Linq;

namespace VideoOS.Management.Server;

public class GeminiPerformanceCounterSet
{
	private readonly ConcurrentDictionary<string, CounterSetInstance> _instances = new ConcurrentDictionary<string, CounterSetInstance>(StringComparer.OrdinalIgnoreCase);

	private readonly object _lockObject = new object();

	private readonly CounterSet _counterSet;

	private readonly string _counterSetName;

	public readonly SynchronizedReadOnlyCollection<GeminiPerformanceCounterInfo> _counters;

	private CounterSetInstance _defaultInstance;

	private bool _isMultiInstance;

	public GeminiPerformanceCounter this[int counterId]
	{
		get
		{
			if (_defaultInstance == null)
			{
				_defaultInstance = _instances.Single().Value;
			}
			CounterData counterData = _defaultInstance.Counters[counterId];
			if (counterData != null)
			{
				return new GeminiPerformanceCounter(counterData);
			}
			throw new Exception(string.Format(CultureInfo.InvariantCulture, "Counter set '{0}' does not contain a counter with id {1}", _counterSetName, counterId));
		}
	}

	public GeminiPerformanceCounter this[int counterId, string instanceName]
	{
		get
		{
			if (_instances.TryGetValue(instanceName, out var value))
			{
				return new GeminiPerformanceCounter(value.Counters[counterId]);
			}
			RegisterInstance(instanceName);
			return this[counterId, instanceName];
		}
	}

	public bool IsMultiInstance => _isMultiInstance;

	public GeminiPerformanceCounterSet(Guid providerId, Guid counterSetId, string counterSetName, IReadOnlyCollection<GeminiPerformanceCounterInfo> counters, bool isMultiInstance)
	{
		_counterSetName = counterSetName;
		_counters = new SynchronizedReadOnlyCollection<GeminiPerformanceCounterInfo>(_lockObject, counters);
		_counterSet = new CounterSet(providerId, counterSetId, isMultiInstance ? CounterSetInstanceType.MultipleAggregate : CounterSetInstanceType.GlobalAggregate);
		_isMultiInstance = isMultiInstance;
		foreach (GeminiPerformanceCounterInfo counter in _counters)
		{
			try
			{
				_counterSet.AddCounter(counter.Id, counter.CounterType, counter.Name);
			}
			catch (ArgumentException)
			{
			}
		}
		if (!_isMultiInstance)
		{
			RegisterInstance(_counterSetName);
		}
	}

	private void RegisterInstance(string instanceName)
	{
		if (_instances.TryGetValue(instanceName, out var value))
		{
			return;
		}
		try
		{
			value = _counterSet.CreateCounterSetInstance(instanceName);
			foreach (GeminiPerformanceCounterInfo counter in _counters)
			{
				value.Counters[counter.Name].Value = 0L;
			}
			if (!_instances.TryAdd(instanceName, value))
			{
				throw new Exception(string.Format(CultureInfo.InvariantCulture, "Could not add CounterSetInstance '{0}' to the collection. It may already exist.", instanceName));
			}
		}
		catch (ArgumentException)
		{
		}
	}
}
