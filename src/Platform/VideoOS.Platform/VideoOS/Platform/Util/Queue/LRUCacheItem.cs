using System;

namespace VideoOS.Platform.Util.Queue;

public class LRUCacheItem<K, V>
{
	public readonly K Key;

	public readonly V Value;

	public long TicksAtLastUsage;

	public LRUCacheItem(K k, V v)
	{
		Key = k;
		Value = v;
		TicksAtLastUsage = DateTime.UtcNow.Ticks;
	}
}
