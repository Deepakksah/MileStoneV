using System;
using System.Collections.Generic;
using System.Xml;

namespace VideoOS.Platform.SDK.Config;

public static class SiteInfoCache
{
	private static TimeSpan _cacheLifeTime = TimeSpan.FromMinutes(2.0);

	private static List<SiteInfoCacheElement> CacheElements = new List<SiteInfoCacheElement>();

	private static object _lock = new object();

	public static void Store(Uri uri, string userName, bool basic, bool unauthorized, XmlDocument xmlDocument)
	{
		SiteInfoCacheElement siteInfoCacheElement = new SiteInfoCacheElement
		{
			ServerUri = uri,
			UserName = userName,
			Basic = basic,
			Unauthorized = unauthorized,
			XmlDocument = xmlDocument,
			LoadedDateTime = DateTime.UtcNow
		};
		lock (_lock)
		{
			FlushOldElements();
			foreach (SiteInfoCacheElement cacheElement in CacheElements)
			{
				if (cacheElement.ServerUri.Equals(uri) && cacheElement.UserName.Equals(userName) && cacheElement.Basic == basic)
				{
					CacheElements.Remove(cacheElement);
					break;
				}
			}
			if (siteInfoCacheElement.XmlDocument != null)
			{
				CacheElements.Add(siteInfoCacheElement);
			}
		}
	}

	public static SiteInfoCacheElement Lookup(Uri uri, string userName, bool basic)
	{
		lock (_lock)
		{
			FlushOldElements();
			foreach (SiteInfoCacheElement cacheElement in CacheElements)
			{
				if (cacheElement.ServerUri.Equals(uri) && cacheElement.UserName.Equals(userName) && cacheElement.Basic == basic)
				{
					return cacheElement;
				}
			}
		}
		return null;
	}

	private static void FlushOldElements()
	{
		lock (_lock)
		{
			List<SiteInfoCacheElement> list = new List<SiteInfoCacheElement>();
			foreach (SiteInfoCacheElement cacheElement in CacheElements)
			{
				if (cacheElement.LoadedDateTime + _cacheLifeTime < DateTime.UtcNow)
				{
					list.Add(cacheElement);
				}
			}
			foreach (SiteInfoCacheElement item in list)
			{
				CacheElements.Remove(item);
			}
		}
	}
}
