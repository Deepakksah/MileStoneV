using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.ServerConfigurator.RegistrationContainer;

public class ApplyCertificateWrapperV1 : IWrappedApplyCertificate
{
	private VideoOS.ServerConfigurator.CertificateRegistration.IApplyCertificate _realRegistrar { get; }

	public ApplyCertificateWrapperV1(object inner)
	{
		_realRegistrar = (VideoOS.ServerConfigurator.CertificateRegistration.IApplyCertificate)inner;
	}

	public Status CleanUp()
	{
		return _realRegistrar.CleanUp();
	}

	public Status Initialize(ILogger logger)
	{
		return _realRegistrar.Initialize(logger);
	}

	public Status PostApplyCertificate()
	{
		return _realRegistrar.PostApplyCertificate();
	}

	public Status ApplyCertificate(string thumbprint)
	{
		return _realRegistrar.ApplyCertificate(thumbprint);
	}

	public Status PreApplyCertificate()
	{
		return _realRegistrar.PreApplyCertificate();
	}
}
