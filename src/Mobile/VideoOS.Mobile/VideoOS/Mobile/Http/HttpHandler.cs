using System;
using System.Collections.Generic;
using System.Net;
using VideoOS.Mobile.Portable.Http;

namespace VideoOS.Mobile.Http;

public class HttpHandler
{
	public delegate HttpStatuses Handler(HttpListenerContext httpContext);

	private readonly Dictionary<string, Handler> _handlers = new Dictionary<string, Handler>();

	private readonly object _handlersLocker = new object();

	public void AddHandler(string relativePath, Handler handler)
	{
		relativePath = relativePath.ToLower();
		lock (_handlersLocker)
		{
			_handlers.Add(relativePath, handler);
		}
	}

	public void RemoveHandler(string relativePath)
	{
		relativePath = relativePath.ToLower();
		lock (_handlersLocker)
		{
			if (_handlers.ContainsKey(relativePath))
			{
				_handlers.Remove(relativePath);
			}
		}
	}

	public HttpStatuses Process(string relativePath, HttpListenerContext httpContext)
	{
		HttpStatuses httpStatuses = HttpStatuses.Ok;
		relativePath = relativePath.ToLower();
		try
		{
			Handler handler = null;
			lock (_handlersLocker)
			{
				foreach (KeyValuePair<string, Handler> handler2 in _handlers)
				{
					if (relativePath.StartsWith(handler2.Key))
					{
						handler = handler2.Value;
						break;
					}
				}
			}
			return handler?.Invoke(httpContext) ?? HttpStatuses.NotFound;
		}
		catch (Exception)
		{
			return HttpStatuses.InternalServerError;
		}
	}
}
