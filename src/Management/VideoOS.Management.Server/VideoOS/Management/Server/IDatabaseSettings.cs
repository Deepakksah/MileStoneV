using System;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public interface IDatabaseSettings
{
	int ConnectionTimeout { get; set; }

	int DeadlockPause { get; set; }

	int DeadlockRetries { get; set; }

	TimeSpan ExecutionTimeThreshold { get; set; }

	int RequestTimeout { get; set; }

	ISqlConnectionManager GetSqlConnectionManager();
}
