namespace VideoOS.Server;

public sealed class RuleLogRetentionData
{
	public long MaxNumberOfLogEntries { get; set; }

	public long DaysToKeepLogEntries { get; set; }
}
