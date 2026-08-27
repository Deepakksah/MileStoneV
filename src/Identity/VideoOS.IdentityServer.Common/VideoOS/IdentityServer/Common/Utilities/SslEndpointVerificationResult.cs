namespace VideoOS.IdentityServer.Common.Utilities;

public enum SslEndpointVerificationResult
{
	NotVerified,
	TimedOut,
	InvalidHostName,
	SslNotSupported,
	SslSupported,
	SslSupportedInvalidCertificate,
	EndpointNotFound,
	UnknownResponse
}
