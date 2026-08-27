using System;

namespace VideoOS.Platform.EventsData;

public class AnalyticsObject
{
	public virtual Guid ID { get; set; }

	public virtual string Name { get; set; }

	public virtual string Type { get; set; }

	public virtual string Description { get; set; }

	public virtual double Confidence { get; set; }

	public virtual string Value { get; set; }

	public virtual bool AlarmTrigger { get; set; }

	public virtual bool Removed { get; set; }

	public virtual string Color { get; set; }

	public virtual double Size { get; set; }

	public virtual string SizeUnit { get; set; }

	public virtual AnalyticsBoundingBox BoundingBox { get; set; }

	public virtual AnalyticsPolygon Polygon { get; set; }

	public virtual AnalyticsMotion Motion { get; set; }

	public virtual byte[] Mask { get; set; }

	public virtual string Data { get; set; }
}
