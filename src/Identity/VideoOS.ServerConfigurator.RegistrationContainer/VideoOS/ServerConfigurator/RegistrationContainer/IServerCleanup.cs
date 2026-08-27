using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.ServerConfigurator.RegistrationContainer;

public interface IServerCleanup : IRegistrationContainer
{
	StatusContainer Initialize(ILogger logger);

	StatusContainer CleanUp();
}
