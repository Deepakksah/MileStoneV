using System;
using System.Net;
using System.Text;
using VideoOS.Mobile.Http.FileSystem;
using VideoOS.Mobile.Portable.Common;
using VideoOS.Mobile.Portable.Http;
using VideoOS.Mobile.Portable.Interfaces;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.Net;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Http;

public class HttpServer : IDisposable, IControl
{
	public enum Type
	{
		[Description("http")]
		Http,
		[Description("https")]
		HttpSecure
	}

	private readonly IHttpListener _listener;

	private readonly string _listenPath;

	private bool _disposed;

	private readonly HttpHandler _httpHandler = new HttpHandler();

	private readonly Type _type;

	private readonly string _prefix;

	private readonly bool _closeConnectionOnResponse;

	public IFolder Root { get; set; }

	public HttpServer(string ipAddress, int port, string path, Type type, IHttpListenerFactory factory, bool closeConnectionOnResponse = true)
	{
		_listenPath = path;
		_type = type;
		_closeConnectionOnResponse = closeConnectionOnResponse;
		StringBuilder stringBuilder = new StringBuilder().Append(EnumsHelper.GetDescription(type)).Append("://");
		_prefix = stringBuilder.ToString();
		stringBuilder.Append(ipAddress).Append(":").Append(port)
			.Append("/");
		if (!string.IsNullOrWhiteSpace(path))
		{
			stringBuilder.Append(path).Append("/");
		}
		_listener = factory.CreateListener(new string[1] { stringBuilder.ToString() });
		_listener.SubsribeForAllMethods(OnHttpClient);
	}

	private void OnHttpClient(HttpListenerContext httpContext)
	{
		HttpListenerRequest request = httpContext.Request;
		HttpListenerResponse response = httpContext.Response;
		HttpStatuses httpStatuses = HttpStatuses.Ok;
		try
		{
			httpStatuses = CorrectInputAddress(httpContext);
			string relativePath = HttpServerHelper.ExtractRelativePath(request.Url.LocalPath, _listenPath);
			if (HttpStatuses.Ok == httpStatuses)
			{
				httpStatuses = ProcessHandlerRequest(relativePath, httpContext);
			}
			if (HttpStatuses.NotFound == httpStatuses)
			{
				httpStatuses = HttpFileHandler.ProcessFileRequest(relativePath, httpContext, Root);
			}
		}
		catch (Exception ex)
		{
			httpStatuses = HttpStatuses.InternalServerError;
			Log.Error("HttpServer.OnHttpClient", ex.Message);
		}
		response.StatusCode = (int)httpStatuses;
		response.StatusDescription = EnumsHelper.GetDescription(httpStatuses);
		if (_closeConnectionOnResponse && httpStatuses != HttpStatuses.SwitchingProtocol)
		{
			response.Close();
		}
	}

	private HttpStatuses ProcessHandlerRequest(string relativePath, HttpListenerContext httpContext)
	{
		return _httpHandler.Process(relativePath, httpContext);
	}

	private HttpStatuses CorrectInputAddress(HttpListenerContext httpContext)
	{
		HttpListenerRequest request = httpContext.Request;
		if (!string.Equals(request.RawUrl, request.Url.PathAndQuery, StringComparison.InvariantCultureIgnoreCase))
		{
			return RedirectToAddress(httpContext, request.Url.PathAndQuery);
		}
		return HttpStatuses.Ok;
	}

	private HttpStatuses RedirectToAddress(HttpListenerContext httpContext, string address)
	{
		httpContext.Response.AddHeaderExt("Location", address);
		return HttpStatuses.MovedPermanently;
	}

	public void AddHandler(string relativePath, HttpHandler.Handler handler)
	{
		_httpHandler.AddHandler(relativePath, handler);
	}

	public void RemoveHandler(string relativePath)
	{
		_httpHandler.RemoveHandler(relativePath);
	}

	public void Start()
	{
		_listener.Start();
	}

	public void Stop()
	{
		_listener.Stop();
	}

	private void QuiteStop()
	{
		try
		{
			Stop();
		}
		catch (Exception)
		{
		}
	}

	~HttpServer()
	{
		Dispose(disposing: true);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (_disposed)
		{
			return;
		}
		if (disposing)
		{
			QuiteStop();
			if (Root != null)
			{
				Root.Dispose();
			}
			Root = null;
		}
		_disposed = true;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
