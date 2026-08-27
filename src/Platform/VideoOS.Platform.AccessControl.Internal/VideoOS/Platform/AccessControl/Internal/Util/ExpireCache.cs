using System;
using System.Collections.Generic;

namespace VideoOS.Platform.AccessControl.Internal.Util;

public class ExpireCache<TKey, TItem>
{
	private sealed class CacheItem<TCachedItem>
	{
		public TCachedItem Item { get; set; }

		public DateTime CachedTimeStamp { get; set; }
	}

	public class ItemInfo
	{
		public TKey Key { get; set; }

		public TItem Item { get; set; }
	}

	private readonly TimeSpan _expireTimeSpan;

	private readonly TimeSpan _expireNullTimeSpan;

	private readonly int _cacheSize;

	private readonly Dictionary<TKey, CacheItem<TItem>> _cache = new Dictionary<TKey, CacheItem<TItem>>();

	private readonly List<TKey> _keyList = new List<TKey>();

	public ExpireCache(TimeSpan expireTimeSpan, int cacheSize)
		: this(expireTimeSpan, expireTimeSpan, cacheSize)
	{
	}

	public ExpireCache(TimeSpan expireTimeSpan, TimeSpan expireNullTimeSpan, int cacheSize)
	{
		_expireTimeSpan = ((expireTimeSpan > TimeSpan.Zero) ? expireTimeSpan : TimeSpan.MaxValue);
		_expireNullTimeSpan = ((expireNullTimeSpan > TimeSpan.Zero) ? expireNullTimeSpan : TimeSpan.MaxValue);
		_cacheSize = ((cacheSize > 0) ? cacheSize : int.MaxValue);
	}

	public bool HasItem(TKey key)
	{
		lock (_cache)
		{
			return _cache.ContainsKey(key);
		}
	}

	public void SetItem(TKey key, TItem item)
	{
		SetItem(key, item, out var _, out var _);
	}

	public void SetItem(TKey key, TItem item, out TItem oldItem)
	{
		SetItem(key, item, out oldItem, out var _);
	}

	public void SetItem(TKey key, TItem item, out bool itemInCacheUpdated)
	{
		SetItem(key, item, out var _, out itemInCacheUpdated);
	}

	public void SetItem(TKey key, TItem item, out TItem oldItem, out bool itemInCacheUpdated)
	{
		lock (_cache)
		{
			oldItem = default(TItem);
			itemInCacheUpdated = false;
			if (_cache.ContainsKey(key))
			{
				_keyList.Remove(key);
				oldItem = _cache[key].Item;
				itemInCacheUpdated = true;
			}
			_cache[key] = new CacheItem<TItem>
			{
				Item = item,
				CachedTimeStamp = DateTime.UtcNow
			};
			_keyList.Add(key);
			EnsureCacheSize();
		}
	}

	public TItem GetItem(TKey key)
	{
		bool itemExistsInCache;
		return GetItem(key, out itemExistsInCache);
	}

	public TItem GetItem(TKey key, out bool itemExistsInCache)
	{
		lock (_cache)
		{
			itemExistsInCache = false;
			if (_cache.TryGetValue(key, out var value))
			{
				if (value.Item != null && (_expireTimeSpan == TimeSpan.MaxValue || value.CachedTimeStamp + _expireTimeSpan > DateTime.UtcNow))
				{
					itemExistsInCache = true;
					return value.Item;
				}
				if (value.Item == null && (_expireNullTimeSpan == TimeSpan.MaxValue || value.CachedTimeStamp + _expireNullTimeSpan > DateTime.UtcNow))
				{
					itemExistsInCache = true;
					return value.Item;
				}
				RemoveItem(key);
			}
			return default(TItem);
		}
	}

	public List<ItemInfo> GetAllItems()
	{
		lock (_cache)
		{
			List<ItemInfo> list = new List<ItemInfo>();
			foreach (TKey key in _keyList)
			{
				list.Add(new ItemInfo
				{
					Key = key,
					Item = _cache[key].Item
				});
			}
			return list;
		}
	}

	public void RemoveAllItems()
	{
		lock (_cache)
		{
			_cache.Clear();
			_keyList.Clear();
		}
	}

	public void RemoveItem(TKey key)
	{
		lock (_cache)
		{
			_keyList.Remove(key);
			_cache.Remove(key);
		}
	}

	private void EnsureCacheSize()
	{
		while (_cache.Count > _cacheSize)
		{
			RemoveItem(_keyList[0]);
		}
	}
}
