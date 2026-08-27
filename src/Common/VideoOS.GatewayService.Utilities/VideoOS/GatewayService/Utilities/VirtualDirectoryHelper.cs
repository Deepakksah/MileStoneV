using VideoOS.Server.Admin.Utilities;

namespace VideoOS.GatewayService.Utilities;

public class VirtualDirectoryHelper : IVirtualDirectoryHelper
{
	public IVirtualDirectoryWrapper CreateVirtualDirectory(string websiteId, string appPoolId, string virtualApplicationPath, string physicalPath)
	{
		return new VirtualDirectoryWrapper(new VirtualDirectory(websiteId, appPoolId, virtualApplicationPath, physicalPath));
	}

	public void DeleteVirtualDirectory(string websiteId, string webPath)
	{
		VirtualDirectory.DeleteVirtualDirectory(websiteId, webPath);
	}
}
