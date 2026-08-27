using System;
using System.IO;
using System.Net;

namespace VideoOS.Common.EnvironmentServices.Web;

public class HttpListenerResponse : IHttpListenerResponse, IDisposable
{
	private System.Net.HttpListenerResponse _response;

	public string ContentType
	{
		get
		{
			return _response.ContentType;
		}
		set
		{
			_response.ContentType = value;
		}
	}

	public long ContentLength64
	{
		get
		{
			return _response.ContentLength64;
		}
		set
		{
			_response.ContentLength64 = value;
		}
	}

	public Stream OutputStream => _response.OutputStream;

	public int StatusCode
	{
		get
		{
			return _response.StatusCode;
		}
		set
		{
			_response.StatusCode = value;
		}
	}

	public string StatusDescription
	{
		get
		{
			return _response.StatusDescription;
		}
		set
		{
			_response.StatusDescription = value;
		}
	}

	public WebHeaderCollection Headers
	{
		get
		{
			return _response.Headers;
		}
		set
		{
			_response.Headers = value;
		}
	}

	public HttpListenerResponse(System.Net.HttpListenerResponse response)
	{
		_response = response;
	}

	public void Close()
	{
		_response.Close();
	}

	public void Dispose()
	{
		((IDisposable)_response).Dispose();
		GC.SuppressFinalize(this);
	}
}
