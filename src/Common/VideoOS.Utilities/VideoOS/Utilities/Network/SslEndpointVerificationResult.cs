using System;

namespace VideoOS.Utilities.Network;

[CLSCompliant(true)]
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
