using System;

namespace VideoOS.Mobile.VideoSource;

public class LiveSourceConfiguration : ILiveSourceConfiguration
{
	public static ILiveSourceConfiguration Instance = new LiveSourceConfiguration();

	public TimeSpan ToolkitConnectTimeout { get; set; }

	private LiveSourceConfiguration()
	{
		ToolkitConnectTimeout = TimeSpan.FromSeconds(5.0);
	}
}
