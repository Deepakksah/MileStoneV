using System;

namespace VideoOS.GatewayService.Utilities;

public interface IConfigFileHelper
{
	bool UpdateManagementServerUri(string gatewayConfigPath, Uri managementServerUri);

	bool UpdateDllSearchPath(string gatewayConfigPath, string dllPath);

	bool UpdateGatewayServiceUri(string gatewayConfigPath, Uri gatewayServiceUri);

	void SetClientRegistrationId(string appConfigFile, Guid id);

	Guid? GetClientRegistrationId(string appConfigPath);
}
