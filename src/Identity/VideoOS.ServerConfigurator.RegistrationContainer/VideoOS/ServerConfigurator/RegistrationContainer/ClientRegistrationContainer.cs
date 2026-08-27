using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.ServerConfigurator.RegistrationContainer;

public class ClientRegistrationContainer : RegistrationContainerBase, IClientRegistrationV2, IClientCleanup, IRegistrationContainer
{
	private IWrappedClientRegistration _wrappedRegistrar;

	public StatusContainer CleanUp()
	{
		return Status.SuccessStatus().ToContainerStatus();
	}

	public override void CreateInstance(string assemblyLocation, string assemblyQualifiedName)
	{
		base.CreateInstance(assemblyLocation, assemblyQualifiedName);
		EnsureCompatibility();
	}

	private void EnsureCompatibility()
	{
		IEnumerable<string> source = from x in base.RegistrarObject.GetType().GetInterfaces()
			select x.Name;
		if (source.Contains("IClientRegistrationV2"))
		{
			_wrappedRegistrar = new ClientRegistrationRegistrarWrapperV2(base.RegistrarObject);
			return;
		}
		if (source.Contains("IClientRegistration"))
		{
			_wrappedRegistrar = new ClientRegistrationRegistrarWrapperV1(base.RegistrarObject);
			return;
		}
		throw new InvalidCastException(FormattableString.Invariant($"Unable to wrap the type {base.RegistrarObject.GetType()} to a known interface"));
	}

	public StatusContainer Initialize(ILogger logger)
	{
		return _wrappedRegistrar.Initialize(logger).ToContainerStatus();
	}

	public StatusContainer PreRegisterClient()
	{
		return _wrappedRegistrar.PreRegisterClient().ToContainerStatus();
	}

	public StatusContainer RegisterClient(CredentialContainer identityServerCredentials, Uri authorizationService)
	{
		return _wrappedRegistrar.RegisterClient(identityServerCredentials, authorizationService).ToContainerStatus();
	}

	public StatusContainer PostRegisterClient()
	{
		return _wrappedRegistrar.PostRegisterClient().ToContainerStatus();
	}
}
