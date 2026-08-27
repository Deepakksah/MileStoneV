using System;

namespace VideoOS.ServerConfigurator.CertificateRegistration;

public interface ISystemConfiguration : ISystemConfigurationCleanUp
{
	Status PreConfigureSystem();

	Status ConfigureSystem(string oldHostName, Uri configurationAddress, Uri communicationAddress, Uri authorizationServer, Credential credentials);

	Status PostConfigureSystem();
}
