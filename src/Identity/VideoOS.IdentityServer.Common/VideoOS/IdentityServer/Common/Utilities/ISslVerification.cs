using System;

namespace VideoOS.IdentityServer.Common.Utilities;

public interface ISslVerification
{
	SslEndpointVerificationResult VerifyEndpointSupportsSsl(Uri uri);

	SslEndpointVerificationResult VerifyEndpointSupportsSsl(Uri uri, int timeoutInSeconds);
}
