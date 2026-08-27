using System;
using VideoOS.IdentityServer.Common.Utilities;

namespace VideoOS.IdentityServer.Common;

public interface IIdpServerInformation
{
	Uri UriValidated { get; }

	bool IsValid { get; }

	bool IsCurrentVersion { get; }

	ServerVersionCompareResult ServerVersionCompare { get; }

	bool IsOAuthSupported { get; }

	bool ValidCertificate { get; }

	bool UnsecureIdpAvailable { get; }

	bool SecureIdpAvailable { get; }

	bool UnsecureIdpValidated { get; }

	bool SecureIdpValidated { get; }

	IdpDiscoveryType RequestedIdpDiscoveryType { get; }

	bool ContainsCustomPort { get; }

	Uri ValidatedServerUri { get; }

	IIdpOpenConfiguration IdpConfiguration { get; }

	SslEndpointVerificationResult SecureSslVerification { get; }

	SslEndpointVerificationResult UnsecureSslVerification { get; }
}
