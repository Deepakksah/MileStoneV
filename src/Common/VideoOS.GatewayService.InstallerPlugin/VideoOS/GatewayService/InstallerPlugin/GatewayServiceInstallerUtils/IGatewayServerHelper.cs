using System;
using VideoOS.Installer.Common;

namespace VideoOS.GatewayService.InstallerPlugin.GatewayServiceInstallerUtils;

public interface IGatewayServerHelper
{
	bool TryGetManagementServerInfo(Uri managementServerAddress, out ManagementServerInfo managementServerInfo);
}
