using VideoOS.Platform.Util;

namespace VideoOS.Platform.SDK.Export;

public class Environment
{
	public static void Initialize()
	{
		if (!InternalCommandService.Instance.Initialized)
		{
			InternalCommandService.Instance = new SDKInternalCommandService();
		}
	}
}
