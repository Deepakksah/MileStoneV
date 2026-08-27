namespace VideoOS.Platform.EventsData;

public class AnalyticsData : AnalyticsDataWithoutSnapshots
{
	public virtual AnalyticsSnapshot[] Snapshots { get; set; }
}
