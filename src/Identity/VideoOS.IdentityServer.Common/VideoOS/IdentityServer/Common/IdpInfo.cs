using System;

namespace VideoOS.IdentityServer.Common;

public class IdpInfo
{
	public IIdpOpenConfiguration IdpConfiguration { get; set; }

	public bool IsOAuthSupported => !string.IsNullOrEmpty(IdpConfiguration?.ServerVersion);

	public Uri Address { get; set; }
}
