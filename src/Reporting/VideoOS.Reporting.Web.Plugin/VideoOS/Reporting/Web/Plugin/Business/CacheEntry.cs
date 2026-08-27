using System;

namespace VideoOS.Reporting.Web.Plugin.Business;

public class CacheEntry<T>
{
	public DateTime Timestamp { get; private set; }

	public T Entry { get; private set; }

	public bool PendingUpdate { get; set; }

	public bool Empty { get; set; }

	public bool Error { get; set; }

	public CacheEntry()
	{
		Timestamp = DateTime.UtcNow;
		Empty = true;
		Error = false;
	}

	public CacheEntry(T entry)
	{
		Timestamp = DateTime.UtcNow;
		Entry = entry;
		Empty = false;
		Error = false;
	}

	public void UpdateTimestamp()
	{
		Timestamp = DateTime.UtcNow;
	}

	public CacheEntry<T> Copy(T entry)
	{
		return new CacheEntry<T>(entry)
		{
			Timestamp = Timestamp,
			PendingUpdate = PendingUpdate,
			Empty = Empty,
			Error = Error
		};
	}
}
