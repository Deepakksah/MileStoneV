using System;

namespace VideoOS.ServerConfigurator.CertificateRegistration;

public interface IRegisterServiceV3 : IServerCleanup
{
	Status PreRegisterService();

	Status RegisterThisService(Uri serverToRegisterTo, Uri unsecureServerToRegisterTo, Uri identityProvider, string hostToRegister, bool isSecure, Credential credentials);

	Status PostRegisterService();
}
