using System;
using VideoOS.Server.Admin.Utilities;

namespace VideoOS.GatewayService.InstallerPlugin;

public interface ISetupIIS
{
	void Install(string virtualDirectoryPhysicalPath, string websiteId, string websiteAddress, AccountInfo serviceAccount, Action registerGatewayServiceAction);

	void Uninstall(string websiteId);

	void SetupSecureBinding(string websiteId, string managementUri);
}
