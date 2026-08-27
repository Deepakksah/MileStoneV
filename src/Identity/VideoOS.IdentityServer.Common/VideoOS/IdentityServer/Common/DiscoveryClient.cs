using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Common.EnvironmentServices.Web;
using VideoOS.IdentityServer.Common.Model.Responses;

namespace VideoOS.IdentityServer.Common;

public class DiscoveryClient : IDiscoveryClient, IDisposable
{
	private bool _disposed;

	private readonly IEnumerable<Uri> _uris;

	private readonly IHttpClient _client;

	private CancellationToken _linkedCancellationToken;

	public DiscoveryClient(IEnumerable<Uri> uris, HttpMessageHandler innerHandler = null)
		: this(uris, new HttpClientFactory(), innerHandler)
	{
	}

	public DiscoveryClient(IEnumerable<Uri> uris, IHttpClientFactory httpClientFactory, HttpMessageHandler innerHandler)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		if (uris == null)
		{
			throw new ArgumentNullException("uris");
		}
		_uris = uris;
		HttpMessageHandler httpMessageHandler = (HttpMessageHandler)(((object)innerHandler) ?? ((object)new HttpClientHandler()));
		_client = httpClientFactory?.Create(httpMessageHandler) ?? throw new ArgumentNullException("httpClientFactory");
	}

	public async Task<DiscoveryResponse> GetAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return await GetAsyncInternal(".well-known/openid-configuration", new DiscoveryResponseFactory(), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<ExternalProviderDiscoveryResponse> GetExternalProviderAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return await GetAsyncInternal(".well-known/openid-configuration/externalproviders", new ExternalProviderDiscoveryResponseFactory(), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<T> GetAsyncInternal<T>(string discoveryEndpoint, IGenericDiscoveryResponseBaseFactory<T> factory, CancellationToken cancellationToken = default(CancellationToken)) where T : DiscoveryResponseBase
	{
		using CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
		_linkedCancellationToken = cancellationTokenSource.Token;
		T result = await WaitForFirstCompleted(_uris.Select((Uri url) => ProcessUrlAsync(url, _client, discoveryEndpoint, factory, _linkedCancellationToken)), factory).ConfigureAwait(continueOnCapturedContext: false);
		cancellationTokenSource.Cancel();
		cancellationToken.ThrowIfCancellationRequested();
		return result;
	}

	private static async Task<T> WaitForFirstCompleted<T>(IEnumerable<Task<T>> tasks, IGenericDiscoveryResponseBaseFactory<T> factory) where T : DiscoveryResponseBase
	{
		List<T> errorResponses = new List<T>();
		List<Task<T>> taskList = new List<Task<T>>(tasks);
		while (taskList.Count > 0)
		{
			Task<T> firstCompleted = await Task.WhenAny(taskList).ConfigureAwait(continueOnCapturedContext: false);
			T val = await firstCompleted.ConfigureAwait(continueOnCapturedContext: false);
			if (!val.IsError)
			{
				return val;
			}
			errorResponses.Add(val);
			taskList.Remove(firstCompleted);
		}
		return factory.Create("Error resolving authorization server address", errorResponses.ToArray());
	}

	private static async Task<T> ProcessUrlAsync<T>(Uri baseAddress, IHttpClient client, string discoveryEndpoint, IGenericDiscoveryResponseBaseFactory<T> factory, CancellationToken cancellationToken) where T : DiscoveryResponseBase
	{
		string endpoint = new UriBuilder(baseAddress)
		{
			Path = Path.Combine(baseAddress.AbsolutePath, discoveryEndpoint)
		}.Uri.AbsoluteUri;
		HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, endpoint);
		try
		{
			IHttpResponseMessage httpResponseMessage = await client.SendAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			T result;
			if (!httpResponseMessage.IsSuccessStatusCode)
			{
				result = factory.Create(httpResponseMessage.StatusCode, "No authorization server found at: " + endpoint + ": " + httpResponseMessage.ReasonPhrase);
			}
			else
			{
				result = factory.Create(baseAddress, await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false));
			}
			return result;
		}
		catch (Exception innerException)
		{
			while (innerException.InnerException != null)
			{
				innerException = innerException.InnerException;
			}
			return factory.Create(innerException, "No authorization server found at: " + endpoint + ": " + innerException.Message);
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing && !_disposed)
		{
			_disposed = true;
			_client.Dispose();
		}
	}
}
