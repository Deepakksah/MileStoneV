using System;
using System.Collections.Generic;
using VideoOS.ServerConfigurator.CertificateRegistration;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.CertificateConfigurator;

public class RegisterServiceCleanUpProxy : BaseCleanupProxy<ServerCleanUpUnwrap, IServerCleanup>
{
	public static Dictionary<Guid, IServerCleanup> GetEndpointInterfaces(IEnumerable<ServerEndpointConfiguration> endpoints, VideoOS.ServerConfigurator.Interfaces.ILogger logger, VideoOS.ServerConfigurator.CertificateRegistration.ILogger registrationLogger, bool contiueOnError, out StatusCode result)
	{
		return BaseCleanupProxy<ServerCleanUpUnwrap, IServerCleanup>.GetEndpointInterfacesBase(endpoints, (ServerEndpointConfiguration x) => x.ThisServerRegistrar, logger, registrationLogger, contiueOnError, out result);
	}
}
