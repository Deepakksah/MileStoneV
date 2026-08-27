using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.ServerConfigurator.RegistrationContainer;

public interface IClientCleanup : IRegistrationContainer
{
	StatusContainer Initialize(ILogger logger);

	StatusContainer CleanUp();
}
