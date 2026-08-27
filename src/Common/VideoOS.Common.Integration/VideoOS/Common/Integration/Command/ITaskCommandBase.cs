namespace VideoOS.Common.Integration.Command;

public interface ITaskCommandBase
{
	long TaskId { get; }

	string TaskXml { get; }

	int RetryCount { get; set; }

	bool StopRetry { get; }

	bool Permanent { get; set; }
}
