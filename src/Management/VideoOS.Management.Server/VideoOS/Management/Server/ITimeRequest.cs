using System;

namespace VideoOS.Management.Server;

public interface ITimeRequest
{
	DateTime StartTime { get; }

	DateTime EndTime { get; }

	long ExecutionTime { get; }

	TimeSpan ExecutionSpan { get; }
}
