using System;
using System.Collections.Generic;
using VideoOS.ServerConfigurator.CertificateRegistration;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.CertificateConfigurator;

public class SystemConfigurationCleanupProxy : BaseCleanupProxy<SystemConfigurationCleanUpUnwrap, ISystemConfigurationCleanUp>
{
	public static Dictionary<Guid, ISystemConfigurationCleanUp> GetEndpointInterfaces(IEnumerable<SystemConfigurationElement> configurationElements, VideoOS.ServerConfigurator.Interfaces.ILogger logger, VideoOS.ServerConfigurator.CertificateRegistration.ILogger registrationLogger, bool contiueOnError, out StatusCode result)
	{
		return BaseCleanupProxy<SystemConfigurationCleanUpUnwrap, ISystemConfigurationCleanUp>.GetEndpointInterfacesBase(configurationElements, (SystemConfigurationElement x) => x.ConfigurationImplementation, logger, registrationLogger, contiueOnError, out result);
	}
}
