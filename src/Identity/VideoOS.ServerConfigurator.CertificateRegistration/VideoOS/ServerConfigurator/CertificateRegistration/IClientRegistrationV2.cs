using System;

namespace VideoOS.ServerConfigurator.CertificateRegistration;

public interface IClientRegistrationV2 : IClientCleanup
{
	Status PreRegisterClient();

	Status RegisterClient(Credential identityServerCredentials, Uri authorizationService);

	Status PostRegisterClient();
}
