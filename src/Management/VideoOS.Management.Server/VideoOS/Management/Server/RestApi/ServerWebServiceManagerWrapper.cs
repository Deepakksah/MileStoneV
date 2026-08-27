using System;
using System.Globalization;
using VideoOS.Server.Webservice.Shared;

namespace VideoOS.Management.Server.RestApi;

public class ServerWebServiceManagerWrapper : IServerWebServiceManager, IDisposable
{
	private ServerWebServiceManager _inner;

	public ServerWebServiceManagerWrapper()
	{
		_inner = new ServerWebServiceManager(int.Parse(Config.Instance.GetAppSetting("ServerPort", "8080"), CultureInfo.InvariantCulture));
	}

	public void Dispose()
	{
		_inner.Dispose();
	}

	public int GetVmoServerPort()
	{
		return _inner.CallWithRetry((ServerWebServiceClient client) => client.GetVmoServerPort(), null, (Exception ex, Action ac) => false);
	}

	public int GetVmoServerSecurePort()
	{
		return _inner.CallWithRetry((ServerWebServiceClient client) => client.GetVmoServerSecurePort(), null, (Exception ex, Action ac) => false);
	}
}
