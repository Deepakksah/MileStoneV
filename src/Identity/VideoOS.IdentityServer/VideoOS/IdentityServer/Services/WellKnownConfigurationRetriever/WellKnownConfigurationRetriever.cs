using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel.Client;
using VideoOS.IdentityServer.Model.Result;

namespace VideoOS.IdentityServer.Services.WellKnownConfigurationRetriever;

public class WellKnownConfigurationRetriever : IWellKnownConfigurationRetriever
{
	private readonly IHttpClientFactory _clientFactory;

	private readonly IHttpClientExtensionsHelper _httpClientExtensionsHelper;

	public WellKnownConfigurationRetriever(IHttpClientFactory clientFactory, IHttpClientExtensionsHelper httpClientExtensionsHelper)
	{
		_clientFactory = clientFactory;
		_httpClientExtensionsHelper = httpClientExtensionsHelper;
	}

	public async Task<Result<WellKnownConfigurationResponse>> GetWellKnownConfiguration(string address, bool requireHttps, CancellationToken cancellationToken = default(CancellationToken))
	{
		HttpClient client = _clientFactory.CreateClient();
		DiscoveryDocumentRequest request = new DiscoveryDocumentRequest
		{
			Address = address,
			Policy = new DiscoveryPolicy
			{
				RequireHttps = requireHttps
			}
		};
		DiscoveryDocumentResponse discoveryDocumentResponse = await _httpClientExtensionsHelper.GetDiscoveryDocumentAsync(client, request, cancellationToken);
		WellKnownConfigurationResponse extendedResponse = new WellKnownConfigurationResponse
		{
			DiscoveryDocumentResponse = discoveryDocumentResponse
		};
		if (discoveryDocumentResponse.IsError)
		{
			return Error.Failure(discoveryDocumentResponse.Error);
		}
		string loginProviderUrl = discoveryDocumentResponse.TryGet("external_providers_uri");
		if (loginProviderUrl == null)
		{
			return Error.Failure("'external_providers_uri' not found in retrieved configuration");
		}
		ProtocolRequest protocolRequest = request.Clone();
		protocolRequest.Method = HttpMethod.Get;
		protocolRequest.Address = loginProviderUrl;
		protocolRequest.Prepare();
		try
		{
			HttpResponseMessage httpResponseMessage = await client.SendAsync(protocolRequest, cancellationToken);
			if (!httpResponseMessage.IsSuccessStatusCode)
			{
				return Error.Failure("Error connecting to " + loginProviderUrl + ": " + httpResponseMessage.ReasonPhrase);
			}
			extendedResponse.LoginProviders = JsonSerializer.Deserialize<List<LoginProvider>>(await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken));
		}
		catch (Exception ex)
		{
			return Error.Failure("Error connecting to " + loginProviderUrl + ": " + ex.Message);
		}
		return extendedResponse;
	}
}
