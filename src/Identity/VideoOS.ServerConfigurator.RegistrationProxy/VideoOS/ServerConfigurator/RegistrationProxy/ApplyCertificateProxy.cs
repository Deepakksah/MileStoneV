using System.Runtime.Remoting.Lifetime;
using VideoOS.ServerConfigurator.CertificateRegistration;
using VideoOS.ServerConfigurator.RegistrationContainer;

namespace VideoOS.ServerConfigurator.RegistrationProxy;

public class ApplyCertificateProxy : ProxyBase<VideoOS.ServerConfigurator.RegistrationContainer.IApplyCertificate, ApplyCertificateContainer>, VideoOS.ServerConfigurator.CertificateRegistration.IApplyCertificate, VideoOS.ServerConfigurator.CertificateRegistration.IServerCleanup
{
	public ApplyCertificateProxy(string assemblyQualifiedName, string assemblyLocation, ISponsor sponsor)
		: base(assemblyQualifiedName, assemblyLocation, sponsor)
	{
	}

	public Status ApplyCertificate(string thumbprint)
	{
		return base.Container.ApplyCertificate(thumbprint).FromContainerStatus();
	}

	public Status CleanUp()
	{
		return base.Container.CleanUp().FromContainerStatus();
	}

	public Status Initialize(ILogger logger)
	{
		return base.Container.Initialize(logger).FromContainerStatus();
	}

	public Status PostApplyCertificate()
	{
		return base.Container.PostApplyCertificate().FromContainerStatus();
	}

	public Status PreApplyCertificate()
	{
		return base.Container.PreApplyCertificate().FromContainerStatus();
	}
}
