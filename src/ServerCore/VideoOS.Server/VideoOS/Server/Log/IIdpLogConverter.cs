using VideoOS.Common.Integration;
using VideoOS.IdentityServer.Common.Model.Responses;

namespace VideoOS.Server.Log;

public interface IIdpLogConverter
{
	LogEntry ConvertIdpLog(EventLogModel idpLog, string idpServerName);
}
