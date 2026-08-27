using System;

namespace VideoOS.Platform.EventsData;

public class AnalyticsRule
{
	public virtual Guid ID { get; set; }

	public virtual string Name { get; set; }

	public virtual string Type { get; set; }

	public virtual AnalyticsPolygon[] Polygons { get; set; }
}
