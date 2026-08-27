using System;

namespace VideoOS.ServerConfigurator.CertificateConfigurator;

public interface IManagementServerHelper : IDisposable
{
	(bool? secureState, bool certificateError) IsSecure(Uri managementServerAddress, Uri alternativeAddress);
}
