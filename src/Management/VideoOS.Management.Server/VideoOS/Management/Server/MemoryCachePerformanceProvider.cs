using VideoOS.Common.Integration;

namespace VideoOS.Management.Server;

public class MemoryCachePerformanceProvider : IMemoryCachePerformanceProvider
{
	private string _cacheName;

	public MemoryCachePerformanceProvider(string cacheName)
	{
		_cacheName = cacheName;
	}

	public void NotifyCacheInvalidated()
	{
		GeminiPerformanceCounterManager.GetPerformanceCounter(PerformanceCounterHelper.MemoryCacheInvalidationsSec, _cacheName).Increment();
		GeminiPerformanceCounterManager.GetPerformanceCounter(PerformanceCounterHelper.MemoryCacheTotalInvalidations, _cacheName).Increment();
	}

	public void NotifyCacheLookupFailed()
	{
		GeminiPerformanceCounterManager.GetPerformanceCounter(PerformanceCounterHelper.MemoryCacheMissesSec, _cacheName).Increment();
		GeminiPerformanceCounterManager.GetPerformanceCounter(PerformanceCounterHelper.MemoryCacheTotalMisses, _cacheName).Increment();
	}

	public void NotifyCacheLookupSucceeded()
	{
		GeminiPerformanceCounterManager.GetPerformanceCounter(PerformanceCounterHelper.MemoryCacheHitsSec, _cacheName).Increment();
		GeminiPerformanceCounterManager.GetPerformanceCounter(PerformanceCounterHelper.MemoryCacheTotalHits, _cacheName).Increment();
	}
}
