using Microsoft.Extensions.Logging;

namespace VideoOS.GatewayService.Utilities;

public class AppPoolHelperWrapperFactory : IAppPoolHelperWrapperFactory
{
	public IAppPoolHelperWrapper CreateAppPoolHelperWrapper(ILogger log)
	{
		return new AppPoolHelperWrapper(log);
	}
}
