namespace VideoOS.ServerConfigurator.CertificateRegistration;

public interface IClientCleanup
{
	Status Initialize(ILogger logger);

	Status CleanUp();
}
