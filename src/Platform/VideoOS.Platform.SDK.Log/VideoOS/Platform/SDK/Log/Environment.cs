using VideoOS.Platform.Common.Logging;
using VideoOS.Platform.Log;

namespace VideoOS.Platform.SDK.Log;

public static class Environment
{
	public static void Initialize()
	{
		if (!VideoOS.Platform.Log.LogClient.Instance.Initialized)
		{
			VideoOS.Platform.Log.LogClient.Instance = new VideoOS.Platform.Common.Logging.LogClient();
		}
		if (MultiUserEnvironment.LogClientFactory == null)
		{
			MultiUserEnvironment.LogClientFactory = new LogClientFactory();
		}
		if (MultiEnvironment.LogClientFactory == null)
		{
			MultiEnvironment.LogClientFactory = new LogClientFactory();
		}
	}
}
