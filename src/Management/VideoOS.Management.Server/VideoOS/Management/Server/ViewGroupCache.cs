using VideoOS.Common.Integration;

namespace VideoOS.Management.Server;

public sealed class ViewGroupCache
{
	private static GenericMemoryCache<string, string> _cache = new GenericMemoryCache<string, string>(new MemoryCachePerformanceProvider("ViewGroupCache"), 5000, 300);

	public static string GetRootViewGroupIdFromViewGroupId(string viewGroupId)
	{
		object obj = _cache.Find(viewGroupId);
		if (obj != null)
		{
			return (string)obj;
		}
		return null;
	}

	public static void AddToCache(string viewGroupId, string rootViewGroupId)
	{
		_cache.Add(viewGroupId, rootViewGroupId);
	}

	public static void Clear()
	{
		_cache.Clear();
	}
}
