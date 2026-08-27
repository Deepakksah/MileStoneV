using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VideoOS.LogServer.Communication.Writers;

namespace VideoOS.IM.Server.Logger.AuditLog;

public interface IAuditLogger
{
	void Initialize([NotNull] Uri logServerUri);

	void Initialize([NotNull] ILogWriter logWriter);

	Task WriteAuditLogEntryAsync(Guid messageId, IDictionary<string, string> parameters, string source, string user, string userLocation);
}
