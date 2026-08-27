using System;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.Utilities;

namespace VideoOS.GatewayService.InstallerPlugin.GatewayServiceInstallerUtils;

public class GatewayServerHelper : ServerHelper, IGatewayServerHelper
{
	bool IGatewayServerHelper.TryGetManagementServerInfo(Uri managementServerAddress, out ManagementServerInfo managementServerInfo)
	{
		return TryGetManagementServerInfo(managementServerAddress, out managementServerInfo);
	}
}
