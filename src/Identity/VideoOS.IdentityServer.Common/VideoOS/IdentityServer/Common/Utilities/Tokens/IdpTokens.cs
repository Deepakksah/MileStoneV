using System;

namespace VideoOS.IdentityServer.Common.Utilities.Tokens;

public sealed class IdpTokens : IIdpTokens
{
	public string RefreshToken { get; }

	public string AccessToken { get; }

	public string IdToken { get; }

	public IdpTokens(string refreshToken, string accessToken, string idToken)
	{
		if (string.IsNullOrEmpty(refreshToken))
		{
			throw new ArgumentNullException("refreshToken");
		}
		if (string.IsNullOrEmpty(accessToken))
		{
			throw new ArgumentNullException("accessToken");
		}
		if (string.IsNullOrEmpty(idToken))
		{
			throw new ArgumentNullException("idToken");
		}
		RefreshToken = refreshToken;
		AccessToken = accessToken;
		IdToken = idToken;
	}
}
