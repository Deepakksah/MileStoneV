namespace VideoOS.Server;

public sealed class AuditLogRetentionData
{
	public long MaxNumberOfLogEntries { get; set; }

	public long DaysToKeepLogEntries { get; set; }
}
