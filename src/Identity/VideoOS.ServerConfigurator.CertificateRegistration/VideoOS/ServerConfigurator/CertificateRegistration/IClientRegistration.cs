using System;
using System.Net;

namespace VideoOS.ServerConfigurator.CertificateRegistration;

[Obsolete("This interface is for compatibility with ealier versions of components")]
public interface IClientRegistration : IClientCleanup
{
	Status PreRegisterClient();

	Status RegisterClient(NetworkCredential identityServerCredentials, Uri clientRegistrationService);

	Status PostRegisterClient();
}
