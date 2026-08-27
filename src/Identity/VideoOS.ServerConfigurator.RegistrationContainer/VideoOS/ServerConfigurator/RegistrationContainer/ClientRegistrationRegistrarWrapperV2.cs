using System;
using System.Net;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.ServerConfigurator.RegistrationContainer;

public class ClientRegistrationRegistrarWrapperV2 : IWrappedClientRegistration
{
	private VideoOS.ServerConfigurator.CertificateRegistration.IClientRegistrationV2 _realRegistrar;

	public ClientRegistrationRegistrarWrapperV2(object realRegistrar)
	{
		_realRegistrar = (VideoOS.ServerConfigurator.CertificateRegistration.IClientRegistrationV2)realRegistrar;
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
		Credential identityServerCredentials2 = (NetworkCredential)identityServerCredentials;
		return _realRegistrar.RegisterClient(identityServerCredentials2, authorizationService);
	}

	public Status PostRegisterClient()
	{
		return _realRegistrar.PostRegisterClient();
	}
}
