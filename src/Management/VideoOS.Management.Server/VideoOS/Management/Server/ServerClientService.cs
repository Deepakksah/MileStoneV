using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Server.Webservice.Shared;

namespace VideoOS.Management.Server;

public class ServerClientService : IGeminiServerService, IDisposable
{
	private const string Area = "ServerClientService";

	private const string Layer = "Business";

	private bool _isDisposed;

	private static object _connectionPoolLock = new object();

	private static List<ServerWebServiceManager> _clientPool = new List<ServerWebServiceManager>();

	public ServerWebServiceManager GetClient()
	{
		lock (_connectionPoolLock)
		{
			if (_clientPool.Count > 50)
			{
				for (int i = 0; i < _clientPool.Count; i++)
				{
					if (!_clientPool[i].Active)
					{
						_clientPool[i].Dispose();
						_clientPool.RemoveAt(i);
						if (_clientPool.Count <= 50)
						{
							break;
						}
					}
				}
			}
			ServerWebServiceManager serverWebServiceManager = _clientPool.FirstOrDefault((ServerWebServiceManager x) => !x.Active);
			if (serverWebServiceManager == null)
			{
				serverWebServiceManager = new ServerWebServiceManager(new WcfWebServiceManagerExceptionHandler(), int.Parse(Config.Instance.GetAppSetting("ServerPort", "8080"), CultureInfo.InvariantCulture));
				_clientPool.Add(serverWebServiceManager);
			}
			serverWebServiceManager.Active = true;
			return serverWebServiceManager;
		}
	}

	public void ReleaseClient(ServerWebServiceManager client)
	{
		if (client == null)
		{
			return;
		}
		lock (_connectionPoolLock)
		{
			client.Active = false;
		}
	}

	public void StartService(IGeminiRequestContext requestContext)
	{
	}

	public void StopService(IGeminiRequestContext requestContext)
	{
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_isDisposed)
		{
		}
		_isDisposed = true;
	}
}
