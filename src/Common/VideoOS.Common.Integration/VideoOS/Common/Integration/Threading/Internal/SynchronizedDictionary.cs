using System.Collections.Generic;

namespace VideoOS.Common.Integration.Threading.Internal;

public class SynchronizedDictionary<TKey, TValue>
{
	private readonly Dictionary<TKey, TValue> _dictionary;

	private readonly object _lock;

	public int Count => _dictionary.Count;

	public object SyncRoot => _lock;

	public TValue this[TKey key]
	{
		get
		{
			lock (_lock)
			{
				return _dictionary[key];
			}
		}
		set
		{
			lock (_lock)
			{
				_dictionary[key] = value;
			}
		}
	}

	public Dictionary<TKey, TValue>.KeyCollection Keys
	{
		get
		{
			lock (_lock)
			{
				return _dictionary.Keys;
			}
		}
	}

	public Dictionary<TKey, TValue>.ValueCollection Values
	{
		get
		{
			lock (_lock)
			{
				return _dictionary.Values;
			}
		}
	}

	public SynchronizedDictionary()
	{
		_lock = new object();
		_dictionary = new Dictionary<TKey, TValue>();
	}

	public bool Contains(TKey key)
	{
		lock (_lock)
		{
			return _dictionary.ContainsKey(key);
		}
	}

	public void Remove(TKey key)
	{
		lock (_lock)
		{
			_dictionary.Remove(key);
		}
	}

	public void Clear()
	{
		lock (_lock)
		{
			_dictionary.Clear();
		}
	}
}
