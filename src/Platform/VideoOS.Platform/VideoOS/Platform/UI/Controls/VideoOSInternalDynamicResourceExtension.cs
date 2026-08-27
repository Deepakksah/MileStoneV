using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public class VideoOSInternalDynamicResourceExtension : DynamicResourceExtension
{
	public VideoOSInternalDynamicResourceExtension(VideoOSInternalResourceKeys resourceKey)
	{
		base.ResourceKey = resourceKey;
	}
}
