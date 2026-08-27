using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using VideoOS.Reporting.Web.Plugin.Base;
using VideoOS.Reporting.Web.Plugin.Settings;

namespace VideoOS.Reporting.Web.Plugin.Business;

public class LiveDataCache<TKey, TValue>
{
	public delegate void QueryCounterValue(TKey key, IServiceLogger logger, params string[] args);

	private Dictionary<TKey, CacheEntry<TValue>> _liveDataCache = new Dictionary<TKey, CacheEntry<TValue>>();

	public CacheEntry<TValue> GetValue(TKey key, QueryCounterValue queryCounterValue, IServiceLogger serviceLogger, params string[] queryArgs)
	{
		CacheEntry<TValue> result;
		lock (_liveDataCache)
		{
			CacheEntry<TValue> value = null;
			if (_liveDataCache.TryGetValue(key, out value))
			{
				result = value.Copy(value.Entry);
				if (!value.PendingUpdate && DateTime.UtcNow - value.Timestamp > TimeSpan.FromSeconds(ConfigSettings.LiveDataCacheEntryLifetimeInSec))
				{
					value.PendingUpdate = true;
					Task.Factory.StartNew(delegate
					{
						queryCounterValue(key, serviceLogger, queryArgs);
					}, "QueryPerformanceCounter task", TaskCreationOptions.LongRunning);
				}
			}
			else
			{
				serviceLogger.LogToServerLog(string.Format(CultureInfo.InvariantCulture, "LiveDataCache, no cache yet for: '{0}'", key), LogType.Information);
				value = new CacheEntry<TValue>();
				value.PendingUpdate = true;
				_liveDataCache[key] = value;
				result = value.Copy(value.Entry);
				Task.Factory.StartNew(delegate
				{
					queryCounterValue(key, serviceLogger, queryArgs);
				}, "QueryPerformanceCounter task", TaskCreationOptions.LongRunning);
			}
		}
		return result;
	}

	public CacheEntry<TValue> GetValue(TKey key, QueryCounterValue queryCounterValue, Func<Guid, TValue> databaseAccess, Guid recorderId, IServiceLogger serviceLogger, params string[] queryArgs)
	{
		CacheEntry<TValue> result;
		lock (_liveDataCache)
		{
			CacheEntry<TValue> value = null;
			if (_liveDataCache.TryGetValue(key, out value))
			{
				result = value.Copy(value.Entry);
				if (!value.PendingUpdate && DateTime.UtcNow - value.Timestamp > TimeSpan.FromSeconds(ConfigSettings.LiveDataCacheEntryLifetimeInSec))
				{
					value.PendingUpdate = true;
					Task.Factory.StartNew(delegate
					{
						queryCounterValue(key, serviceLogger, queryArgs);
					}, "QueryPerformanceCounter task", TaskCreationOptions.LongRunning);
				}
			}
			else
			{
				serviceLogger.LogToServerLog(string.Format(CultureInfo.InvariantCulture, "LiveDataCache, no cache yet for: '{0}'", key), LogType.Information);
				value = new CacheEntry<TValue>(databaseAccess(recorderId));
				value.PendingUpdate = true;
				_liveDataCache[key] = value;
				result = value.Copy(value.Entry);
				Task.Factory.StartNew(delegate
				{
					queryCounterValue(key, serviceLogger, queryArgs);
				}, "QueryPerformanceCounter task", TaskCreationOptions.LongRunning);
			}
		}
		return result;
	}

	public void UpdateCache(TKey key, TValue value)
	{
		lock (_liveDataCache)
		{
			PurgeCache();
			_liveDataCache[key] = new CacheEntry<TValue>(value);
		}
	}

	public void ResetPendingUpdate(TKey key)
	{
		lock (_liveDataCache)
		{
			CacheEntry<TValue> value = null;
			if (_liveDataCache.TryGetValue(key, out value))
			{
				value.PendingUpdate = false;
				value.Error = true;
				value.UpdateTimestamp();
			}
			PurgeCache();
		}
	}

	private void PurgeCache()
	{
		List<TKey> list = new List<TKey>();
		foreach (TKey key in _liveDataCache.Keys)
		{
			if (DateTime.UtcNow - _liveDataCache[key].Timestamp > TimeSpan.FromSeconds(ConfigSettings.LiveDataCacheEntryLifetimeInSec * 10))
			{
				list.Add(key);
			}
		}
		foreach (TKey item in list)
		{
			_liveDataCache.Remove(item);
		}
	}
}
