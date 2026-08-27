using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;

namespace VideoOS.Common.EnvironmentServices.Web;

public class HttpListenerRequest : IHttpListenerRequest
{
	private System.Net.HttpListenerRequest _request;

	public bool IsWebSocketRequest => _request.IsWebSocketRequest;

	public string HttpMethod => _request.HttpMethod;

	public Uri Url => _request.Url;

	public string[] AcceptTypes => _request.AcceptTypes;

	public string ContentType => _request.ContentType;

	public NameValueCollection Headers => _request.Headers;

	public NameValueCollection QueryString => _request.QueryString;

	public Stream InputStream => _request.InputStream;

	public HttpListenerRequest(System.Net.HttpListenerRequest request)
	{
		_request = request;
	}
}
