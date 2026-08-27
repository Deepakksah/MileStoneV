using System;
using System.Net;

namespace VideoOS.IdentityServer.Common;

public class RegistrationClientFactory : IRegistrationClientFactory
{
	public IRegistrationClient Create(Uri baseAddress)
	{
		return new IdpRegistrationClient(baseAddress);
	}

	public IRegistrationClient Create(Uri baseAddress, ICredentials credentials)
	{
		return new IdpRegistrationClient(baseAddress, credentials);
	}
}
