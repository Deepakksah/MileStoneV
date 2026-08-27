using System;
using VideoOS.UI.Common.WPF.Timeline.InterfaceData;

namespace VideoOS.UI.Common.WPF.Timeline.Cache;

public class CacheEntry : SourceQueryResult
{
	public DateTime FetchTime { get; set; }

	public DateTime OutOfUseTime { get; set; }

	public CacheEntry()
	{
		OutOfUseTime = DateTime.MaxValue;
	}

	public CacheEntry(SourceQueryResult queryResult)
		: base(queryResult)
	{
		OutOfUseTime = DateTime.MaxValue;
		FetchTime = DateTime.UtcNow;
	}

	public CacheEntry(SourceQuery query)
		: base(query)
	{
		FetchTime = DateTime.MaxValue;
	}
}
