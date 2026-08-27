using System;
using System.ServiceModel;

namespace VideoOS.Management.VmoClient.Proxy;

public sealed class ServerProxy : ProxyClientServiceBase<ServerServiceClient>
{
	public override string ServiceName => ResolveServiceName("ServerService");

	public override Uri ServiceAddress => new Uri("urn:videoos:management:server:serverservice");

	public override ICommunicationObject CommunicationObject
	{
		get
		{
			if (base.ServiceClient != null)
			{
				return base.ServiceClient.ChannelFactory;
			}
			return null;
		}
	}

	public ServerProxy(ServerConnection serverConnection)
		: base((ConnectionSettings)serverConnection)
	{
	}

	public override void Initialize()
	{
		if (base.ServiceClient != null)
		{
			try
			{
				base.ServiceClient.Close();
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
				base.ServiceClient.Abort();
			}
		}
		base.ServiceClient = new ServerServiceClient(GetBinding(), GetEndpointAddress());
		ConfigureEndpoint(base.ServiceClient.Endpoint);
		ConfigureCredentials(base.ServiceClient.ClientCredentials);
		base.IsInitialized = true;
	}

	public string CheckAuthentication()
	{
		try
		{
			return CallWithRetry(() => base.ServiceClient.CheckAuthentication());
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}
}
