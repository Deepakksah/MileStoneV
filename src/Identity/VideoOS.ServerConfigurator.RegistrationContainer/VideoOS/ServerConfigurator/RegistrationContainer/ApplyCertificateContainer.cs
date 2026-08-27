using System;
using System.Linq;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.ServerConfigurator.RegistrationContainer;

public class ApplyCertificateContainer : RegistrationContainerBase, IApplyCertificate, IServerCleanup, IRegistrationContainer
{
	private IWrappedApplyCertificate _wrappedRegistrar;

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
		if ((from x in base.RegistrarObject.GetType().GetInterfaces()
			select x.Name).Contains("IApplyCertificate"))
		{
			_wrappedRegistrar = new ApplyCertificateWrapperV1(base.RegistrarObject);
			return;
		}
		throw new InvalidCastException(FormattableString.Invariant($"Unable to wrap the type {base.RegistrarObject.GetType()} to a known interface"));
	}

	public StatusContainer Initialize(ILogger logger)
	{
		return _wrappedRegistrar.Initialize(logger).ToContainerStatus();
	}

	public StatusContainer PostApplyCertificate()
	{
		return _wrappedRegistrar.PostApplyCertificate().ToContainerStatus();
	}

	public StatusContainer PreApplyCertificate()
	{
		return _wrappedRegistrar.PreApplyCertificate().ToContainerStatus();
	}

	public StatusContainer ApplyCertificate(string thumbprint)
	{
		return _wrappedRegistrar.ApplyCertificate(thumbprint).ToContainerStatus();
	}
}
