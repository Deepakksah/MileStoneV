using System;
using System.Collections.Generic;

namespace VideoOS.Platform.Client;

public class TimelineSequencesEventArgs : EventArgs
{
	public IEnumerable<TimelineSourceQueryResult> Results { get; private set; }

	public TimelineSequencesEventArgs(IEnumerable<TimelineSourceQueryResult> results)
	{
		Results = results;
	}

	public TimelineSequencesEventArgs(TimelineSourceQueryResult result)
	{
		Results = new List<TimelineSourceQueryResult> { result };
	}
}
