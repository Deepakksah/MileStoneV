using Microsoft.Extensions.Logging;

namespace VideoOS.GatewayService.Utilities;

public interface IAppPoolHelperWrapperFactory
{
	IAppPoolHelperWrapper CreateAppPoolHelperWrapper(ILogger log);
}
