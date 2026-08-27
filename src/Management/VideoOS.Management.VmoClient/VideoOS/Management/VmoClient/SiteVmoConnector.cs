using System;
using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.Management.VmoClient;

public class SiteVmoConnector : ISiteVmoConnector
{
	private readonly IServerConnectionFactory _serverConnectionFactory;

	private readonly IVmoClientFactory _vmoClientFactory;

	private readonly IVmoClientValidator _vmoClientValidator;

	public SiteVmoConnector(IServerConnectionFactory serverConnectionFactory, IVmoClientFactory vmoClientFactory, IVmoClientValidator vmoClientValidator)
	{
		_serverConnectionFactory = serverConnectionFactory;
		_vmoClientFactory = vmoClientFactory;
		_vmoClientValidator = vmoClientValidator;
	}

	public IVmoClient TryConnect(Guid siteId, Uri address, ITokenCache tokenCache)
	{
		IServerConnection serverConnection = _serverConnectionFactory.Create(address, tokenCache);
		return TryCreateConnectedVmoClient(serverConnection, siteId);
	}

	public IVmoClient TryConnectLegacy(Guid siteId, Uri address)
	{
		IServerConnection serverConnection = _serverConnectionFactory.Create(address);
		return TryCreateConnectedVmoClient(serverConnection, siteId);
	}

	public IVmoClient TryConnectLegacyAsUser(Guid siteId, Uri address, string username, string password)
	{
		IServerConnection serverConnection = _serverConnectionFactory.Create(address, username, password);
		return TryCreateConnectedVmoClient(serverConnection, siteId);
	}

	private IVmoClient TryCreateConnectedVmoClient(IServerConnection serverConnection, Guid siteId)
	{
		if (serverConnection == null)
		{
			throw new ArgumentNullException("serverConnection");
		}
		IVmoClient vmoClient = null;
		bool flag = false;
		try
		{
			serverConnection.Connect();
			flag = true;
			vmoClient = _vmoClientFactory.Create(serverConnection);
			if (!_vmoClientValidator.IsValid(vmoClient, siteId))
			{
				throw new InvalidServerIdException(siteId.ToString());
			}
			return vmoClient;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
			if (flag)
			{
				serverConnection.Disconnect();
			}
			serverConnection.Dispose();
			vmoClient?.Dispose();
			throw;
		}
	}
}
