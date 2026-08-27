using System;

namespace VideoOS.IdentityServer.Common;

public interface IIdentityServerClientSettings
{
	Uri BaseServerAddress { get; }

	string ClientId { get; }

	string GrantType { get; }
}
