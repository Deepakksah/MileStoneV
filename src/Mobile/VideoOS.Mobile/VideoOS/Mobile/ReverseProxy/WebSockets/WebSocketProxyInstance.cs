using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.ReverseProxy.WebSockets;

public class WebSocketProxyInstance
{
	private static readonly List<string> _headerBlacklist = new List<string> { "Sec-WebSocket-Key", "Sec-WebSocket-Version", "Sec-WebSocket-Extensions", "Connection", "Upgrade", "Host" };

	private readonly Guid _instanceId = Guid.NewGuid();

	private readonly CancellationTokenSource _cancellationTokenSource;

	private readonly CancellationToken _cancellationToken;

	private readonly Action<WebSocketProxyInstance> _instanceClosedNotification;

	private readonly DateTime _createTimestamp = DateTime.UtcNow;

	private WebSocketProxyWorker _worker;

	public Guid InstanceId => _instanceId;

	public DateTime CreateTimeStamp => _createTimestamp;

	public DateTime ActivityTimestamp => _worker?.ActivityTimestamp ?? DateTime.MinValue;

	public WebSocketProxyInstance(Action<WebSocketProxyInstance> instanceClosedNotification, CancellationToken cancellationToken)
	{
		_instanceClosedNotification = instanceClosedNotification;
		_cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
		_cancellationToken = _cancellationTokenSource.Token;
	}

	public async Task<HttpStatusCode> StartAsync(HttpListenerContext listenerContext, HttpRequestMessage targetRequestMessage, TimeSpan timeout)
	{
		HttpStatusCode result = default(HttpStatusCode);
		object obj;
		int num;
		try
		{
			Task<HttpListenerWebSocketContext> acceptTask = listenerContext.AcceptWebSocketAsync(null);
			await Task.WhenAny(acceptTask, Task.Delay(timeout)).ConfigureAwait(continueOnCapturedContext: false);
			if (!acceptTask.IsCompleted)
			{
				throw new Exception("Failed to accept incomming websocket due to timeout");
			}
			if (acceptTask.Result == null)
			{
				throw new Exception("Failed to accept incomming websocket due to no context");
			}
			HttpListenerWebSocketContext _clientContext = acceptTask.Result;
			Uri requestUri = targetRequestMessage.RequestUri;
			Uri uri = new UriBuilder
			{
				Scheme = ((requestUri.Scheme == "https") ? "wss" : "ws"),
				Host = requestUri.Host,
				Port = requestUri.Port,
				Path = requestUri.PathAndQuery
			}.Uri;
			ClientWebSocket targetSocket = new ClientWebSocket();
			foreach (KeyValuePair<string, IEnumerable<string>> item in (HttpHeaders)targetRequestMessage.Headers)
			{
				if (!_headerBlacklist.Contains(item.Key))
				{
					targetSocket.Options.SetRequestHeader(item.Key, item.Value?.FirstOrDefault());
				}
			}
			try
			{
				await targetSocket.ConnectAsync(uri, _cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception innerException)
			{
				throw new Exception("Failed to connect to websocket server", innerException);
			}
			_worker = new WebSocketProxyWorker(_clientContext.WebSocket, targetSocket, WorkerClosed, _cancellationToken);
			result = HttpStatusCode.SwitchingProtocols;
			return result;
		}
		catch (Exception ex)
		{
			obj = ex;
			num = 1;
		}
		if (num != 1)
		{
			return result;
		}
		Exception ex2 = (Exception)obj;
		Log.Error("WebSocketProxyManager.StartAsync", ex2.Message);
		Log.Exception(ex2);
		if (_worker != null)
		{
			await _worker.CloseAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		return HttpStatusCode.InternalServerError;
	}

	public async Task CloseAsync(bool force = false)
	{
		if (force)
		{
			_cancellationTokenSource.Cancel();
		}
		else if (_worker != null)
		{
			await _worker.CloseAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	private void WorkerClosed(WebSocketProxyWorker worker)
	{
		_worker = null;
		_instanceClosedNotification(this);
	}
}
