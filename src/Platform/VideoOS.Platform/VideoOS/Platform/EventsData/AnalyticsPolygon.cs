namespace VideoOS.Platform.EventsData;

public class AnalyticsPolygon
{
	public virtual bool Closed { get; set; }

	public virtual AnalyticsColor Color { get; set; }

	public virtual AnalyticsColor FillColor { get; set; }

	public virtual AnalyticsPoint[] Points { get; set; }
}
