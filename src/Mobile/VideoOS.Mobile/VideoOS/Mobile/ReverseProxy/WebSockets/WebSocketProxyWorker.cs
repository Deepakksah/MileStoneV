using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.ReverseProxy.WebSockets;

public class WebSocketProxyWorker
{
	private const int _closeTimeout = 10000;

	private const int _bufferSize = 4096;

	private readonly WebSocket _sourceSocket;

	private readonly WebSocket _targetSocket;

	private readonly Action<WebSocketProxyWorker> _workerClosedNotification;

	private readonly CancellationToken _cancellationToken;

	private DateTime _activityTimestamp = DateTime.UtcNow;

	public DateTime ActivityTimestamp => _activityTimestamp;

	public WebSocketProxyWorker(WebSocket sourceContext, WebSocket targetSocket, Action<WebSocketProxyWorker> workerClosedNotification, CancellationToken cancellationToken)
	{
		_sourceSocket = sourceContext;
		_targetSocket = targetSocket;
		_workerClosedNotification = workerClosedNotification;
		_cancellationToken = cancellationToken;
		Task.Run(async delegate
		{
			await RunAsync().ConfigureAwait(continueOnCapturedContext: false);
		});
	}

	public async Task RunAsync()
	{
		_ = 1;
		try
		{
			Task task = ForwardAsync(_sourceSocket, _targetSocket);
			Task task2 = ForwardAsync(_targetSocket, _sourceSocket);
			await Task.WhenAny(task, task2).ConfigureAwait(continueOnCapturedContext: false);
			await CloseAsync().ConfigureAwait(continueOnCapturedContext: false);
			_workerClosedNotification(this);
		}
		catch (Exception ex)
		{
			Log.Error("ProxyWorker.RunAsync", ex.Message);
			Log.Exception(ex);
		}
	}

	public async Task CloseAsync()
	{
		try
		{
			await Task.WhenAll(CloseAsync(_sourceSocket), CloseAsync(_targetSocket)).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (WebSocketException)
		{
		}
		catch (Exception ex2)
		{
			Log.Error("ProxyWorker.CloseAsync", ex2.Message);
			Log.Exception(ex2);
		}
	}

	private async Task CloseAsync(WebSocket socket)
	{
		if (socket.State != WebSocketState.Closed && socket.State != WebSocketState.Aborted)
		{
			Task closeTask = socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, string.Empty, _cancellationToken);
			await Task.WhenAny(closeTask, Task.Delay(10000)).ConfigureAwait(continueOnCapturedContext: false);
			if (!closeTask.IsCompleted)
			{
				socket.Abort();
			}
		}
	}

	public async Task ForwardAsync(WebSocket source, WebSocket target)
	{
		_ = 1;
		try
		{
			byte[] buffer = new byte[4096];
			while (!_cancellationToken.IsCancellationRequested && source.State == WebSocketState.Open && target.State == WebSocketState.Open)
			{
				WebSocketReceiveResult webSocketReceiveResult = await source.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				if (webSocketReceiveResult.MessageType == WebSocketMessageType.Close)
				{
					break;
				}
				await target.SendAsync(new ArraySegment<byte>(buffer, 0, webSocketReceiveResult.Count), webSocketReceiveResult.MessageType, webSocketReceiveResult.EndOfMessage, _cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				_activityTimestamp = DateTime.UtcNow;
			}
		}
		catch (WebSocketException)
		{
		}
		catch (Exception ex2)
		{
			Log.Error("ProxyWorker.ForwardAsync", ex2.Message);
			Log.Exception(ex2);
		}
	}
}
