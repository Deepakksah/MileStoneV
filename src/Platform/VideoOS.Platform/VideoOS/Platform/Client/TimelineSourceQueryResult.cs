using System.Collections.Generic;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Client;

public class TimelineSourceQueryResult
{
	public TimeInterval TimeInterval { get; private set; }

	public IEnumerable<TimelineDataArea> Sequences { get; set; }

	public TimelineSourceQueryResult(TimeInterval interval)
	{
		TimeInterval = interval;
		Sequences = new List<TimelineDataArea>();
	}
}
