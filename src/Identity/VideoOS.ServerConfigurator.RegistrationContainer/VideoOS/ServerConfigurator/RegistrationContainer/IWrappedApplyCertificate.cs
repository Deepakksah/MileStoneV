using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.ServerConfigurator.RegistrationContainer;

public interface IWrappedApplyCertificate
{
	Status Initialize(ILogger logger);

	Status PreApplyCertificate();

	Status ApplyCertificate(string thumbprint);

	Status PostApplyCertificate();

	Status CleanUp();
}
