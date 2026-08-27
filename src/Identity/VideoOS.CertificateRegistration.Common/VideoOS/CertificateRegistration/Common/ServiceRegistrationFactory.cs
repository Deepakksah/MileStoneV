using System;
using System.Net;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.CertificateRegistration.Common;

public class ServiceRegistrationFactory : IServiceRegistrationFactory
{
	public IServiceRegistration Create(Uri managementServerAddress, Uri unsecureManagementServerAddress, Uri identityProvider, NetworkCredential networkCredential, ILogger logger)
	{
		return new ServiceRegistration(managementServerAddress, unsecureManagementServerAddress, identityProvider, networkCredential, logger);
	}
}
