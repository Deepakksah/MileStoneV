namespace VideoOS.GatewayService.Utilities;

public interface ISslHelperWrapper
{
	void EnableSsl(string websiteId, string virtualDirectoryName);

	void DisableSsl(string websiteId, string virtualDirectoryName);
}
