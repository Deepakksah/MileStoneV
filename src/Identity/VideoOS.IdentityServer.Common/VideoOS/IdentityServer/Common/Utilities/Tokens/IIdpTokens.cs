namespace VideoOS.IdentityServer.Common.Utilities.Tokens;

public interface IIdpTokens
{
	string RefreshToken { get; }

	string AccessToken { get; }

	string IdToken { get; }
}
