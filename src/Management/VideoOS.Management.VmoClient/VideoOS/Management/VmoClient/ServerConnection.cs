using System;
using System.Security;
using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.Management.VmoClient.Proxy;

namespace VideoOS.Management.VmoClient;

public sealed class ServerConnection : ConnectionManager, IServerConnection, IDisposable
{
	public ServerConnection()
	{
		InitializeDefaults();
	}

	public ServerConnection(string serverName)
		: this()
	{
		try
		{
			base.ServerName = serverName;
		}
		catch (InvalidPropertyValueException ex)
		{
			throw new InvalidArgumentException(ex.Message, ex);
		}
	}

	public ServerConnection(string serverName, string userName, string password)
		: this()
	{
		try
		{
			base.ServerName = serverName;
			base.ConnectAsUser = true;
			base.ConnectAsUserName = userName;
			base.ConnectAsUserPassword = password;
		}
		catch (InvalidPropertyValueException ex)
		{
			throw new InvalidArgumentException(ex.Message, ex);
		}
	}

	public ServerConnection(string serverName, string userName, SecureString password)
		: this()
	{
		try
		{
			base.ServerName = serverName;
			base.ConnectAsUser = true;
			base.ConnectAsUserName = userName;
			base.ConnectAsUserPassword = EncryptionUtil.DecryptSecureString(password);
		}
		catch (InvalidPropertyValueException ex)
		{
			throw new InvalidArgumentException(ex.Message, ex);
		}
	}

	public ServerConnection(string serverName, bool useHttps, ITokenCache tokenCache)
	{
		if (tokenCache == null)
		{
			throw new InvalidArgumentException("Token cache can not be null");
		}
		try
		{
			base.ServerName = serverName;
			base.NetworkProtocol = (useHttps ? NetworkProtocol.Https : NetworkProtocol.Http);
			base.TokenCache = tokenCache;
			InitializeProxies();
		}
		catch (InvalidPropertyValueException ex)
		{
			throw new InvalidArgumentException(ex.Message, ex);
		}
	}

	private void InitializeDefaults()
	{
		InitializeProxies();
	}

	private void InitializeProxies()
	{
		RegisterClientProxy(new ServerProxy(this));
		RegisterClientProxy(new SecurityProxy(this));
		RegisterClientProxy(new ManagementServerProxy(this));
	}

	public bool CheckVersion(ServerVersion requiredVersion)
	{
		return base.ServerVersion >= requiredVersion;
	}

	public IServerConnection Copy()
	{
		ServerConnection serverConnection = ((base.TokenCache != null) ? new ServerConnection(base.ServerName, base.NetworkProtocol == NetworkProtocol.Https, base.TokenCache) : ((!base.ConnectAsUser) ? new ServerConnection(base.ServerName) : new ServerConnection(base.ServerName, base.ConnectAsUserName, base.ConnectAsUserPassword)));
		if (base.ServerPort != 0)
		{
			serverConnection.ServerPort = base.ServerPort;
		}
		return serverConnection;
	}
}
