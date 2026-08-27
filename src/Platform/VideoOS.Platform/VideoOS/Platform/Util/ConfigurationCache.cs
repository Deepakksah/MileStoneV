using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace VideoOS.Platform.Util;

public class ConfigurationCache
{
	private ServerId _serverId;

	private object _lockObject = new object();

	private DateTime _lastFlushCacheCheckDateTime = DateTime.MinValue;

	private TimeSpan _flushCheckInterval = TimeSpan.FromMinutes(1.0);

	private TimeSpan _cacheLifeTime = TimeSpan.FromMinutes(15.0);

	private Hashtable _configurations = new Hashtable();

	private static Guid _licenseId = new Guid("E145597A-ECAF-40A1-99E6-14BB9D45A09E");

	public ServerId ServerId => _serverId;

	public TimeSpan FlushCheckInterval
	{
		get
		{
			return _flushCheckInterval;
		}
		set
		{
			_flushCheckInterval = value;
		}
	}

	public TimeSpan CacheLifeTime
	{
		get
		{
			return _cacheLifeTime;
		}
		set
		{
			_cacheLifeTime = value;
		}
	}

	public ConfigurationCache(ServerId serverId)
	{
		if (serverId == null)
		{
			throw new ArgumentNullException("serverId");
		}
		_serverId = serverId;
	}

	public Item GetItem(Guid kind, Guid objectId)
	{
		lock (_lockObject)
		{
			Trace("GetItem (In)", kind, objectId);
			CheckToFlushCacheElements();
			if (_configurations[kind] is KindConfiguration kindConfiguration)
			{
				Item item = kindConfiguration.GetItem(objectId);
				if (item != null)
				{
					Trace("GetItem (out)", kind, "Found: " + item.Name);
					return item;
				}
			}
			Trace("GetItem (out)", kind, "Not Found");
			return null;
		}
	}

	public List<Item> GetItems(Guid kind)
	{
		lock (_lockObject)
		{
			Trace("GetItems (In)", kind, "");
			CheckToFlushCacheElements();
			if (_configurations[kind] is KindConfiguration kindConfiguration)
			{
				List<Item> items = kindConfiguration.GetItems();
				if (items != null)
				{
					Trace("GetItems (Out)", kind, "Return items: " + items.Count.ToString(CultureInfo.InvariantCulture));
					return items;
				}
			}
			Trace("GetItems (Out)", kind, "None found");
			return null;
		}
	}

	public List<Item> GetChildItems(Guid kind, Guid parentId)
	{
		lock (_lockObject)
		{
			Trace("GetChildItems (In)", kind, parentId);
			CheckToFlushCacheElements();
			if (_configurations[kind] is KindConfiguration kindConfiguration)
			{
				List<Item> items = kindConfiguration.GetItems(parentId);
				if (items != null)
				{
					Trace("GetChildItems (Out)", kind, "Result count: " + items.Count.ToString(CultureInfo.InvariantCulture));
					return items;
				}
			}
			Trace("GetChildItems (Out)", kind, "None found");
			return null;
		}
	}

	public void Store(Item item)
	{
		lock (_lockObject)
		{
			if (!(item.FQID.Kind == _licenseId))
			{
				Trace("Store", item.FQID.Kind, item.Name);
				KindConfiguration kindConfiguration = _configurations[item.FQID.Kind] as KindConfiguration;
				if (kindConfiguration == null)
				{
					kindConfiguration = new KindConfiguration
					{
						Kind = item.FQID.Kind
					};
					_configurations[item.FQID.Kind] = kindConfiguration;
				}
				kindConfiguration.StoreItem(item);
			}
		}
	}

	public void StoreList(Guid kind, List<Item> items, Guid parentId, bool allItems)
	{
		lock (_lockObject)
		{
			if (!(kind == _licenseId))
			{
				string[] obj = new string[6]
				{
					"Count: ",
					items.Count.ToString(CultureInfo.InvariantCulture),
					", parentId:",
					null,
					null,
					null
				};
				Guid guid = parentId;
				obj[3] = guid.ToString();
				obj[4] = ", All:";
				obj[5] = allItems.ToString();
				Trace("StoreList", kind, string.Concat(obj));
				KindConfiguration kindConfiguration = _configurations[kind] as KindConfiguration;
				if (kindConfiguration == null)
				{
					kindConfiguration = new KindConfiguration
					{
						Kind = kind
					};
					_configurations[kind] = kindConfiguration;
				}
				kindConfiguration.StoreItems(parentId, items, allItems);
			}
		}
	}

	public void Flush(Guid kind)
	{
		if (kind == Guid.Empty)
		{
			FlushAll();
			return;
		}
		lock (_lockObject)
		{
			Trace("Flush", kind, "");
			_configurations.Remove(kind);
		}
	}

	public void FlushAll()
	{
		lock (_lockObject)
		{
			Trace("Flush", Guid.Empty, "Entire cache");
			_configurations.Clear();
		}
	}

	public DateTime GetLastModified(Guid kind)
	{
		lock (_lockObject)
		{
			DateTime result = ((_configurations[kind] is KindConfiguration kindConfiguration) ? kindConfiguration.LastModified : DateTime.MinValue);
			Trace("GetLastModified", kind, result.ToString(CultureInfo.InvariantCulture));
			return result;
		}
	}

	private void CheckToFlushCacheElements()
	{
		if (!(_lastFlushCacheCheckDateTime + _flushCheckInterval < DateTime.UtcNow))
		{
			return;
		}
		Trace("CheckToFlushCacheElements (Start)");
		lock (_lockObject)
		{
			DateTime dateTime = DateTime.UtcNow - _cacheLifeTime;
			List<Guid> list = new List<Guid>();
			foreach (object value in _configurations.Values)
			{
				if (((KindConfiguration)value).LastAccessed < dateTime)
				{
					list.Add(((KindConfiguration)value).Kind);
				}
			}
			foreach (Guid item in list)
			{
				Trace("CheckToFlushCacheElements ", item, "Flushed");
				_configurations.Remove(item);
			}
			_lastFlushCacheCheckDateTime = DateTime.UtcNow;
		}
		Trace("CheckToFlushCacheElements (End)");
	}

	private static void Trace(string where, Guid kind, string function)
	{
		if (Configuration.TraceItemConfigurationCache)
		{
			EnvironmentManager instance = EnvironmentManager.Instance;
			string[] obj = new string[6] { where, "  ", function, " (", null, null };
			Guid guid = kind;
			obj[4] = guid.ToString();
			obj[5] = ") ";
			instance.Log(error: false, "ConfigurationCache", string.Concat(obj));
		}
	}

	private static void Trace(string where, Guid kind, Guid id)
	{
		if (Configuration.TraceItemConfigurationCache)
		{
			EnvironmentManager instance = EnvironmentManager.Instance;
			string[] obj = new string[6] { where, "  Id:", null, null, null, null };
			Guid guid = id;
			obj[2] = guid.ToString();
			obj[3] = " (";
			guid = kind;
			obj[4] = guid.ToString();
			obj[5] = ") ";
			instance.Log(error: false, "ConfigurationCache", string.Concat(obj));
		}
	}

	private static void Trace(string where)
	{
		if (Configuration.TraceItemConfigurationCache)
		{
			EnvironmentManager.Instance.Log(error: false, "ConfigurationCache", where);
		}
	}
}
