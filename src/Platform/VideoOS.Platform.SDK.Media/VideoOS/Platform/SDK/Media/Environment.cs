using VideoOS.Platform.Util;

namespace VideoOS.Platform.SDK.Media;

public class Environment
{
	public static void Initialize()
	{
		if (!InternalMediaService.Instance.Initialized)
		{
			InternalMediaService.Instance = new SDKInternalMediaService();
		}
	}
}
