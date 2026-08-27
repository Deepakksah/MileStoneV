using System;

namespace VideoOS.Server.CommandHandler;

public interface IVmoClientFactory : IDisposable
{
	VmoClientsPool.VmoTimedClient GetConnectedClient();

	void ReleaseClient(VmoClientsPool.VmoTimedClient client);
}
