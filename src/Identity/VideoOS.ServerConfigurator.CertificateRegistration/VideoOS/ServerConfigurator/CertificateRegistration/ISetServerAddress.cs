using System;

namespace VideoOS.ServerConfigurator.CertificateRegistration;

public interface ISetServerAddress : IClientCleanup
{
	Status PreSetServerAddress();

	Status SetServerAddress(Uri newManagementServerAddress);

	Status PostSetServerAddress();
}
