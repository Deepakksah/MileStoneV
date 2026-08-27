using System;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.Server.CertificateRegistration;

public interface IRegistrationIisManagementService
{
	void CleanupIisManagementService();

	void CleanupReportService();

	void RegisterIisManagementService(string iisConfigFilePath, Uri authorizationServerUri, ILogger logger);

	void RegisterReportService(string reportConfigFilePath, Uri authorizationServerUri, ILogger logger);
}
