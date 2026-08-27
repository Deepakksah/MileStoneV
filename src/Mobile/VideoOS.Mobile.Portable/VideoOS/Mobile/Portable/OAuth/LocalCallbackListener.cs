using System;
using System.Collections.Generic;
using System.Net;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.Net;

namespace VideoOS.Mobile.Portable.OAuth;

public class LocalCallbackListener : ICallbackClilent, IDisposable
{
	private Action<HttpListenerContext> _subscriber;

	private Uri _redirectUrl;

	private IHttpListener _listener;

	private bool _disposing;

	private bool _isDisposed;

	public Uri RedirectUrl => _redirectUrl;

	public LocalCallbackListener(string alias = "", string hostname = "127.0.0.1", int port = 22231)
	{
		_redirectUrl = new UriBuilder(Uri.UriSchemeHttp, hostname, port, alias).Uri;
		_listener = new HttpListenerFactory<HttpListenerAsync>().CreateListener(new List<string> { _redirectUrl.ToString() });
		_listener.SubscribedMethods = new Dictionary<HttpMethods, OnHttpClient> { 
		{
			HttpMethods.GET,
			OnNewHttpClientReceived
		} };
	}

	private void OnNewHttpClientReceived(HttpListenerContext context)
	{
		try
		{
			if (_subscriber != null)
			{
				_subscriber(context);
			}
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "OnNewHttpClientReceived", ex.Message);
		}
	}

	public void SubscribeForCode(Action<HttpListenerContext> subscriber)
	{
		_subscriber = subscriber;
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
			if (_disposing && _listener != null)
			{
				(_listener as IDisposable)?.Dispose();
				_listener = null;
			}
			_isDisposed = true;
		}
	}

	public void Start()
	{
		_listener.Start();
	}
}
