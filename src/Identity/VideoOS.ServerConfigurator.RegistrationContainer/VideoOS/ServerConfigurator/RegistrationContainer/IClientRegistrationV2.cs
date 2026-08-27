using System;

namespace VideoOS.ServerConfigurator.RegistrationContainer;

public interface IClientRegistrationV2 : IClientCleanup, IRegistrationContainer
{
	StatusContainer PreRegisterClient();

	StatusContainer RegisterClient(CredentialContainer identityServerCredentials, Uri authorizationService);

	StatusContainer PostRegisterClient();
}
