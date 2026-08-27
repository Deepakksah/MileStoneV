using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Common.EnvironmentServices.Web;

public sealed class HttpContent : IHttpContent
{
	private IHttpContentHeaders _headers;

	public HttpContent Inner{ get; }

	public IHttpContentHeaders Headers => _headers ?? (_headers = new HttpContentHeaders(Inner.Headers));

	public HttpContent(HttpContent httpContent)
	{
		Inner = httpContent ?? throw new ArgumentNullException("httpContent");
	}

	public Task<string> ReadAsStringAsync()
	{
		return Inner.ReadAsStringAsync();
	}

	public Task<string> ReadAsStringAsync(CancellationToken cancellationToken)
	{
		return Inner.ReadAsStringAsync();
	}

	public Task<T> ReadAsAsync<T, TFormatter>(IEnumerable<TFormatter> formatters)
	{
		return Inner.ReadAsAsync<T, TFormatter>(formatters);
	}

	public Task<T> ReadAsAsync<T, TFormatter>(IEnumerable<TFormatter> formatters, CancellationToken cancellationToken)
	{
		return Inner.ReadAsAsync<T, TFormatter>(formatters, cancellationToken);
	}

	public Task<Stream> ReadAsStreamAsync()
	{
		return Inner.ReadAsStreamAsync();
	}

	public Task<Stream> ReadAsStreamAsync(CancellationToken cancellationToken)
	{
		return Inner.ReadAsStreamAsync();
	}
}
