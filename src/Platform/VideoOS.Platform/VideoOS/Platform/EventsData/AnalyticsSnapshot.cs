namespace VideoOS.Platform.EventsData;

public class AnalyticsSnapshot
{
	public virtual int TimeOffset { get; set; }

	public virtual uint Width { get; set; }

	public virtual uint Height { get; set; }

	public virtual bool HasOverlay { get; set; }

	public virtual uint SizeInBytes { get; set; }

	public virtual byte[] Image { get; set; }

	public virtual string Path { get; set; }

	public virtual string Source { get; set; }
}
