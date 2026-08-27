namespace VideoOS.Platform.EventsData;

public class AnalyticsBoundingBox
{
	public virtual double Top { get; set; }

	public virtual double Left { get; set; }

	public virtual double Bottom { get; set; }

	public virtual double Right { get; set; }

	public virtual AnalyticsColor Color { get; set; }
}
