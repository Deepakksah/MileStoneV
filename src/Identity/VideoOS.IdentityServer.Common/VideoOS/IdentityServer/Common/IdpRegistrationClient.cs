using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VideoOS.Common.EnvironmentServices.Web;
using VideoOS.IdentityServer.Common.Model.Responses;

namespace VideoOS.IdentityServer.Common;

public class IdpRegistrationClient : IdpClientBase, IRegistrationClient, IDisposable
{
	public IdpRegistrationClient(Uri baseAddress)
		: this(baseAddress, (ICredentials)null)
	{
	}

	public IdpRegistrationClient(Uri baseAddress, ICredentials credentials)
		: this(baseAddress, new HttpClientFactory().Create((HttpMessageHandler)(object)CreateClientHandler(credentials)))
	{
	}

	public IdpRegistrationClient(Uri baseAddress, IHttpClient client)
		: base(baseAddress, client)
	{
	}

	private static HttpClientHandler CreateClientHandler(ICredentials credentials = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		return new HttpClientHandler
		{
			Credentials = credentials,
			UseDefaultCredentials = (credentials == null)
		};
	}

	public async Task<ClientGetModel> GetClient(string clientId)
	{
		if (string.IsNullOrEmpty(clientId))
		{
			throw new ArgumentException("Value cannot be null or empty.", "clientId");
		}
		return await GetClient(await GetAdminTokenWithRegistrationScope().ConfigureAwait(continueOnCapturedContext: false), clientId).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<ClientGetModel> GetClient(string accessToken, string clientId)
	{
		if (string.IsNullOrEmpty(accessToken))
		{
			throw new ArgumentException("Value cannot be null or empty.", "accessToken");
		}
		if (string.IsNullOrEmpty(clientId))
		{
			throw new ArgumentException("Value cannot be null or empty.", "clientId");
		}
		return await GetClientResponse(accessToken, clientId).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<ClientGetModel[]> GetClients()
	{
		return await GetClients(await GetAdminTokenWithRegistrationScope().ConfigureAwait(continueOnCapturedContext: false)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<ClientGetModel[]> GetClients(string accessToken)
	{
		if (string.IsNullOrEmpty(accessToken))
		{
			throw new ArgumentException("Value cannot be null or empty.", "accessToken");
		}
		return await GetClientsResponse(accessToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<RegisterClientResponse> RegisterClient(string clientId, string clientName, IEnumerable<string> scopes)
	{
		if (string.IsNullOrEmpty(clientId))
		{
			throw new ArgumentException("Value cannot be null or empty.", "clientId");
		}
		if (scopes == null)
		{
			throw new ArgumentNullException("scopes");
		}
		return await RegisterClient(await GetAdminTokenWithRegistrationScope().ConfigureAwait(continueOnCapturedContext: false), clientId, clientName, scopes).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<RegisterClientResponse> RegisterClient(string accessToken, string clientId, string clientName, IEnumerable<string> scopes)
	{
		if (string.IsNullOrEmpty(accessToken))
		{
			throw new ArgumentException("Value cannot be null or empty.", "accessToken");
		}
		if (string.IsNullOrEmpty(clientId))
		{
			throw new ArgumentException("Value cannot be null or empty.", "clientId");
		}
		if (scopes == null)
		{
			throw new ArgumentNullException("scopes");
		}
		return await RegisterClientResponse(accessToken, clientId, clientName, scopes, new List<string> { "client_credentials" }, null).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<RegisterClientResponse> RegisterAuthorizationCodeClient(string clientId, string clientName, IEnumerable<string> scopes, IEnumerable<string> redirectUris)
	{
		if (string.IsNullOrEmpty(clientId))
		{
			throw new ArgumentException("Value cannot be null or empty.", "clientId");
		}
		if (redirectUris == null || !redirectUris.Any())
		{
			throw new ArgumentException("Value cannot be null or empty.", "redirectUris");
		}
		if (scopes == null)
		{
			throw new ArgumentNullException("scopes");
		}
		return await RegisterAuthorizationCodeClient(await GetAdminTokenWithRegistrationScope().ConfigureAwait(continueOnCapturedContext: false), clientId, clientName, scopes, redirectUris).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<RegisterClientResponse> RegisterAuthorizationCodeClient(string accessToken, string clientId, string clientName, IEnumerable<string> scopes, IEnumerable<string> redirectUris)
	{
		if (string.IsNullOrEmpty(accessToken))
		{
			throw new ArgumentException("Value cannot be null or empty.", "accessToken");
		}
		if (string.IsNullOrEmpty(clientId))
		{
			throw new ArgumentException("Value cannot be null or empty.", "clientId");
		}
		if (redirectUris == null || !redirectUris.Any())
		{
			throw new ArgumentException("Value cannot be null or empty.", "redirectUris");
		}
		if (scopes == null)
		{
			throw new ArgumentNullException("scopes");
		}
		return await RegisterClientResponse(accessToken, clientId, clientName, scopes, new List<string> { "authorization_code", "refresh_token" }, redirectUris).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<RegisterClientResponse> RegisterClientCredentialsAndAuthCodeClient(string clientId, string clientName, IEnumerable<string> scopes, IEnumerable<string> redirectUris, IEnumerable<string> postLogoutRedirectUris)
	{
		if (string.IsNullOrEmpty(clientId))
		{
			throw new ArgumentException("Value cannot be null or empty.", "clientId");
		}
		if (scopes == null)
		{
			throw new ArgumentNullException("scopes");
		}
		if (redirectUris == null || !redirectUris.Any())
		{
			throw new ArgumentException("Value cannot be null or empty.", "redirectUris");
		}
		if (postLogoutRedirectUris == null || !postLogoutRedirectUris.Any())
		{
			throw new ArgumentException("Value cannot be null or empty.", "postLogoutRedirectUris");
		}
		return await RegisterClientCredentialsAndAuthCodeClient(await GetAdminTokenWithRegistrationScope().ConfigureAwait(continueOnCapturedContext: false), clientId, clientName, scopes, redirectUris, postLogoutRedirectUris).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<RegisterClientResponse> RegisterClientCredentialsAndAuthCodeClient(string accessToken, string clientId, string clientName, IEnumerable<string> scopes, IEnumerable<string> redirectUris, IEnumerable<string> postLogoutRedirectUris)
	{
		if (string.IsNullOrEmpty(accessToken))
		{
			throw new ArgumentException("Value cannot be null or empty.", "accessToken");
		}
		if (string.IsNullOrEmpty(clientId))
		{
			throw new ArgumentException("Value cannot be null or empty.", "clientId");
		}
		if (scopes == null)
		{
			throw new ArgumentNullException("scopes");
		}
		if (redirectUris == null || !redirectUris.Any())
		{
			throw new ArgumentException("Value cannot be null or empty.", "redirectUris");
		}
		if (postLogoutRedirectUris == null || !postLogoutRedirectUris.Any())
		{
			throw new ArgumentException("Value cannot be null or empty.", "postLogoutRedirectUris");
		}
		return await RegisterClientResponse(accessToken, clientId, clientName, scopes, new List<string> { "client_credentials", "authorization_code" }, redirectUris, postLogoutRedirectUris).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<string> GetAdminTokenWithRegistrationScope()
	{
		HttpContent content = (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)new List<KeyValuePair<string, string>>
		{
			new KeyValuePair<string, string>("grant_type", "windows_auth"),
			new KeyValuePair<string, string>("scope", "write:client"),
			new KeyValuePair<string, string>("client_id", "winauthclient")
		});
		HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, BuildFullUri("/connect/token"))
		{
			Content = content
		};
		return (await SendAndReadContent<TokenResponse>(request).ConfigureAwait(continueOnCapturedContext: false)).Access_Token;
	}

	public async Task<string[]> GetAllValidScopes()
	{
		return (await SendAndReadContent<BaseResponse<List<string>>>(new HttpRequestMessage(HttpMethod.Get, BuildFullUri("api/scopes"))).ConfigureAwait(continueOnCapturedContext: false)).Data.ToArray();
	}

	private async Task<ClientGetModel> GetClientResponse(string accessToken, string clientId)
	{
		HttpRequestMessage val = new HttpRequestMessage(HttpMethod.Get, BuildFullUri("api/clients/" + clientId));
		val.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
		return await SendAndReadContent<ClientGetModel>(val).ConfigureAwait(continueOnCapturedContext: false);
	}

	private async Task<ClientGetModel[]> GetClientsResponse(string accessToken)
	{
		HttpRequestMessage val = new HttpRequestMessage(HttpMethod.Get, BuildFullUri("api/clients"));
		val.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
		return (await SendAndReadContent<BaseResponse<ClientGetModel[]>>(val).ConfigureAwait(continueOnCapturedContext: false)).Data;
	}

	private async Task<RegisterClientResponse> RegisterClientResponse(string accessToken, string clientId, string clientName, IEnumerable<string> scopes, IEnumerable<string> grantTypes, IEnumerable<string> redirectUris, IEnumerable<string> postLogoutRedirectUris = null)
	{
		HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, BuildFullUri("api/clients/" + clientId));
		try
		{
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
			request.Content = (HttpContent)new StringContent(JsonConvert.SerializeObject((object)new RegisterClientModel
			{
				ClientName = clientName,
				ClientScopes = scopes.ToArray(),
				ClientGrantTypes = grantTypes,
				ClientRedirectUris = redirectUris,
				PostLogoutRedirectUris = postLogoutRedirectUris
			}), Encoding.UTF8, "application/json");
			return (await SendAndReadContent<BaseResponse<RegisterClientResponse>>(request).ConfigureAwait(continueOnCapturedContext: false)).Data;
		}
		finally
		{
			((IDisposable)request)?.Dispose();
		}
	}
}
