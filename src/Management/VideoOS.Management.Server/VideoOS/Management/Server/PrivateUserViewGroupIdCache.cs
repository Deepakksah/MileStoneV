using System;
using VideoOS.Common.Integration;

namespace VideoOS.Management.Server;

public sealed class PrivateUserViewGroupIdCache
{
	private static object sync = new object();

	private static volatile PrivateUserViewGroupIdCache instance;

	private GenericMemoryCache<string, string> cache;

	public static PrivateUserViewGroupIdCache Instance
	{
		get
		{
			if (instance != null)
			{
				return instance;
			}
			lock (sync)
			{
				if (instance != null)
				{
					return instance;
				}
				instance = new PrivateUserViewGroupIdCache();
			}
			return instance;
		}
	}

	public PrivateUserViewGroupIdCache()
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds(40.0);
		cache = new GenericMemoryCache<string, string>(new MemoryCachePerformanceProvider("PrivateUserViewGroupIdCache"), 5000, (int)timeSpan.TotalSeconds);
	}

	public string GetPrivateViewGroupId(string userDomainAccountName)
	{
		if (cache.TryGet(userDomainAccountName, out var value))
		{
			return value;
		}
		return null;
	}

	public void AddToCache(string userDomainAccountName, string viewGroupId)
	{
		cache.Add(userDomainAccountName, viewGroupId);
	}

	public void Clear()
	{
		cache.Clear();
	}
}
