using System;
using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.Management.VmoClient;

public interface IServerConnectionFactory
{
	IServerConnection Create(Uri address);

	IServerConnection Create(Uri address, string username, string password);

	IServerConnection Create(Uri address, ITokenCache tokenCache);
}
