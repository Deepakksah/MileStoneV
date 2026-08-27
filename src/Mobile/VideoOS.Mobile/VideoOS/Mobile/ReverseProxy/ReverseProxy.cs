using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using VideoOS.Mobile.Portable.Http;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.ReverseProxy.Authorization;
using VideoOS.Mobile.ReverseProxy.WebSockets;

namespace VideoOS.Mobile.ReverseProxy;

public class ReverseProxy : IRequestHandler, IDisposable
{
	private bool _disposed;

	private readonly HttpClient _httpClient;

	private readonly IServiceDiscovery _discoveryService;

	private readonly IPathMapper _pathMapper;

	private readonly string _xForwardedPrefix;

	private readonly Dictionary<string, Action<HttpListenerContext, IEnumerable<string>>> _responseHeadersHandlers = new Dictionary<string, Action<HttpListenerContext, IEnumerable<string>>>();

	public readonly Dictionary<string, Action<HttpRequestMessage, IEnumerable<string>>> _requestHeadersHandlers = new Dictionary<string, Action<HttpRequestMessage, IEnumerable<string>>>();

	public VideoOS.Mobile.ReverseProxy.Authorization.Authorization Authorization { get; private set; }

	public ReverseProxy(IServiceDiscovery discoveryService, IPathMapper pathMapper, string xForwardedPrefix, TimeSpan timeout)
	{
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		_responseHeadersHandlers.Add("Content-Length".ToLower(), HandleContentLength);
		_responseHeadersHandlers.Add("Transfer-Encoding".ToLower(), HandleTransferEncoding);
		_discoveryService = discoveryService;
		_pathMapper = pathMapper;
		_xForwardedPrefix = xForwardedPrefix;
		Authorization = new VideoOS.Mobile.ReverseProxy.Authorization.Authorization();
		HttpClientHandler val = new HttpClientHandler
		{
			AllowAutoRedirect = false,
			UseCookies = false
		};
		_httpClient = new HttpClient((HttpMessageHandler)(object)val)
		{
			Timeout = timeout
		};
	}

	public void AddHeaderHandler(string headerName, Action<HttpListenerContext, IEnumerable<string>> handler)
	{
		_responseHeadersHandlers.Add(headerName.ToLower(), handler);
	}

	public void AddRequestHeaderHandler(string headerName, Action<HttpRequestMessage, IEnumerable<string>> handler)
	{
		_requestHeadersHandlers.Add(headerName.ToLower(), handler);
	}

	public HttpStatuses HandleRequest(HttpListenerContext context)
	{
		return HandleRequest(context, requiresAuthentication: false);
	}

	private async Task<HttpStatusCode> HandleRequestAsync(HttpListenerContext context, bool requiresAuthentication)
	{
		HttpRequestMessage targetRequest = null;
		HttpStatusCode statusCode = BuildTargetMessage(context.Request, out targetRequest, requiresAuthentication);
		if (statusCode == HttpStatusCode.OK)
		{
			if (context.Request.IsWebSocketRequest)
			{
				statusCode = await WebSocketProxyManager.Instance.AddProxyAsync(context, targetRequest, _httpClient.Timeout).ConfigureAwait(continueOnCapturedContext: false);
				context.Response.StatusCode = (int)statusCode;
			}
			else
			{
				HttpResponseMessage val = await _httpClient.SendAsync(targetRequest).ConfigureAwait(continueOnCapturedContext: false);
				CopyFromTargetResponseHeaders(context, val);
				statusCode = val.StatusCode;
				context.Response.StatusCode = (int)statusCode;
				await val.Content.CopyToAsync(context.Response.OutputStream).ConfigureAwait(continueOnCapturedContext: false);
			}
			return statusCode;
		}
		context.Response.StatusCode = (int)statusCode;
		return statusCode;
	}

	public HttpStatuses HandleRequest(HttpListenerContext context, bool requiresAuthentication)
	{
		try
		{
			Task<HttpStatusCode> task = Task.Run(() => HandleRequestAsync(context, requiresAuthentication));
			HttpStatusCode result = task.Result;
			if (!task.IsCompleted && task.Status != TaskStatus.RanToCompletion)
			{
				throw new Exception("Proxy request async task failed");
			}
			return (HttpStatuses)result;
		}
		catch (Exception e)
		{
			LogException(e);
			return HttpStatuses.InternalServerError;
		}
	}

	private void LogException(Exception e)
	{
		Log.Exception(e);
		if (e.InnerException != null)
		{
			LogException(e.InnerException);
		}
		if (!(e is AggregateException ex))
		{
			return;
		}
		foreach (Exception innerException in ex.InnerExceptions)
		{
			LogException(innerException);
		}
	}

	private HttpStatusCode BuildTargetMessage(HttpListenerRequest request, out HttpRequestMessage targetRequest, bool requiresAuthentication = false)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		targetRequest = null;
		Uri uri = BuildTargetUri(request);
		if (uri != null)
		{
			targetRequest = new HttpRequestMessage
			{
				RequestUri = uri,
				Method = HttpMethods.GetMethod(request.HttpMethod)
			};
			if (request.HttpMethod == "OPTIONS")
			{
				return HttpStatusCode.OK;
			}
			if (HttpMethods.HasContent(targetRequest.Method))
			{
				StreamContent content = new StreamContent(request.InputStream);
				targetRequest.Content = (HttpContent)(object)content;
				if (request.ContentType != null)
				{
					targetRequest.Content.Headers.ContentType = new MediaTypeHeaderValue(request.ContentType.Split(';')[0]);
				}
			}
			foreach (string header in request.Headers)
			{
				if (_requestHeadersHandlers.ContainsKey(header.ToLower()))
				{
					_requestHeadersHandlers[header.ToLower()](targetRequest, request.Headers.GetValues(header));
					Log.Trace("--- Process request headers", header + "; " + string.Join(", ", request.Headers.GetValues(header)));
				}
				else
				{
					((HttpHeaders)targetRequest.Headers).TryAddWithoutValidation(header, (IEnumerable<string>)(request.Headers.GetValues(header)?.ToArray()));
				}
			}
			if (requiresAuthentication)
			{
				RequestAuthorization requestAuthorization = new RequestAuthorization(Authorization, request);
				HttpStatusCode httpStatusCode = requestAuthorization.Authorize();
				if (httpStatusCode != HttpStatusCode.OK)
				{
					return httpStatusCode;
				}
				requestAuthorization.ApplyTokenHeader(targetRequest);
			}
			targetRequest.Headers.ExtendHeader("X-Forwarded-Proto", request.Url.Scheme);
			targetRequest.Headers.ExtendHeader("X-Forwarded-For", request.LocalEndPoint?.ToString());
			targetRequest.Headers.ExtendHeader("X-Forwarded-Host", request.Headers.GetValues("Host")?.FirstOrDefault());
			targetRequest.Headers.ExtendHeader("X-Forwarded-Prefix", _xForwardedPrefix);
			targetRequest.Headers.Host = uri.Host;
		}
		return HttpStatusCode.OK;
	}

	private void HandleContentLength(HttpListenerContext context, IEnumerable<string> values)
	{
		string text = values.FirstOrDefault();
		if (text != null)
		{
			context.Response.ContentLength64 = long.Parse(text);
		}
	}

	private void HandleTransferEncoding(HttpListenerContext context, IEnumerable<string> values)
	{
		context.Response.SendChunked = true;
	}

	private void CopyFromTargetResponseHeaders(HttpListenerContext context, HttpResponseMessage responseMessage)
	{
		ProcessResponseHeaders((HttpHeaders)(object)responseMessage.Headers, context);
		HttpContent content = responseMessage.Content;
		if (((content != null) ? content.Headers : null) != null)
		{
			ProcessResponseHeaders((HttpHeaders)(object)responseMessage.Content.Headers, context);
		}
	}

	private void ProcessResponseHeaders(HttpHeaders headers, HttpListenerContext context)
	{
		if (headers == null || context == null)
		{
			return;
		}
		foreach (KeyValuePair<string, IEnumerable<string>> header in headers)
		{
			Log.Trace("--- Process headers", header.Key + ": " + string.Join(", ", header.Value));
			if (_responseHeadersHandlers.ContainsKey(header.Key.ToLower()))
			{
				_responseHeadersHandlers[header.Key.ToLower()](context, header.Value);
				continue;
			}
			foreach (string item in header.Value)
			{
				context.Response.Headers.Add(header.Key, item);
			}
		}
	}

	private Uri BuildTargetUri(HttpListenerRequest request)
	{
		string relativeUri = _pathMapper.Map(request.RawUrl);
		return new Uri(_discoveryService.GetServiceUri(), relativeUri);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (_disposed)
		{
			return;
		}
		if (disposing)
		{
			HttpClient httpClient = _httpClient;
			if (httpClient != null)
			{
				((HttpMessageInvoker)httpClient).Dispose();
			}
		}
		_disposed = true;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	~ReverseProxy()
	{
		Dispose(disposing: true);
	}
}
