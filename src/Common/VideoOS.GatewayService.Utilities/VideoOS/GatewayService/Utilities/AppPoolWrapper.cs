using Microsoft.Web.Administration;

namespace VideoOS.GatewayService.Utilities;

public class AppPoolWrapper
{
	public ApplicationPool AppPool { get; }

	public AppPoolWrapper()
	{
		AppPool = null;
	}

	public AppPoolWrapper(ApplicationPool appPool)
	{
		AppPool = appPool;
	}
}
