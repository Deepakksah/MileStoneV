using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Mobile.Portable.Interfaces;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Portable.Net.WebSockets;

public class WsClientImplementer : IClientContext, IRequest, IResponse
{
	private ClientWebSocket _wsClient;

	public string Protocol { get; private set; }

	public string Method { get; set; }

	public WsClientImplementer(bool isSecured)
	{
		Protocol = (isSecured ? "wss://" : "ws://");
	}

	public IRequest CreateRequest(string webAddress)
	{
		_wsClient = new ClientWebSocket();
		_wsClient.ConnectAsync(new Uri(webAddress), CancellationToken.None).Wait();
		return this;
	}

	public void GetResponse(Action<IResponse> asyncCallback)
	{
		Task.Factory.StartNew(delegate
		{
			try
			{
				asyncCallback(this);
				if (!_wsClient.CloseAsync(WebSocketCloseStatus.NormalClosure, "Websocket close request", CancellationToken.None).Wait(new TimeSpan(0, 2, 0)))
				{
					_wsClient.Abort();
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				_wsClient.Dispose();
			}
		});
	}

	public int Read(byte[] buffer, int nOffset, int nCount)
	{
		int result = 0;
		try
		{
			ArraySegment<byte> buffer2 = new ArraySegment<byte>(buffer, nOffset, nCount);
			if (_wsClient.State == WebSocketState.Open)
			{
				Task<WebSocketReceiveResult> task = _wsClient.ReceiveAsync(buffer2, CancellationToken.None);
				task.Wait();
				if (task.Result.MessageType == WebSocketMessageType.Close)
				{
					_wsClient.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
				}
				else if (task.Result.Count > 0)
				{
					result = task.Result.Count;
				}
			}
		}
		catch (Exception ex)
		{
			if (ex is AggregateException)
			{
				ex = (ex as AggregateException).Flatten();
			}
			VideoOS.Mobile.Portable.Log.Log.Error("WebSocketExt.SendAsync", $"Send failed with error: {ex.Message}");
		}
		Thread.Sleep(1);
		return result;
	}
}
