using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.ServerConfigurator.CertificateConfigurator;

public interface IUnwrap
{
	void SetInnerItem(IWrapperCleanup innerItem);

	Status Initialize(ILogger logger);
}
