namespace VideoOS.IdentityServer.Common;

public interface IIdpOpenConfiguration
{
	string Issuer { get; }

	string JwksUri { get; }

	string AuthorizationEndpoint { get; }

	string TokenEndpoint { get; }

	string UserInfoEndpoint { get; }

	string EndSessionEndpoint { get; }

	string CheckSessionIframe { get; }

	string RevocationEndpoint { get; }

	string IntrospectionEndpoint { get; }

	string DeviceAuthorizationEndpoint { get; }

	bool FrontChannelLogoutSupported { get; }

	bool FrontchannelLogoutSessionSupported { get; }

	bool BackchannelLogoutSupported { get; }

	bool BackchannelLogoutSessionSupported { get; }

	string[] ScopesSupported { get; }

	string[] ClaimsSupported { get; }

	string[] GrantTypesSupported { get; }

	string[] ResponseTypesSupported { get; }

	string[] ResponseModesSupported { get; }

	string[] TokenEndpointAuthMethodsSupported { get; }

	string[] IdTokenSigningAlgValuesSupported { get; }

	string[] SubjectTypesSupported { get; }

	string[] CodeChallengeMethodsSupported { get; }

	bool RequestParameterSupported { get; }

	string ServerVersion { get; }
}
