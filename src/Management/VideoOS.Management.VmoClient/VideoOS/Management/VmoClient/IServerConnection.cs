using System;
using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.Management.VmoClient.Proxy;

namespace VideoOS.Management.VmoClient;

public interface IServerConnection : IDisposable
{
	ServerVersion ServerVersion { get; }

	string LogOnName { get; }

	bool IsOpen { get; }

	string ServerName { get; set; }

	int ServerPort { get; set; }

	bool ConnectAsUser { get; set; }

	string ConnectAsUserPassword { get; set; }

	NetworkProtocol NetworkProtocol { get; set; }

	ITokenCache TokenCache { get; }

	Uri ServerUri { get; }

	string ConnectAsUserName { get; }

	IServerConnection Copy();

	void Connect();

	void Disconnect();

	bool CheckVersion(ServerVersion requiredVersion);

	T GetClientProxy<T>() where T : ProxyClientBase;
}
