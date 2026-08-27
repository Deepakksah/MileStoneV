namespace VideoOS.GatewayService.Utilities;

public interface IVirtualDirectoryHelper
{
	IVirtualDirectoryWrapper CreateVirtualDirectory(string websiteId, string appPoolId, string virtualApplicationPath, string physicalPath);

	void DeleteVirtualDirectory(string websiteId, string webPath);
}
