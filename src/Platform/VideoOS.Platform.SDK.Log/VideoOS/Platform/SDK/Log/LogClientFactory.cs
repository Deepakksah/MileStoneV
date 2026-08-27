using VideoOS.Platform.Common.Logging;
using VideoOS.Platform.Log;

namespace VideoOS.Platform.SDK.Log;

public class LogClientFactory : ILogClientFactory
{
	public VideoOS.Platform.Log.LogClient GetLogByContext(UserContext userContext)
	{
		return new VideoOS.Platform.Common.Logging.LogClient(userContext);
	}
}
