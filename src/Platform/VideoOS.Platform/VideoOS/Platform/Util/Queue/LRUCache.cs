using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Platform.Util.Queue;

public class LRUCache<K, V> : IDisposable
{
	public const int LifeSpanMonitorPauseMillis = 250;

	private readonly int _capacity;

	private readonly object _cacheLock = new object();

	private readonly Dictionary<K, LinkedListNode<LRUCacheItem<K, V>>> _cacheMap = new Dictionary<K, LinkedListNode<LRUCacheItem<K, V>>>();

	private readonly LinkedList<LRUCacheItem<K, V>> _lruList = new LinkedList<LRUCacheItem<K, V>>();

	private readonly Action<V> _postRemovalAction;

	private readonly TimeSpan? _itemLifeSpan;

	private bool _disposed;

	private readonly ManualResetEventSlim _disposedEvent = new ManualResetEventSlim();

	public int CacheSize
	{
		get
		{
			lock (_cacheLock)
			{
				return _cacheMap.Count;
			}
		}
	}

	public LRUCache(int capacity, TimeSpan? itemLifeSpan = null, Action<V> postRemovalAction = null)
	{
		_capacity = capacity;
		_itemLifeSpan = itemLifeSpan;
		_postRemovalAction = postRemovalAction;
		if (_itemLifeSpan.HasValue)
		{
			Task.Factory.StartNew(MonitorLifeSpans);
		}
	}

	private void MonitorLifeSpans()
	{
		while (true)
		{
			_disposedEvent.Wait(250);
			if (_disposed)
			{
				break;
			}
			lock (_cacheLock)
			{
				long ticksNow = DateTime.UtcNow.Ticks;
				TimeSpan itemLifeSpan = _itemLifeSpan ?? TimeSpan.MaxValue;
				foreach (K item in (from v in _cacheMap.Values
					where TimeSpan.FromTicks(ticksNow - v.Value.TicksAtLastUsage) >= itemLifeSpan
					select v.Value.Key).ToList())
				{
					Remove(item);
				}
			}
		}
	}

	public V Get(K key)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		lock (_cacheLock)
		{
			if (_cacheMap.TryGetValue(key, out var value))
			{
				V value2 = value.Value.Value;
				_lruList.Remove(value);
				_lruList.AddLast(value);
				value.Value.TicksAtLastUsage = DateTime.UtcNow.Ticks;
				return value2;
			}
			return default(V);
		}
	}

	public bool Remove(K key)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		LinkedListNode<LRUCacheItem<K, V>> value = null;
		try
		{
			lock (_cacheLock)
			{
				if (_cacheMap.TryGetValue(key, out value))
				{
					_lruList.Remove(value);
					_cacheMap.Remove(key);
					return true;
				}
				return false;
			}
		}
		finally
		{
			if (value != null)
			{
				_postRemovalAction?.Invoke(value.Value.Value);
			}
		}
	}

	public void Clear()
	{
		Remove((K key) => true);
	}

	public int Remove(Predicate<K> predicate)
	{
		int num = 0;
		lock (_cacheLock)
		{
			List<K> list = new List<K>();
			foreach (K key in _cacheMap.Keys)
			{
				if (predicate(key))
				{
					list.Add(key);
				}
			}
			foreach (K item in list)
			{
				if (Remove(item))
				{
					num++;
				}
			}
			return num;
		}
	}

	public void Add(K key, V val)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		lock (_cacheLock)
		{
			if (_cacheMap.Count >= _capacity)
			{
				LinkedListNode<LRUCacheItem<K, V>> first = _lruList.First;
				Remove(first.Value.Key);
			}
			Remove(key);
			LinkedListNode<LRUCacheItem<K, V>> linkedListNode = new LinkedListNode<LRUCacheItem<K, V>>(new LRUCacheItem<K, V>(key, val));
			_lruList.AddLast(linkedListNode);
			_cacheMap.Add(key, linkedListNode);
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (_disposed || !disposing)
		{
			return;
		}
		lock (_cacheLock)
		{
			if (_disposed)
			{
				return;
			}
			_disposed = true;
		}
		_disposedEvent.Set();
		_disposedEvent.Dispose();
	}
}
