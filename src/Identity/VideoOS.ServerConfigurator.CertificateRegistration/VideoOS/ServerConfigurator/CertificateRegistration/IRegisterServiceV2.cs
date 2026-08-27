using System;

namespace VideoOS.ServerConfigurator.CertificateRegistration;

public interface IRegisterServiceV2 : IServerCleanup
{
	Status PreRegisterService();

	Status RegisterThisService(Uri serverToRegisterTo, string hostToRegister, bool isSecure, Credential credentials);

	Status PostRegisterService();
}
