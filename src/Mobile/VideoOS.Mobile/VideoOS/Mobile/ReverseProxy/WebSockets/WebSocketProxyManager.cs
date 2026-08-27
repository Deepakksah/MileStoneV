using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.ReverseProxy.WebSockets;

public class WebSocketProxyManager : IDisposable
{
	private readonly List<WebSocketProxyInstance> _instances = new List<WebSocketProxyInstance>();

	private readonly object _instancesLock = new object();

	private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

	private static readonly WebSocketProxyManager _instance = new WebSocketProxyManager();

	private const int _monitorInterval = 5;

	private const int _activityTimeout = 60;

	private bool _disposedValue;

	public static WebSocketProxyManager Instance => _instance;

	private WebSocketProxyManager()
	{
		Task.Run(() => InstanceMonitor(_cancellationTokenSource.Token));
	}

	public async Task<HttpStatusCode> AddProxyAsync(HttpListenerContext listenerContext, HttpRequestMessage targetRequestMessage, TimeSpan timeout)
	{
		WebSocketProxyInstance proxyInstance = new WebSocketProxyInstance(InstanceClosed, _cancellationTokenSource.Token);
		HttpStatusCode httpStatusCode = await proxyInstance.StartAsync(listenerContext, targetRequestMessage, timeout).ConfigureAwait(continueOnCapturedContext: false);
		if (httpStatusCode == HttpStatusCode.SwitchingProtocols)
		{
			lock (_instancesLock)
			{
				_instances.Add(proxyInstance);
			}
		}
		return httpStatusCode;
	}

	private void InstanceClosed(WebSocketProxyInstance instance)
	{
		lock (_instancesLock)
		{
			_instances.Remove(instance);
		}
	}

	public void Close()
	{
		_cancellationTokenSource.Cancel();
	}

	private async Task InstanceMonitor(CancellationToken cancellationToken)
	{
		_ = 1;
		try
		{
			List<WebSocketProxyInstance> lastInstancesMarkedForClose = new List<WebSocketProxyInstance>();
			while (!cancellationToken.IsCancellationRequested)
			{
				try
				{
					List<WebSocketProxyInstance> list = null;
					lock (_instancesLock)
					{
						list = new List<WebSocketProxyInstance>(_instances);
					}
					if (list.Count > 0)
					{
						DateTime utcNow = DateTime.UtcNow;
						List<Task> list2 = new List<Task>();
						List<WebSocketProxyInstance> list3 = new List<WebSocketProxyInstance>();
						foreach (WebSocketProxyInstance instance in list)
						{
							if (instance.ActivityTimestamp.AddMinutes(60.0) < utcNow)
							{
								list3.Add(instance);
								bool flag = lastInstancesMarkedForClose.Any((WebSocketProxyInstance marked) => marked.InstanceId == instance.InstanceId);
								Log.Debug("WebSocketProxyManager.InstanceMonitor", $"WebSocketProxyManager: Closing instance {instance.InstanceId} due to inactivity. Force={flag}");
								list2.Add(instance.CloseAsync(flag));
							}
						}
						lastInstancesMarkedForClose = list3;
						if (list2.Count > 0)
						{
							await Task.WhenAll(list2).ConfigureAwait(continueOnCapturedContext: false);
						}
					}
				}
				catch (Exception ex)
				{
					Log.Error("WebSocketProxyManager.InstanceMonitor", ex.Message);
					Log.Exception(ex);
				}
				await Task.Delay(TimeSpan.FromMinutes(5.0)).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		catch (Exception ex2)
		{
			Log.Error("WebSocketProxyManager.InstanceMonitor failed and is exiting", ex2.Message);
			Log.Exception(ex2);
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposedValue)
		{
			if (disposing)
			{
				_cancellationTokenSource.Cancel();
			}
			_disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
