using VideoOS.IdentityServer.Common.Model.Responses;

namespace VideoOS.Server.Log;

public interface IIdpLogHandler
{
	void Publish(EventLogModel[] idpEvents);
}
