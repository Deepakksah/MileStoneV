using System.Collections.Generic;

namespace VideoOS.ServerConfigurator.Interfaces;

public interface IClientRegistrationQueryModel : INonvolatileClientRegistrationQueryModel
{
	IEnumerable<ClientEndpointConfiguration> ClientEndpointsToBeInstalled { get; }

	IEnumerable<SystemConfigurationElement> ConfigurationElementsToBeInstalled { get; }

	bool UseLocalManagementServer();

	bool IsLocalManagementServerPresent();

	bool? GetSecureState();

	bool? GetServerGroupSecureState();
}
