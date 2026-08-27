using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Common.EnvironmentServices.Web;

namespace VideoOS.IdentityServer.Common;

public class IdpTokenClient : IdpClientBase
{
	private readonly IIdentityServerClientSettings _identityServerClientSettings;

	public IdpTokenClient(IIdentityServerClientSettings identityServerClientSettings)
		: this((ICredentials)null, identityServerClientSettings)
	{
	}

	public IdpTokenClient(ICredentials credentials, IIdentityServerClientSettings identityServerClientSettings)
		: this(credentials, identityServerClientSettings, new HttpClientFactory())
	{
	}

	public IdpTokenClient(ICredentials credentials, IIdentityServerClientSettings identityServerClientSettings, IHttpClientFactory httpClientFactory)
		: this(httpClientFactory.Create((HttpMessageHandler)(object)CreateClientHandler(credentials)), identityServerClientSettings)
	{
	}

	public IdpTokenClient(IHttpClient client, IIdentityServerClientSettings identityServerClientSettings)
		: base(identityServerClientSettings.BaseServerAddress, client)
	{
		_identityServerClientSettings = identityServerClientSettings ?? throw new ArgumentNullException("identityServerClientSettings");
	}

	private static HttpClientHandler CreateClientHandler(ICredentials credentials = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		return new HttpClientHandler
		{
			Credentials = credentials,
			UseDefaultCredentials = (credentials == null || credentials == CredentialCache.DefaultCredentials)
		};
	}

	public Task<TokenResponse> ClientCredentialsAuthenticationAsync(string secret, CancellationToken cancellationToken)
	{
		return ClientCredentialsAuthenticationAsync(secret, null, cancellationToken);
	}

	public Task<TokenResponse> ClientCredentialsAuthenticationAsync(string secret, string scope)
	{
		return ClientCredentialsAuthenticationAsync(secret, scope, CancellationToken.None);
	}

	public Task<TokenResponse> ClientCredentialsAuthenticationAsync(string secret, string scope, CancellationToken cancellationToken)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		if (string.IsNullOrEmpty(secret))
		{
			throw new ArgumentException("Value cannot be null or empty.", "secret");
		}
		HttpRequestMessage val = new HttpRequestMessage(HttpMethod.Post, BuildFullUri("connect/token"));
		List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>
		{
			new KeyValuePair<string, string>("grant_type", "client_credentials")
		};
		if (scope != null)
		{
			list.Add(new KeyValuePair<string, string>("scope", scope));
		}
		val.Content = (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)list);
		val.Headers.Authorization = (AuthenticationHeaderValue)(object)new BasicOpenAuthenticationHeaderValue(_identityServerClientSettings.ClientId, secret);
		return SendAndReadContent<TokenResponse>(val, cancellationToken);
	}

	public Task<TokenResponse> WindowsAuthenticationAsync()
	{
		return WindowsAuthenticationAsync(CancellationToken.None);
	}

	public Task<TokenResponse> WindowsAuthenticationAsync(CancellationToken cancellationToken)
	{
		return WindowsAuthenticationAsync(null, cancellationToken);
	}

	public Task<TokenResponse> WindowsAuthenticationAsync(string scope)
	{
		return WindowsAuthenticationAsync(scope, CancellationToken.None);
	}

	public Task<TokenResponse> WindowsAuthenticationAsync(string scope, CancellationToken cancellationToken)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>
		{
			new KeyValuePair<string, string>("grant_type", "windows_credentials"),
			new KeyValuePair<string, string>("client_id", _identityServerClientSettings.ClientId)
		};
		if (!string.IsNullOrEmpty(scope))
		{
			list.Add(new KeyValuePair<string, string>("scope", scope));
		}
		HttpContent content = (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)list);
		HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, BuildFullUri("/connect/token"))
		{
			Content = content
		};
		return SendAndReadContent<TokenResponse>(request, cancellationToken);
	}

	public Task<TokenResponse> ResourceOwnerAuthenticationAsync(string username, string password)
	{
		return ResourceOwnerAuthenticationAsync(username, password, CancellationToken.None);
	}

	public Task<TokenResponse> ResourceOwnerAuthenticationAsync(string username, string password, CancellationToken cancellationToken)
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Expected O, but got Unknown
		List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>
		{
			new KeyValuePair<string, string>("grant_type", "password"),
			new KeyValuePair<string, string>("username", username),
			new KeyValuePair<string, string>("password", password),
			new KeyValuePair<string, string>("client_id", _identityServerClientSettings.ClientId)
		};
		HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, BuildFullUri("/connect/token"))
		{
			Content = (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)list)
		};
		return SendAndReadContent<TokenResponse>(request, cancellationToken);
	}

	public Task<TokenResponse> RefreshTokenRequestAsync(string jwtRefreshToken)
	{
		return RefreshTokenRequestAsync(jwtRefreshToken, CancellationToken.None);
	}

	public Task<TokenResponse> RefreshTokenRequestAsync(string jwtRefreshToken, CancellationToken cancellationToken)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0069: Expected O, but got Unknown
		List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>
		{
			new KeyValuePair<string, string>("grant_type", "refresh_token"),
			new KeyValuePair<string, string>("refresh_token", jwtRefreshToken),
			new KeyValuePair<string, string>("client_id", _identityServerClientSettings.ClientId)
		};
		HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, BuildFullUri("/connect/token"))
		{
			Content = (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)list)
		};
		return SendAndReadContent<TokenResponse>(request, cancellationToken);
	}

	public Task<TokenResponse> ExchangeCodeForTokensAsync(string code, string codeVerifier, string redirectUri, string scope)
	{
		return ExchangeCodeForTokensAsync(code, codeVerifier, redirectUri, scope, CancellationToken.None);
	}

	public Task<TokenResponse> ExchangeCodeForTokensAsync(string code, string codeVerifier, string redirectUri, string scope, CancellationToken cancellationToken)
	{
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		//IL_009d: Expected O, but got Unknown
		List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>
		{
			new KeyValuePair<string, string>("grant_type", "authorization_code"),
			new KeyValuePair<string, string>("code", code),
			new KeyValuePair<string, string>("code_verifier", codeVerifier),
			new KeyValuePair<string, string>("redirect_uri", redirectUri),
			new KeyValuePair<string, string>("scope", scope),
			new KeyValuePair<string, string>("client_id", _identityServerClientSettings.ClientId)
		};
		HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, BuildFullUri("/connect/token"))
		{
			Content = (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)list)
		};
		return SendAndReadContent<TokenResponse>(request, cancellationToken);
	}

	public Task<TokenResponse> ExchangeTrustedAccessTokenForTokensAsync(string trustedJwtAccessToken, string scope)
	{
		return ExchangeTrustedAccessTokenForTokensAsync(trustedJwtAccessToken, scope, CancellationToken.None);
	}

	public Task<TokenResponse> ExchangeTrustedAccessTokenForTokensAsync(string trustedJwtAccessToken, string scope, CancellationToken cancellationToken)
	{
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		//IL_00a4: Expected O, but got Unknown
		List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>
		{
			new KeyValuePair<string, string>("grant_type", "urn:ietf:params:oauth:grant-type:token-exchange"),
			new KeyValuePair<string, string>("subject_token", trustedJwtAccessToken),
			new KeyValuePair<string, string>("subject_token_type", "urn:ietf:params:oauth:token-type:access_token"),
			new KeyValuePair<string, string>("exchange_style", "vms_federation"),
			new KeyValuePair<string, string>("scope", scope),
			new KeyValuePair<string, string>("client_id", _identityServerClientSettings.ClientId)
		};
		HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, BuildFullUri("/connect/token"))
		{
			Content = (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)list)
		};
		return SendAndReadContent<TokenResponse>(request, cancellationToken);
	}
}
