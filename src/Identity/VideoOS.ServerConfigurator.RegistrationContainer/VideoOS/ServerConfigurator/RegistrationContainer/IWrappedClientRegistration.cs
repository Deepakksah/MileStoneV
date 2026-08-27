using System;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.ServerConfigurator.RegistrationContainer;

public interface IWrappedClientRegistration
{
	Status Initialize(ILogger logger);

	Status PreRegisterClient();

	Status RegisterClient(CredentialContainer identityServerCredentials, Uri authorizationService);

	Status PostRegisterClient();

	Status CleanUp();
}
