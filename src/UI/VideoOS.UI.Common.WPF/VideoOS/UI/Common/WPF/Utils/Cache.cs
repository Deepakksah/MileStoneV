using System;
using System.Collections.Concurrent;
using System.Threading;

namespace VideoOS.UI.Common.WPF.Utils;

public class Cache<TKey, TValue>
{
	private struct KeyAccess(KeyRegistration registration, DateTime currentTime)
	{
		public readonly KeyRegistration Registration = registration;

		public readonly DateTime Timestamp = currentTime;
	}

	private class KeyRegistration
	{
		public readonly TKey Key;

		public readonly TValue Value;

		private long _accessCount;

		public KeyRegistration(TKey key, TValue value)
		{
			Key = key;
			Value = value;
			_accessCount = 0L;
		}

		public void AddReference()
		{
			Interlocked.Increment(ref _accessCount);
		}

		public bool RemoveReference()
		{
			return Interlocked.Decrement(ref _accessCount) == 0;
		}
	}

	private readonly Func<DateTime> _provideCurrentTime;

	private readonly TimeSpan _inactivitySpan;

	private readonly ConcurrentDictionary<TKey, KeyRegistration> _dictionary = new ConcurrentDictionary<TKey, KeyRegistration>();

	private readonly ConcurrentQueue<KeyAccess> _accessQueue = new ConcurrentQueue<KeyAccess>();

	private long _cleanupLock;

	public Cache(TimeSpan inactivitySpan, Func<DateTime> provideCurrentTime)
	{
		_inactivitySpan = inactivitySpan;
		_provideCurrentTime = provideCurrentTime;
	}

	public Cache(TimeSpan inactivitySpan)
		: this(inactivitySpan, (Func<DateTime>)(() => DateTime.UtcNow))
	{
	}

	public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
	{
		KeyRegistration orAdd = _dictionary.GetOrAdd(key, (TKey k) => new KeyRegistration(key, valueFactory(key)));
		_accessQueue.Enqueue(new KeyAccess(orAdd, _provideCurrentTime()));
		orAdd.AddReference();
		CleanupOldRegistrations();
		return orAdd.Value;
	}

	private void CleanupOldRegistrations()
	{
		if (Interlocked.Exchange(ref _cleanupLock, 1L) != 0L)
		{
			return;
		}
		KeyAccess result;
		while (_accessQueue.TryPeek(out result) && !(result.Timestamp + _inactivitySpan > _provideCurrentTime()))
		{
			_accessQueue.TryDequeue(out result);
			if (result.Registration.RemoveReference())
			{
				_dictionary.TryRemove(result.Registration.Key, out var _);
			}
		}
		Interlocked.Exchange(ref _cleanupLock, 0L);
	}
}
