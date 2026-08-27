using System;

namespace VideoOS.UI.Common.WPF.Timeline;

public class TimelineCacheSettings
{
	public TimeSpan BlockLength { get; }

	public double PrefetchRatio { get; }

	public double KeepRatio { get; }

	public TimeSpan LiveViewsRefreshSpan { get; }

	public TimeSpan DatabaseStartRefreshSpan { get; }

	public TimeSpan UnusedDataKeepSpan { get; }

	public TimelineCacheSettings(TimeSpan? blockLength = null, double prefetchRatio = 0.1, double keepRatio = 1.0, TimeSpan? liveViewsRefreshSpan = null, TimeSpan? databaseStartRefreshSpan = null, TimeSpan? unusedDataKeepSpan = null)
	{
		BlockLength = blockLength.GetValueOrDefault(TimeSpan.FromHours(1.0));
		PrefetchRatio = prefetchRatio;
		KeepRatio = keepRatio;
		LiveViewsRefreshSpan = liveViewsRefreshSpan.GetValueOrDefault(TimeSpan.FromSeconds(30.0));
		DatabaseStartRefreshSpan = databaseStartRefreshSpan.GetValueOrDefault(TimeSpan.FromMinutes(1.0));
		UnusedDataKeepSpan = unusedDataKeepSpan.GetValueOrDefault(TimeSpan.FromMinutes(1.0));
	}
}
