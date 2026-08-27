using System;
using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.Management.VmoClient;

public sealed class ServerConnectionFactory : IServerConnectionFactory
{
	public IServerConnection Create(Uri address)
	{
		return new ServerConnection(address.DnsSafeHost)
		{
			ServerPort = address.Port
		};
	}

	public IServerConnection Create(Uri address, string username, string password)
	{
		return new ServerConnection(address.DnsSafeHost, username, password)
		{
			ServerPort = address.Port
		};
	}

	public IServerConnection Create(Uri address, ITokenCache tokenCache)
	{
		return new ServerConnection(address.DnsSafeHost, address.Scheme == Uri.UriSchemeHttps, tokenCache)
		{
			ServerPort = address.Port
		};
	}
}
