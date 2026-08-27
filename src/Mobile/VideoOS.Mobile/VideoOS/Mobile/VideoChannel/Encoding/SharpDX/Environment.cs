using SharpDX.MediaFoundation;

namespace VideoOS.Mobile.VideoChannel.Encoding.SharpDX;

public static class Environment
{
	public static void Initialize()
	{
		MediaFactory.Startup(131184, 0);
	}

	public static void Cleanup()
	{
		MediaFactory.Shutdown();
	}
}
