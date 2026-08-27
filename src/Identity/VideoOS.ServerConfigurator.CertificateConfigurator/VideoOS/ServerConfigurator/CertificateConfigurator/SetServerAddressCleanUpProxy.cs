using System;
using System.Collections.Generic;
using VideoOS.ServerConfigurator.CertificateRegistration;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.CertificateConfigurator;

public class SetServerAddressCleanUpProxy : BaseCleanupProxy<ClientCleanUpUnwrap, IClientCleanup>
{
	public static Dictionary<Guid, IClientCleanup> GetEndpointInterfaces(IEnumerable<ClientEndpointConfiguration> endpoints, VideoOS.ServerConfigurator.Interfaces.ILogger logger, VideoOS.ServerConfigurator.CertificateRegistration.ILogger registrationLogger, bool contiueOnError, out StatusCode result)
	{
		return BaseCleanupProxy<ClientCleanUpUnwrap, IClientCleanup>.GetEndpointInterfacesBase(endpoints, (ClientEndpointConfiguration x) => x.ServerAddressRegistrar, logger, registrationLogger, contiueOnError, out result);
	}
}
