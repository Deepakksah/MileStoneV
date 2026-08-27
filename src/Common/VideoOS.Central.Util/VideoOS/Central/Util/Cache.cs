using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoOS.Central.Util;

public class Cache<Tkey, TValue>
{
	private sealed class CacheValue
	{
		public TValue Value { get; set; }

		public DateTime LastTouchedUtc { get; set; }
	}

	private readonly Dictionary<Tkey, CacheValue> _values = new Dictionary<Tkey, CacheValue>();

	private readonly object _locker = new object();

	private DateTime _nextExpirationCheckUtc;

	private readonly TimeSpan _expirationCheckInterval;

	private readonly TimeSpan _expirationInterval;

	private readonly Func<Tkey, TValue> _factory;

	private readonly Action<TValue> _demolisher;

	private readonly Func<DateTime> _utcNowProvider;

	public Cache(Func<Tkey, TValue> factory, Action<TValue> demolisher, TimeSpan expirationInterval, TimeSpan expirationCheckInterval)
		: this(factory, demolisher, expirationInterval, expirationCheckInterval, (Func<DateTime>)(() => DateTime.UtcNow))
	{
	}

	public Cache(Func<Tkey, TValue> factory, Action<TValue> demolisher, TimeSpan expirationInterval, TimeSpan expirationCheckInterval, Func<DateTime> utcNowProvider)
	{
		_factory = factory ?? throw new ArgumentNullException("factory");
		_demolisher = demolisher;
		_utcNowProvider = utcNowProvider ?? throw new ArgumentNullException("utcNowProvider");
		_expirationInterval = expirationInterval;
		_expirationCheckInterval = expirationCheckInterval;
		_nextExpirationCheckUtc = _utcNowProvider() + _expirationCheckInterval;
	}

	public TValue Get(Tkey key)
	{
		TValue val = default(TValue);
		CleanUpExpired();
		lock (_locker)
		{
			if (!_values.TryGetValue(key, out var value))
			{
				value = new CacheValue();
				value.Value = _factory(key);
				_values.Add(key, value);
			}
			value.LastTouchedUtc = _utcNowProvider();
			return value.Value;
		}
	}

	public void CleanUpExpired()
	{
		DateTime now = _utcNowProvider();
		if (!(now > _nextExpirationCheckUtc))
		{
			return;
		}
		lock (_locker)
		{
			if (!(now > _nextExpirationCheckUtc))
			{
				return;
			}
			_nextExpirationCheckUtc = now + _expirationCheckInterval;
			List<KeyValuePair<Tkey, CacheValue>> expired = _values.Where((KeyValuePair<Tkey, CacheValue> kvp) => now > kvp.Value.LastTouchedUtc + _expirationInterval).ToList();
			expired.ForEach(delegate(KeyValuePair<Tkey, CacheValue> ex)
			{
				_values.Remove(ex.Key);
			});
			if (_demolisher == null)
			{
				return;
			}
			Task.Factory.StartNew(delegate
			{
				expired.ForEach(delegate(KeyValuePair<Tkey, CacheValue> ex)
				{
					_demolisher(ex.Value.Value);
				});
			});
		}
	}
}
