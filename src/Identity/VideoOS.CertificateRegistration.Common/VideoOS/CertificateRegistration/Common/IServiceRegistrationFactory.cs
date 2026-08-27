using System;
using System.Net;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.CertificateRegistration.Common;

public interface IServiceRegistrationFactory
{
	IServiceRegistration Create(Uri managementServerAddress, Uri unsecureManagementServerAddress, Uri identityProvider, NetworkCredential networkCredential, ILogger logger);
}
