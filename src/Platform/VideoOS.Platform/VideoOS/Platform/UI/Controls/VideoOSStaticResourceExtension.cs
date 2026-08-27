using System;
using System.Windows.Markup;

namespace VideoOS.Platform.UI.Controls;

public class VideoOSStaticResourceExtension : MarkupExtension
{
	public VideoOSStaticResourceKeys ResourceKey { get; set; }

	public VideoOSStaticResourceExtension()
	{
	}

	public VideoOSStaticResourceExtension(VideoOSStaticResourceKeys resourceKey)
	{
		ResourceKey = resourceKey;
	}

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return ThemeManager.Instance.FindStaticResource(ResourceKey);
	}
}
