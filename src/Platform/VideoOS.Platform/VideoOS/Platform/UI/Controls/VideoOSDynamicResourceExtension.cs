using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public class VideoOSDynamicResourceExtension : DynamicResourceExtension
{
	public VideoOSDynamicResourceExtension(VideoOSResourceKeys resourceKey)
	{
		base.ResourceKey = resourceKey;
	}
}
