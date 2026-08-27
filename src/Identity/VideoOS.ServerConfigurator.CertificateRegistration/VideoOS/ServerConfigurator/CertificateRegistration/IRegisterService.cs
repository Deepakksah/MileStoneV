using System;

namespace VideoOS.ServerConfigurator.CertificateRegistration;

public interface IRegisterService : IServerCleanup
{
	Status PreRegisterService();

	Status RegisterThisService(Uri serverToRegisterTo, bool isSecure, Credential credentials);

	Status PostRegisterService();
}
