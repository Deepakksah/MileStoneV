using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VideoOS.Installer.ServerConfiguratorPlugin;

public sealed class ManagementServerEndpointVerifier : IDisposable
{
	private static readonly TimeSpan _retryDelaySec = TimeSpan.FromSeconds(1.0);

	private readonly Uri _address;

	private readonly HttpClient _httpClient;

	public ManagementServerEndpointVerifier(Uri address)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		_address = address;
		_httpClient = new HttpClient();
	}

	private Uri GetAddressResolverUri()
	{
		return new UriBuilder(_address)
		{
			Path = "/managementserver/.Well-known/uris"
		}.Uri;
	}

	public async Task<bool> WaitForServiceAvailabilityAsync(TimeSpan timeout)
	{
		using CancellationTokenSource cts = new CancellationTokenSource();
		cts.CancelAfter(timeout);
		Uri address = GetAddressResolverUri();
		while (true)
		{
			try
			{
				await GetManagementServerUris(address, cts.Token).ConfigureAwait(continueOnCapturedContext: false);
				return true;
			}
			catch (Exception)
			{
			}
			try
			{
				await Task.Delay(_retryDelaySec, cts.Token).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (OperationCanceledException)
			{
				return false;
			}
		}
	}

	private async Task<ManagementServerUrisData> GetManagementServerUris(Uri addressResolverUri, CancellationToken cancellationToken)
	{
		return JsonConvert.DeserializeObject<ManagementServerUrisData>(await GetRawUris(addressResolverUri, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
	}

	private async Task<string> GetRawUris(Uri urisRequestAddress, CancellationToken cancellationToken)
	{
		HttpResponseMessage urisResponse = await _httpClient.GetAsync(urisRequestAddress, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		try
		{
			urisResponse.EnsureSuccessStatusCode();
			return await urisResponse.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		finally
		{
			((IDisposable)urisResponse)?.Dispose();
		}
	}

	public void Dispose()
	{
		((HttpMessageInvoker)_httpClient).Dispose();
		GC.SuppressFinalize(this);
	}
}
