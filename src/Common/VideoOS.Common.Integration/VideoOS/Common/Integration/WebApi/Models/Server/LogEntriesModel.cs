using System.Collections.Generic;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class LogEntriesModel
{
	public IEnumerable<LogEntryAuditModel> AuditLogEntries { get; set; }

	public IEnumerable<LogEntrySystemModel> SystemLogEntries { get; set; }

	public IEnumerable<LogEntryRuleModel> RuleLogEntries { get; set; }
}
