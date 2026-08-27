using System;
using System.Collections.Generic;
using System.Net;

namespace VideoOS.Mobile.Portable.Net;

public class HttpListenerBase : IHttpListener, IHttpListenerConfig
{
	protected readonly HttpListener _listener;

	private static TimeSpan _requestTimeout = TimeSpan.FromSeconds(5.0);

	public Dictionary<HttpMethods, OnHttpClient> SubscribedMethods { get; set; }

	public bool TcpKeepAlive { get; set; }

	public static void InitializeTimeouts(TimeSpan requestTimeout)
	{
		_requestTimeout = requestTimeout;
	}

	public HttpListenerBase()
	{
		_listener = new HttpListener();
		_listener.TimeoutManager.DrainEntityBody = _requestTimeout;
		_listener.TimeoutManager.EntityBody = _requestTimeout;
		_listener.TimeoutManager.HeaderWait = _requestTimeout;
	}

	public HttpListenerBase(IEnumerable<string> prefixes)
		: this()
	{
		SetPrefixes(prefixes);
	}

	public void SetPrefixes(IEnumerable<string> prefixes)
	{
		foreach (string prefix in prefixes)
		{
			_listener.Prefixes.Add(prefix);
		}
	}

	public void SubsribeForAllMethods(OnHttpClient callback)
	{
		if (SubscribedMethods == null)
		{
			SubscribedMethods = new Dictionary<HttpMethods, OnHttpClient>();
			for (int i = 0; i < 9; i++)
			{
				SubscribedMethods.Add((HttpMethods)i, callback);
			}
		}
	}

	protected virtual void OnNewHttpClient(HttpListenerContext httpContext)
	{
		HttpMethods key = (HttpMethods)Enum.Parse(typeof(HttpMethods), httpContext.Request.HttpMethod);
		if (SubscribedMethods?[key] != null)
		{
			SubscribedMethods[key](httpContext);
			return;
		}
		httpContext.Response.StatusCode = 501;
		httpContext.Response.Close();
	}

	public virtual void Start()
	{
		SetTcpKeepAlive(TcpKeepAlive);
		_listener.Start();
	}

	private void SetTcpKeepAlive(bool tcpKeepAlive)
	{
		try
		{
			if (tcpKeepAlive)
			{
				_listener.TimeoutManager.IdleConnection = new TimeSpan(18, 0, 0);
			}
		}
		catch (Exception)
		{
		}
	}

	public virtual void Stop()
	{
		_listener.Stop();
	}

	public virtual void Close()
	{
		_listener.Close();
	}
}
