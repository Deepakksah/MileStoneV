namespace VideoOS.Common.Integration;

public interface ILogManager
{
	void AddLogEntry(LogEntry logEntry);

	void AddLogEntry(LogEntryId logEntryId, params string[] arguments);

	void AddAuditLogEntry(LogEntryAudit logEntryAudit);
}
