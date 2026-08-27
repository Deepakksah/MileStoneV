namespace VideoOS.Common.Integration;

public interface IMemoryCachePerformanceProvider
{
	void NotifyCacheInvalidated();

	void NotifyCacheLookupFailed();

	void NotifyCacheLookupSucceeded();
}
