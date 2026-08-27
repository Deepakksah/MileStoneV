using System;

namespace VideoOS.Platform.EventsData;

public class AnalyticsDataWithoutSnapshots
{
	public virtual string Description { get; set; }

	public virtual DateTime? StartTime { get; set; }

	public virtual DateTime? EndTime { get; set; }

	public virtual string Location { get; set; }

	public virtual uint Count { get; set; }

	public virtual AnalyticsRule[] Rules { get; set; }

	public virtual AnalyticsObject[] Objects { get; set; }

	public virtual string[] References { get; set; }

	public virtual AnalyticsVendor Vendor { get; set; }
}
