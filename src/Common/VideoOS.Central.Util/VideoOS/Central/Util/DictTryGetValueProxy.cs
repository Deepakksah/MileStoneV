using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace VideoOS.Central.Util;

public class DictTryGetValueProxy<TKey, TValue> : IDictTryGetValue<TKey, TValue>, IDisposable
{
	private readonly IDictTryGetValue<TKey, TValue> _target;

	private readonly Action _updateTarget;

	private readonly TimeSpan _updateTimeout;

	private readonly TimeSpan _keyLifeSpan;

	private readonly ILogger _logger;

	private DateTime _lastUpdateAt = DateTime.UtcNow;

	private readonly ReaderWriterLockSlim _locker = new ReaderWriterLockSlim();

	private readonly ConcurrentDictionary<TKey, DateTime> _keyLastCheckedAt = new ConcurrentDictionary<TKey, DateTime>();

	public ICollection<TKey> Keys => _keyLastCheckedAt.Keys;

	public static DictTryGetValueProxy<TKey, TValue> Create(IDictTryGetValue<TKey, TValue> target, Action updateTarget, TimeSpan updateTimeout, TimeSpan keyLifeSpan, ILogger logger)
	{
		return new DictTryGetValueProxy<TKey, TValue>(target, updateTarget, updateTimeout, keyLifeSpan, logger);
	}

	public DictTryGetValueProxy(IDictTryGetValue<TKey, TValue> target, Action updateTarget, TimeSpan updateTimeout, TimeSpan keyLifeSpan, ILogger logger)
	{
		_target = target ?? throw new ArgumentNullException("target");
		_updateTarget = updateTarget ?? throw new ArgumentNullException("updateTarget");
		_updateTimeout = updateTimeout;
		_keyLifeSpan = keyLifeSpan;
		_logger = logger ?? throw new ArgumentNullException("logger");
	}

	public bool TryGetValue(TKey key, out TValue value)
	{
		bool flag = TryGetValue(key, out value, "first try");
		if (flag)
		{
			return flag;
		}
		DateTime utcNow = DateTime.UtcNow;
		if (UpdatePerfomedTooSoon(utcNow))
		{
			return false;
		}
		if (UpdatePerfomedForThisKeyTooSoon(key, utcNow))
		{
			return false;
		}
		UpdateInternalState(key, utcNow);
		UpdateTarget();
		RemoveExpiredKeys(utcNow);
		return TryGetValue(key, out value, "second try");
	}

	private bool TryGetValue(TKey key, out TValue value, string number)
	{
		bool num = _target.TryGetValue(key, out value);
		if (!num)
		{
			LoggerExtensions.LogInformation(_logger, "TryGetValue failed on " + number, Array.Empty<object>());
		}
		return num;
	}

	private bool UpdatePerfomedTooSoon(DateTime now)
	{
		_locker.EnterReadLock();
		TimeSpan timeSpan;
		bool flag;
		try
		{
			timeSpan = now - _lastUpdateAt;
			flag = timeSpan < _updateTimeout;
		}
		finally
		{
			_locker.ExitReadLock();
		}
		if (flag)
		{
			LoggerExtensions.LogInformation(_logger, "UpdatePerfomedTooSoon. Passed only seconds of: " + timeSpan.TotalSeconds, Array.Empty<object>());
		}
		return flag;
	}

	private bool UpdatePerfomedForThisKeyTooSoon(TKey key, DateTime now)
	{
		int num;
		if (_keyLastCheckedAt.TryGetValue(key, out var value))
		{
			num = ((now - value < _keyLifeSpan) ? 1 : 0);
			if (num != 0)
			{
				LoggerExtensions.LogInformation(_logger, $"UpdatePerfomedForThisKeyTooSoon. Key: {key}", Array.Empty<object>());
			}
		}
		else
		{
			num = 0;
		}
		return (byte)num != 0;
	}

	private void UpdateInternalState(TKey key, DateTime now)
	{
		_keyLastCheckedAt.AddOrUpdate(key, now, (TKey k, DateTime v) => now);
		_locker.EnterWriteLock();
		try
		{
			_lastUpdateAt = now;
		}
		finally
		{
			_locker.ExitWriteLock();
		}
	}

	private void UpdateTarget()
	{
		_updateTarget();
		LoggerExtensions.LogInformation(_logger, "Target Updated", Array.Empty<object>());
	}

	private void RemoveExpiredKeys(DateTime now)
	{
		(from kvp in _keyLastCheckedAt
			where now - kvp.Value >= _keyLifeSpan
			select kvp.Key).ToList().ForEach(delegate(TKey key)
		{
			_keyLastCheckedAt.TryRemove(key, out var _);
		});
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			_locker?.Dispose();
		}
	}
}
