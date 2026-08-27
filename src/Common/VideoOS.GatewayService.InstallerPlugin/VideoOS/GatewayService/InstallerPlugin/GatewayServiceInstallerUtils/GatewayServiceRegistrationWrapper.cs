namespace VideoOS.GatewayService.InstallerPlugin.GatewayServiceInstallerUtils;

public class GatewayServiceRegistrationWrapper : IGatewayServiceRegistrationWrapper
{
	public bool TryRegisterService(bool isUpgrade, bool createDatabase, string managementServerAddress, string gatewayServiceWebSiteId)
	{
		return new GatewayServiceRegistration().TryRegisterService(isUpgrade, createDatabase, managementServerAddress, gatewayServiceWebSiteId);
	}

	public bool TryUnregisterService(string managementServerAddress)
	{
		return new GatewayServiceRegistration().TryUnregisterService(managementServerAddress);
	}
}
