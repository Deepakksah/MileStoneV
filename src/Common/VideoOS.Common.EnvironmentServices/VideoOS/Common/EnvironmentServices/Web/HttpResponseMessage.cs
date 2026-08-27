using System;
using System.Net;
using System.Net.Http;

namespace VideoOS.Common.EnvironmentServices.Web;

public sealed class HttpResponseMessage : IHttpResponseMessage, IDisposable
{
	private readonly HttpResponseMessage _inner;

	private IHttpContent _content;

	private IHttpResponseHeaders _headers;

	public IHttpContent Content => _content ?? (_content = new HttpContent(_inner.Content));

	public HttpStatusCode StatusCode => _inner.StatusCode;

	public string ReasonPhrase => _inner.ReasonPhrase;

	public bool IsSuccessStatusCode => _inner.IsSuccessStatusCode;

	public IHttpResponseHeaders Headers => _headers ?? (_headers = new HttpResponseHeaders(_inner.Headers));

	public HttpResponseMessage(HttpResponseMessage httpResponseMessage)
	{
		_inner = httpResponseMessage ?? throw new ArgumentNullException("httpResponseMessage");
		_headers = new HttpResponseHeaders(_inner.Headers);
	}

	public void EnsureSuccessStatusCode()
	{
		_inner.EnsureSuccessStatusCode();
	}

	public void Dispose()
	{
		_inner.Dispose();
	}
}
