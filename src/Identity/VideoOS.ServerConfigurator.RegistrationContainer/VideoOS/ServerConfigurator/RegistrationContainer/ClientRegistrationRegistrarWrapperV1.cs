using System;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.ServerConfigurator.RegistrationContainer;

public class ClientRegistrationRegistrarWrapperV1 : IWrappedClientRegistration
{
	private IClientRegistration _realRegistrar;

	public ClientRegistrationRegistrarWrapperV1(object realRegistrar)
	{
		_realRegistrar = (IClientRegistration)realRegistrar;
	}

	public Status CleanUp()
	{
		return _realRegistrar.CleanUp();
	}

	public Status Initialize(ILogger logger)
	{
		return _realRegistrar.Initialize(logger);
	}

	public Status PreRegisterClient()
	{
		return _realRegistrar.PreRegisterClient();
	}

	public Status RegisterClient(CredentialContainer identityServerCredentials, Uri authorizationService)
	{
		return _realRegistrar.RegisterClient(identityServerCredentials, authorizationService);
	}

	public Status PostRegisterClient()
	{
		return _realRegistrar.PostRegisterClient();
	}
}
