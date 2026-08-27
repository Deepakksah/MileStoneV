using System;
using System.Collections.Generic;

namespace VideoOS.ServerConfigurator.Interfaces;

public interface INonvolatileClientRegistrationQueryModel
{
	IEnumerable<ClientEndpointConfiguration> ClientEndpointsInstalled { get; }

	IEnumerable<SystemConfigurationElement> ConfigurationElementsInstalled { get; }

	string HostName { get; }

	bool OverrideLocalManagementServer { get; }

	event EventHandler<EventArgs> ClientEndpointsModified;

	event EventHandler<EventArgs> ClientRegistrationServerModified;

	Uri GetManagementServer();

	Uri GetManagementServer(bool secureState);

	Uri GetAuthorizationServer();

	Uri GetAuthorizationServer(bool secureState);
}
