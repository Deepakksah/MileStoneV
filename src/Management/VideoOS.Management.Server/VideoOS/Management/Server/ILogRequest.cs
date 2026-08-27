using System.Collections.Generic;

namespace VideoOS.Management.Server;

public interface ILogRequest
{
	int LogicalReads { get; }

	int PhysicalReads { get; }

	int CpuTime { get; }

	int ElapsedTime { get; }

	IList<MethodTime> SqlCalls { get; }

	IList<MethodTime> RecursiveSqlCalls { get; }

	void LogSqlCall(MethodTime methodTime);
}
