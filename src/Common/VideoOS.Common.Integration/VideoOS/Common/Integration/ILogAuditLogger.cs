namespace VideoOS.Common.Integration;

public interface ILogAuditLogger
{
	void AddAuditLogEntry(LogEntryAudit logEntryAudit);
}
