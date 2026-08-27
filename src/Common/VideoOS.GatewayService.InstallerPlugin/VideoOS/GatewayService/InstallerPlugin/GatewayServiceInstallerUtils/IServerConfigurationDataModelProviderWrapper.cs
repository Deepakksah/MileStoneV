using System;
using System.Collections.Generic;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.GatewayService.InstallerPlugin.GatewayServiceInstallerUtils;

public interface IServerConfigurationDataModelProviderWrapper
{
	void SetLocationOnEndpointToBeInstalled(Guid endpointId, string location);

	ApplyStatus ApplyCertificatesForEndpointsToBeInstalled(IEnumerable<Guid> serverEndpointIds);

	RegistrationResult ConfigureClientEndpointsToBeInstalled(IEnumerable<Guid> clientEndpointIds, bool offlineRegistration);
}
