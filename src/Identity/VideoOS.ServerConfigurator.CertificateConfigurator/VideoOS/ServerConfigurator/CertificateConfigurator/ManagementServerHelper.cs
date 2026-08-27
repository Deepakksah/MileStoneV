using System;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VideoOS.Common.EnvironmentServices.Web;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.CertificateConfigurator;

public sealed class ManagementServerHelper : IManagementServerHelper, IDisposable
{
	private class ManagementServerUrisData
	{
		public string ProductVersion { get; set; }

		public string UnsecureManagementServer { get; set; }

		public string SecureManagementServer { get; set; }

		public string IdentityProvider { get; set; }

		public string[] ApiGateways { get; set; }
	}

	private readonly IHttpClientFactory _clientFactory;

	private readonly ILogger _logger;

	private IHttpClient _httpClient;

	private Uri _idpAddress;

	private IHttpClient HttpClient => _httpClient ?? (_httpClient = _clientFactory.Create());

	public ManagementServerHelper(ILogger logger, IHttpClientFactory clientFactory)
	{
		_clientFactory = clientFactory;
		_logger = logger;
	}

	public (bool? secureState, bool certificateError) IsSecure(Uri managementServerAddress, Uri alternativeAddress)
	{
		try
		{
			ResolveAuthorizationServerAddressAsync(managementServerAddress, CancellationToken.None).Wait();
			return (secureState: _idpAddress.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase), certificateError: false);
		}
		catch (Exception ex) when (ex.InnerException is AddressResolverException)
		{
			if (alternativeAddress != null && alternativeAddress != managementServerAddress)
			{
				bool? item = IsSecure(alternativeAddress, null).secureState;
				if (item.HasValue)
				{
					return (secureState: item.Value, certificateError: item.Value && ex.OriginatesFrom(typeof(AuthenticationException)));
				}
			}
			_logger.LogWarning(ex.InnerException.Message + ". " + ex.InnerException.InnerException?.Message);
			return (secureState: null, certificateError: false);
		}
		catch (Exception ex2)
		{
			_logger.LogError(FormattableString.Invariant($"Unexpected error: {ex2}"));
			return (secureState: null, certificateError: false);
		}
	}

	private async Task ResolveAuthorizationServerAddressAsync(Uri managementServerAddress, CancellationToken cancellationToken)
	{
		ManagementServerUrisData managementServerUrisData;
		try
		{
			managementServerUrisData = await GetManagementServerUris(managementServerAddress, cancellationToken);
		}
		catch (HttpRequestException ex)
		{
			HttpRequestException innerException = ex;
			throw new AddressResolverException("Error getting managament server uris", (Exception)(object)innerException);
		}
		catch (JsonSerializationException ex2)
		{
			JsonSerializationException innerException2 = ex2;
			throw new AddressResolverException("Error deserializing managament server uris data", (Exception)(object)innerException2);
		}
		catch (AuthenticationException innerException3)
		{
			throw new AddressResolverException("Certificate is not trusted", innerException3);
		}
		if (string.IsNullOrEmpty(managementServerUrisData.IdentityProvider))
		{
			throw new AddressResolverException("Error in uris data: IdentityProvider address not found");
		}
		if (!Uri.TryCreate(managementServerUrisData.IdentityProvider, UriKind.Absolute, out _idpAddress))
		{
			_idpAddress = null;
		}
	}

	private async Task<ManagementServerUrisData> GetManagementServerUris(Uri managementServerAddress, CancellationToken cancellationToken)
	{
		Uri uri = new UriBuilder(managementServerAddress)
		{
			Path = "/managementserver/.Well-known/uris"
		}.Uri;
		return JsonConvert.DeserializeObject<ManagementServerUrisData>(await GetRawUris(uri, cancellationToken));
	}

	private async Task<string> GetRawUris(Uri urisRequestAddress, CancellationToken cancellationToken)
	{
		using IHttpResponseMessage urisResponse = await HttpClient.GetAsync(urisRequestAddress, cancellationToken);
		urisResponse.EnsureSuccessStatusCode();
		return await urisResponse.Content.ReadAsStringAsync();
	}

	public void Dispose()
	{
		_httpClient?.Dispose();
		GC.SuppressFinalize(this);
	}
}
