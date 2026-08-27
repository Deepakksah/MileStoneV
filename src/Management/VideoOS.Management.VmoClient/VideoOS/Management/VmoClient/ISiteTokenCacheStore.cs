using System;
using System.Collections.Generic;

namespace VideoOS.Management.VmoClient;

public interface ISiteTokenCacheStore : IDisposable
{
	bool Contains(Guid siteId);

	bool Contains(Uri address);

	bool Contains(IEnumerable<Uri> addresses);

	SiteTokenCacheStoreItem Get(Guid siteId);

	SiteTokenCacheStoreItem Get(Uri address);

	SiteTokenCacheStoreItem Get(IEnumerable<Uri> addresses);

	SiteTokenCacheStoreItem GetHome();

	bool Add(SiteTokenCacheStoreItem item);

	bool Remove(Guid siteId);

	int Count();
}
