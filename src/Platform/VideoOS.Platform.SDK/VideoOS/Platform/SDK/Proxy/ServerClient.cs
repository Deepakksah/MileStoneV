using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using VideoOS.Platform.Login;
using VideoOS.Platform.SDK.Util;

namespace VideoOS.Platform.SDK.Proxy;

public sealed class ServerClient : ProxyClientServiceBase<ServerServiceClient>
{
	public override string ServiceName => "ServerService.svc";

	public override string ServiceNameOAuth => "ServerServiceOAuth.svc";

	public override Uri ServiceAddress => GetServerAddress();

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

	public ServerClient(LoginSettings loginSettings, Binding binding)
		: base(loginSettings, binding)
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
			ServerTrace.BeforeCall("ServerClient", "CheckAuthentication", ServiceAddress.ToString());
			return CallWithRetry(() => base.ServiceClient.CheckAuthentication());
		}
		catch (Exception ex)
		{
			throw ProxyClientBase.HandleException(ex);
		}
		finally
		{
			ServerTrace.AfterCall("ServerClient", "CheckAuthentication", ServiceAddress.ToString());
		}
	}

	public ServerInformation GetServerInformation()
	{
		try
		{
			ServerTrace.BeforeCall("ServerClient", "GetServerInformation", ServiceAddress.ToString());
			return CallWithRetry(() => base.ServiceClient.GetServerInformation());
		}
		catch (Exception ex)
		{
			throw ProxyClientBase.HandleException(ex);
		}
		finally
		{
			ServerTrace.AfterCall("ServerClient", "GetServerInformation", ServiceAddress.ToString());
		}
	}
}
