namespace VideoOS.GatewayService.InstallerPlugin.GatewayServiceInstallerUtils;

public interface IGatewayServiceRegistrationWrapper
{
	bool TryRegisterService(bool isUpgrade, bool createDatabase, string managementServerAddress, string gatewayServiceWebSiteId);

	bool TryUnregisterService(string managementServerAddress);
}
