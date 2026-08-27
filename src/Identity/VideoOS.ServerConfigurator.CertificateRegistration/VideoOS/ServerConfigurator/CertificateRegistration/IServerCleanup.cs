namespace VideoOS.ServerConfigurator.CertificateRegistration;

public interface IServerCleanup
{
	Status Initialize(ILogger logger);

	Status CleanUp();
}
