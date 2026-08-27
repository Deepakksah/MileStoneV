using System;

namespace VideoOS.Common.Integration.Command;

public class TaskProgressResponse : TaskResponseBase
{
	public DateTime StartTime { get; private set; }

	public int PercentComplete { get; private set; }

	public TaskProgressResponse(DateTime startTime, int percentComplete)
	{
		StartTime = startTime;
		PercentComplete = percentComplete;
	}
}
