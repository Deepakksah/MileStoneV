using VideoOS.Common.Integration;

namespace VideoOS.Management.Server;

public sealed class PrivateViewGroupTokenCache
{
	private static GenericMemoryCache<string, string> _cache = new GenericMemoryCache<string, string>(new MemoryCachePerformanceProvider("PrivateViewGroupTokenCache"), 50000, int.MaxValue);

	public static int Count => _cache.Count;

	public static string GetPrivateViewGroupToken(string viewGroupId)
	{
		object obj = _cache.Find(viewGroupId);
		if (obj != null)
		{
			return (string)obj;
		}
		return viewGroupId;
	}

	public static void AddToCache(string viewGroupId, string privateViewGroupToken)
	{
		_cache.Add(viewGroupId, privateViewGroupToken);
	}

	public static void Clear()
	{
		_cache.Clear();
	}
}
