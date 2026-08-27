using System;
using System.Collections.Generic;
using System.Threading;

namespace VideoOS.Common.Integration;

public sealed class GenericMemoryCache<TKey, TValue>
{
	private Dictionary<TKey, GenericMemoryCacheEntry<TKey, TValue>> _items;

	private ReaderWriterLock _readerWriterLock;

	private int _maxEntries;

	private int _timeoutSeconds;

	private GenericMemoryCacheEntry<TKey, TValue> _listHead;

	private GenericMemoryCacheEntry<TKey, TValue> _listTail;

	private IMemoryCachePerformanceProvider _performanceProvider;

	public int Count
	{
		get
		{
			try
			{
				_readerWriterLock.AcquireReaderLock(-1);
				return _items.Count;
			}
			finally
			{
				if (_readerWriterLock.IsReaderLockHeld)
				{
					_readerWriterLock.ReleaseReaderLock();
				}
			}
		}
	}

	public GenericMemoryCache(int maxEntries, int timeoutSeconds)
		: this((IMemoryCachePerformanceProvider)new DefaultMemoryCachePerformanceProvider(), maxEntries, timeoutSeconds)
	{
	}

	public GenericMemoryCache(IMemoryCachePerformanceProvider performanceProvider, int maxEntries, int timeoutSeconds)
	{
		_performanceProvider = performanceProvider;
		_maxEntries = maxEntries;
		_timeoutSeconds = timeoutSeconds;
		_items = new Dictionary<TKey, GenericMemoryCacheEntry<TKey, TValue>>(maxEntries);
		_readerWriterLock = new ReaderWriterLock();
	}

	public object Find(TKey key)
	{
		_readerWriterLock.AcquireReaderLock(-1);
		try
		{
			if (_items.ContainsKey(key))
			{
				GenericMemoryCacheEntry<TKey, TValue> prevCacheEntry = GetPrevCacheEntry(key);
				GenericMemoryCacheEntry<TKey, TValue> genericMemoryCacheEntry = ((prevCacheEntry == null) ? _listHead : prevCacheEntry.Next);
				if ((DateTime.Now - genericMemoryCacheEntry.Timestamp).TotalSeconds < (double)_timeoutSeconds)
				{
					_performanceProvider.NotifyCacheLookupSucceeded();
					return genericMemoryCacheEntry.Value;
				}
			}
		}
		finally
		{
			_readerWriterLock.ReleaseLock();
		}
		_performanceProvider.NotifyCacheLookupFailed();
		return null;
	}

	public bool TryGet(TKey key, out TValue value)
	{
		value = default(TValue);
		_readerWriterLock.AcquireReaderLock(-1);
		try
		{
			if (_items.ContainsKey(key))
			{
				GenericMemoryCacheEntry<TKey, TValue> prevCacheEntry = GetPrevCacheEntry(key);
				GenericMemoryCacheEntry<TKey, TValue> genericMemoryCacheEntry = ((prevCacheEntry == null) ? _listHead : prevCacheEntry.Next);
				if ((DateTime.Now - genericMemoryCacheEntry.Timestamp).TotalSeconds < (double)_timeoutSeconds)
				{
					value = genericMemoryCacheEntry.Value;
					_performanceProvider.NotifyCacheLookupSucceeded();
					return true;
				}
			}
		}
		finally
		{
			_readerWriterLock.ReleaseLock();
		}
		_performanceProvider.NotifyCacheLookupFailed();
		return false;
	}

	public void Add(TKey key, TValue value)
	{
		_readerWriterLock.AcquireWriterLock(-1);
		try
		{
			if (_items.ContainsKey(key))
			{
				GenericMemoryCacheEntry<TKey, TValue> prevCacheEntry = GetPrevCacheEntry(key);
				GenericMemoryCacheEntry<TKey, TValue> next = ((prevCacheEntry == null) ? _listHead : prevCacheEntry.Next).Next;
				if (prevCacheEntry != null)
				{
					prevCacheEntry.Next = next;
				}
				else
				{
					_listHead = next;
				}
				if (next != null)
				{
					SetPrevCacheEntry(next.Key, prevCacheEntry);
				}
				else
				{
					_listTail = prevCacheEntry;
				}
			}
			GenericMemoryCacheEntry<TKey, TValue> genericMemoryCacheEntry = new GenericMemoryCacheEntry<TKey, TValue>(key, value);
			if (_listTail != null)
			{
				SetPrevCacheEntry(key, _listTail);
				_listTail.Next = genericMemoryCacheEntry;
				_listTail = genericMemoryCacheEntry;
			}
			else
			{
				SetPrevCacheEntry(key, null);
				_listHead = genericMemoryCacheEntry;
				_listTail = genericMemoryCacheEntry;
			}
			if (_items.Count > _maxEntries && _listHead != null && _listTail != null)
			{
				_items.Remove(_listHead.Key);
				_listHead = _listHead.Next;
				SetPrevCacheEntry(_listHead.Key, null);
			}
		}
		finally
		{
			_readerWriterLock.ReleaseLock();
		}
	}

	public void Clear()
	{
		_readerWriterLock.AcquireWriterLock(-1);
		try
		{
			_listHead = null;
			_listTail = null;
			_items.Clear();
			_performanceProvider.NotifyCacheInvalidated();
		}
		finally
		{
			_readerWriterLock.ReleaseLock();
		}
	}

	private GenericMemoryCacheEntry<TKey, TValue> GetPrevCacheEntry(TKey key)
	{
		return _items[key];
	}

	private void SetPrevCacheEntry(TKey key, GenericMemoryCacheEntry<TKey, TValue> entry)
	{
		_items[key] = entry;
	}
}
