namespace VideoOS.ServerConfigurator.CertificateRegistration;

public interface ISystemConfigurationCleanUp
{
	Status Initialize(ILogger logger);

	Status CleanUp();
}
