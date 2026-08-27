using System;
using System.Collections.Generic;
using VideoOS.Installer.ServerConfigurator;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.GatewayService.InstallerPlugin.GatewayServiceInstallerUtils;

public class ServerConfigurationDataModelProviderWrapper : IServerConfigurationDataModelProviderWrapper
{
	public ApplyStatus ApplyCertificatesForEndpointsToBeInstalled(IEnumerable<Guid> serverEndpointIds)
	{
		return ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.ApplyCertificatesForEndpointsToBeInstalled(serverEndpointIds);
	}

	public RegistrationResult ConfigureClientEndpointsToBeInstalled(IEnumerable<Guid> clientEndpointIds, bool offlineRegistration)
	{
		return ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.ConfigureClientEndpointsToBeInstalled(clientEndpointIds, offlineRegistration);
	}

	public void SetLocationOnEndpointToBeInstalled(Guid endpointId, string location)
	{
		ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.SetLocationOnEndpointToBeInstalled(endpointId, location);
	}
}
