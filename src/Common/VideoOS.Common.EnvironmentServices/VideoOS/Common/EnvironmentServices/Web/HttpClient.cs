using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Common.EnvironmentServices.Web;

public sealed class HttpClient : HttpClient, IHttpClient, IDisposable
{
	public HttpClient()
	{
	}

	public HttpClient(HttpMessageHandler handler)
		: base(handler)
	{
	}

	async Task<IHttpResponseMessage> IHttpClient.GetAsync(Uri requestUri)
	{
		return new HttpResponseMessage(await ((HttpClient)this).GetAsync(requestUri).ConfigureAwait(continueOnCapturedContext: false));
	}

	async Task<IHttpResponseMessage> IHttpClient.GetAsync(Uri requestUri, CancellationToken cancellationToken)
	{
		return new HttpResponseMessage(await ((HttpClient)this).GetAsync(requestUri, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
	}

	async Task<IHttpResponseMessage> IHttpClient.SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		return new HttpResponseMessage(await ((HttpMessageInvoker)this).SendAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
	}
}
