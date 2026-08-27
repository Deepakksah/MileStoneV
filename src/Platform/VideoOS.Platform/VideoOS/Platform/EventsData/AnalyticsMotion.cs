namespace VideoOS.Platform.EventsData;

public class AnalyticsMotion
{
	public virtual double Speed { get; set; }

	public virtual string SpeedUnit { get; set; }

	public virtual AnalyticsPolygon Path { get; set; }
}
