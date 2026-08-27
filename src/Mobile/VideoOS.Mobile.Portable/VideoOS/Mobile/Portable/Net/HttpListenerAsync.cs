using System;
using System.Collections.Generic;
using System.Net;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Portable.Net;

public class HttpListenerAsync : HttpListenerBaseWrapper
{
	private bool _isListening;

	public HttpListenerAsync()
	{
	}

	public HttpListenerAsync(IEnumerable<string> prefixes)
		: base(prefixes)
	{
	}

	public override void Start()
	{
		_isListening = true;
		base.Start();
		_listener.BeginGetContext(ListenerCallback, _listener);
	}

	public override void Stop()
	{
		_isListening = false;
		base.Stop();
	}

	public override void Close()
	{
		_isListening = false;
		base.Close();
	}

	private void ListenerCallback(IAsyncResult result)
	{
		try
		{
			HttpListener httpListener = (HttpListener)result.AsyncState;
			HttpListenerContext httpListenerContext = null;
			try
			{
				httpListenerContext = httpListener.EndGetContext(result);
			}
			finally
			{
				if (_isListening)
				{
					httpListener.BeginGetContext(ListenerCallback, httpListener);
				}
				if (httpListenerContext != null)
				{
					OnNewHttpClient(httpListenerContext);
				}
			}
		}
		catch (HttpListenerException ex)
		{
			Logger.Instance.Log(LogContent.Error, "HttpListenerAsync.ListenerCallback", $"{ex} , ErrorCode: {ex.ErrorCode}");
		}
		catch (Exception arg)
		{
			Logger.Instance.Log(LogContent.Error, "HttpListenerAsync.ListenerCallback", $"{arg}");
		}
	}
}
