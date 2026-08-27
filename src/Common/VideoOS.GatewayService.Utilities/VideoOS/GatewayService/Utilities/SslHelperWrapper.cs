using VideoOS.Server.Admin.Utilities;

namespace VideoOS.GatewayService.Utilities;

public class SslHelperWrapper : ISslHelperWrapper
{
	public void DisableSsl(string websiteId, string virtualDirectoryName)
	{
		SslHelper.DisableSsl(websiteId, virtualDirectoryName);
	}

	public void EnableSsl(string websiteId, string virtualDirectoryName)
	{
		SslHelper.EnableSsl(websiteId, virtualDirectoryName);
	}
}
