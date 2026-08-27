using System.Collections.Generic;
using VideoOS.Common.Integration;

namespace VideoOS.Server;

public sealed class SessionLogger : ILogAuditLogger
{
	private List<LogEntryAudit> sessionLogs = new List<LogEntryAudit>();

	public ICollection<LogEntryAudit> SessionLogs => sessionLogs;

	public void AddAuditLogEntry(LogEntryAudit logEntryAudit)
	{
		sessionLogs.Add(logEntryAudit);
	}
}
