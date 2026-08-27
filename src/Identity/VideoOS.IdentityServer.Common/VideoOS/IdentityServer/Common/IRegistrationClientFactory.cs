using System;
using System.Net;

namespace VideoOS.IdentityServer.Common;

public interface IRegistrationClientFactory
{
	IRegistrationClient Create(Uri baseAddress);

	IRegistrationClient Create(Uri baseAddress, ICredentials credentials);
}
