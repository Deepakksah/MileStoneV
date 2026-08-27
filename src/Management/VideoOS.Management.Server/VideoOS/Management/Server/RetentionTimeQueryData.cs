using System;

namespace VideoOS.Management.Server;

public sealed class RetentionTimeQueryData
{
	public int CounterInstanceId { get; set; }

	public DateTime StartDate { get; set; }

	public int Days { get; set; }

	public int SampleCount { get; set; }
}
