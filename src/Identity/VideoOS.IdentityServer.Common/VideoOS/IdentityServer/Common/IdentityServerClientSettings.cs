using System;

namespace VideoOS.IdentityServer.Common;

public sealed class IdentityServerClientSettings : IIdentityServerClientSettings
{
	public const string ProviderOidc = "OIDC";

	public const string GrantTypeWindows = "windows_credentials";

	public const string GrantTypeBasicUser = "password";

	public const string GrantTypeRefreshToken = "refresh_token";

	public const string GrantTypeSecret = "client_credentials";

	public const string GrantTypeAuthCode = "authorization_code";

	public Uri BaseServerAddress { get; }

	public string ClientId { get; }

	public string GrantType { get; }

	public IdentityServerClientSettings(Uri baseServerAddress, string clientId)
	{
		if (baseServerAddress == null)
		{
			throw new ArgumentNullException("baseServerAddress");
		}
		if (!baseServerAddress.IsAbsoluteUri)
		{
			throw new ArgumentException($"{baseServerAddress} can not be a relative uri", "baseServerAddress");
		}
		if (string.IsNullOrEmpty(clientId))
		{
			throw new ArgumentNullException("clientId");
		}
		BaseServerAddress = baseServerAddress;
		ClientId = clientId;
	}

	public IdentityServerClientSettings(Uri baseServerAddress, string clientId, string grantType)
		: this(baseServerAddress, clientId)
	{
		if (string.IsNullOrEmpty(grantType))
		{
			throw new ArgumentNullException("grantType");
		}
		GrantType = grantType;
	}
}
