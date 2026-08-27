using System;

namespace VideoOS.Server;

public interface IEvidenceLockOldRecorderConfig
{
	TimeSpan WaitTimeAfterStartup { get; }

	TimeSpan WaitTimeInCheckLoop { get; }
}
