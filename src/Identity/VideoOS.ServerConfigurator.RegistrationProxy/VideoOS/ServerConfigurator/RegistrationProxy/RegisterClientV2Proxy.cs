using System;
using System.Net;
using System.Runtime.Remoting.Lifetime;
using VideoOS.ServerConfigurator.CertificateRegistration;
using VideoOS.ServerConfigurator.RegistrationContainer;

namespace VideoOS.ServerConfigurator.RegistrationProxy;

public class RegisterClientV2Proxy : ProxyBase<VideoOS.ServerConfigurator.RegistrationContainer.IClientRegistrationV2, ClientRegistrationContainer>, VideoOS.ServerConfigurator.CertificateRegistration.IClientRegistrationV2, VideoOS.ServerConfigurator.CertificateRegistration.IClientCleanup
{
	public RegisterClientV2Proxy(string assemblyQualifiedName, string assemblyLocation, ISponsor sponsor)
		: base(assemblyQualifiedName, assemblyLocation, sponsor)
	{
	}

	public Status CleanUp()
	{
		return base.Container.CleanUp().FromContainerStatus();
	}

	public Status Initialize(ILogger logger)
	{
		return base.Container.Initialize(logger).FromContainerStatus();
	}

	public Status PostRegisterClient()
	{
		return base.Container.PostRegisterClient().FromContainerStatus();
	}

	public Status PreRegisterClient()
	{
		return base.Container.PreRegisterClient().FromContainerStatus();
	}

	public Status RegisterClient(Credential identityServerCredentials, Uri authorizationService)
	{
		return base.Container.RegisterClient((NetworkCredential)identityServerCredentials, authorizationService).FromContainerStatus();
	}
}
