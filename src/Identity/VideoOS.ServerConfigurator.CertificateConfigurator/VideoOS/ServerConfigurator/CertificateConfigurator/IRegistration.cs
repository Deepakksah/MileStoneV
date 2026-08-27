using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.CertificateConfigurator;

public interface IRegistration<TResult>
{
	RegistrationResult Register();
}
