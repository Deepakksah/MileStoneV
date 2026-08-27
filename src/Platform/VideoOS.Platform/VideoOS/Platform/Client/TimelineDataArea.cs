using System;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Client;

public class TimelineDataArea
{
	public object Id { get; private set; }

	public TimeInterval Interval { get; set; }

	public TimelineDataArea()
	{
		Interval = new TimeInterval(DateTime.UtcNow, DateTime.UtcNow);
	}

	public TimelineDataArea(TimeInterval interval)
		: this()
	{
		Interval = interval;
	}

	public TimelineDataArea(object id, TimeInterval interval)
	{
		Id = id;
		Interval = interval;
	}
}
