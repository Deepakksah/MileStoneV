using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Management.VmoClient;

public class SiteTokenCacheStore : ISiteTokenCacheStore, IDisposable
{
	private readonly object locker = new object();

	private bool _disposed;

	private static readonly Lazy<SiteTokenCacheStore> _instance = new Lazy<SiteTokenCacheStore>(() => new SiteTokenCacheStore());

	private List<SiteTokenCacheStoreItem> _tokenCacheStore = new List<SiteTokenCacheStoreItem>();

	public static SiteTokenCacheStore Instance => _instance.Value;

	public bool Contains(Guid siteId)
	{
		lock (locker)
		{
			return Get(siteId) != null;
		}
	}

	public bool Contains(Uri address)
	{
		lock (locker)
		{
			return FindItemUsingConfiguredAddress(address) != null;
		}
	}

	public bool Contains(IEnumerable<Uri> addresses)
	{
		if (addresses == null)
		{
			return false;
		}
		lock (locker)
		{
			foreach (Uri address in addresses)
			{
				if (FindItemUsingConfiguredAddress(address) != null)
				{
					return true;
				}
			}
			return false;
		}
	}

	public SiteTokenCacheStoreItem Get(Guid siteId)
	{
		lock (locker)
		{
			return _tokenCacheStore.FirstOrDefault((SiteTokenCacheStoreItem x) => x.SiteId.Equals(siteId));
		}
	}

	public SiteTokenCacheStoreItem Get(Uri address)
	{
		lock (locker)
		{
			return FindItemUsingConfiguredAddress(address);
		}
	}

	public SiteTokenCacheStoreItem Get(IEnumerable<Uri> addresses)
	{
		if (addresses == null)
		{
			return null;
		}
		lock (locker)
		{
			foreach (Uri address in addresses)
			{
				SiteTokenCacheStoreItem siteTokenCacheStoreItem = FindItemUsingConfiguredAddress(address);
				if (siteTokenCacheStoreItem != null)
				{
					return siteTokenCacheStoreItem;
				}
			}
			return null;
		}
	}

	public SiteTokenCacheStoreItem GetHome()
	{
		lock (locker)
		{
			return _tokenCacheStore.FirstOrDefault((SiteTokenCacheStoreItem x) => x.HomeSite);
		}
	}

	public bool Add(SiteTokenCacheStoreItem item)
	{
		lock (locker)
		{
			if (!Contains(item.SiteId))
			{
				_tokenCacheStore.Add(item);
				return true;
			}
			return false;
		}
	}

	public bool Remove(Guid siteId)
	{
		lock (locker)
		{
			SiteTokenCacheStoreItem siteTokenCacheStoreItem = Get(siteId);
			if (siteTokenCacheStoreItem != null && !siteTokenCacheStoreItem.HomeSite)
			{
				_tokenCacheStore.Remove(siteTokenCacheStoreItem);
				return true;
			}
			return false;
		}
	}

	public int Count()
	{
		return _tokenCacheStore.Count();
	}

	private SiteTokenCacheStoreItem FindItemUsingConfiguredAddress(Uri address)
	{
		if (address == null)
		{
			return null;
		}
		foreach (SiteTokenCacheStoreItem item in _tokenCacheStore)
		{
			if (!item.ConnectedAddress.Equals(address))
			{
				IEnumerable<Uri> addresses = item.Addresses;
				if (addresses == null || !addresses.Contains(address))
				{
					continue;
				}
			}
			return item;
		}
		return null;
	}

	public void Dispose()
	{
		if (_disposed)
		{
			return;
		}
		lock (locker)
		{
			if (_tokenCacheStore.Count > 0)
			{
				_tokenCacheStore.ForEach(delegate(SiteTokenCacheStoreItem tc)
				{
					tc.TokenCache.Dispose();
				});
				_tokenCacheStore.Clear();
			}
		}
		_disposed = true;
		GC.SuppressFinalize(this);
	}
}
