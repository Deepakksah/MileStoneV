using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using Microsoft.Extensions.Logging;
using VideoOS.DataCollector.Service.Configuration;
using VideoOS.DataCollector.Service.Entities.DataCollection;

namespace VideoOS.DataCollector.Service.Sample.Services;

public class PerformanceCounterReaderCategory : IDisposable
{
	private readonly PerformanceCounterCategory _category;

	private DateTime _collectionTime;

	private InstanceDataCollectionCollection? _data;

	private InstanceDataCollectionCollection? _previousData;

	private readonly ReaderWriterLockSlim _lock;

	private bool _initialized;

	private readonly ILogger<PerformanceCounterReaderService> _logger;

	private readonly RuleOptions _options;

	public PerformanceCounterReaderCategory(string category, ILogger<PerformanceCounterReaderService> logger, RuleOptions options)
	{
		_category = new PerformanceCounterCategory(category);
		_collectionTime = DateTime.MinValue;
		_lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
		_logger = logger;
		_options = options;
	}

	public void DoSample()
	{
		if (TimeToCollect())
		{
			CollectData();
		}
	}

	public PerformanceCounterValue GetValue(string counterName, string instanceName)
	{
		if (!_initialized)
		{
			EnsureCalculatedValueInitialization();
		}
		_lock.EnterReadLock();
		try
		{
			return GetInstanceData(counterName, instanceName);
		}
		finally
		{
			_lock.ExitReadLock();
		}
	}

	private void EnsureCalculatedValueInitialization()
	{
		DoSample();
		Thread.Sleep(_options.CounterStateMinSampleIntervalMS);
		DoSample();
		_initialized = true;
	}

	private PerformanceCounterValue GetInstanceData(string counterName, string instanceName)
	{
		PerformanceCounterValue performanceCounterValue = new PerformanceCounterValue(instanceName, counterName, _category.CategoryName)
		{
			CollectionTime = _collectionTime,
			CalculatedValue = 0f,
			RawValue = 0L
		};
		if (!HasInstanceData(_data, counterName, instanceName))
		{
			return new UnknownPerformanceCounterValue();
		}
		CounterSample sample = _data[counterName][instanceName].Sample;
		performanceCounterValue.RawValue = sample.RawValue;
		if (!HasInstanceData(_previousData, counterName, instanceName))
		{
			return new UnknownPerformanceCounterValue();
		}
		CounterSample sample2 = _previousData[counterName][instanceName].Sample;
		performanceCounterValue.CalculatedValue = GetCalculatedValue(sample, sample2, counterName);
		return performanceCounterValue;
	}

	private float GetCalculatedValue(CounterSample currentSample, CounterSample previousSample, string counterName)
	{
		try
		{
			return CounterSampleCalculator.ComputeCounterValue(previousSample, currentSample);
		}
		catch (InvalidOperationException ex)
		{
			_logger.LogError("Failed to sample for {0} {1}", _category.CategoryName ?? string.Empty, counterName ?? string.Empty);
			_logger.LogError(ex.ToString());
			return 0f;
		}
		catch (Win32Exception ex2)
		{
			_logger.LogError("Failed to sample for {0} {1}", _category.CategoryName ?? string.Empty, counterName ?? string.Empty);
			_logger.LogError(ex2.ToString());
			return 0f;
		}
	}

	private static bool HasInstanceData(InstanceDataCollectionCollection? data, string counterName, string instanceName)
	{
		if (data?[counterName] != null)
		{
			return data[counterName][instanceName] != null;
		}
		return false;
	}

	private bool TimeToCollect()
	{
		_lock.EnterReadLock();
		try
		{
			return (DateTime.UtcNow - _collectionTime).TotalMilliseconds >= (double)_options.CounterStateMinSampleIntervalMS;
		}
		finally
		{
			_lock.ExitReadLock();
		}
	}

	private void CollectData()
	{
		_lock.EnterWriteLock();
		try
		{
			if (TimeToCollect())
			{
				InstanceDataCollectionCollection currentData = _category.ReadCategory();
				UpdateSampleData(currentData);
			}
		}
		catch (InvalidOperationException ex)
		{
			_logger.LogError(ex.ToString());
		}
		catch (Win32Exception ex2)
		{
			_logger.LogError(ex2.ToString());
		}
		catch (UnauthorizedAccessException ex3)
		{
			_logger.LogError(ex3.ToString());
		}
		finally
		{
			_lock.ExitWriteLock();
		}
	}

	private void UpdateSampleData(InstanceDataCollectionCollection currentData)
	{
		_previousData = _data;
		_data = currentData;
		_collectionTime = DateTime.UtcNow;
	}

	public void Dispose()
	{
		_lock.Dispose();
	}
}
