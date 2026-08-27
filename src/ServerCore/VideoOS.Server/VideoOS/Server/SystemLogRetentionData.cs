namespace VideoOS.Server;

public sealed class SystemLogRetentionData
{
	public long MaxNumberOfLogEntries { get; set; }

	public long DaysToKeepLogEntries { get; set; }
}
