using System;
using System.Windows.Markup;

namespace VideoOS.Platform.UI.Controls;

public class VideoOSIconBuiltInSourceExtension : MarkupExtension
{
	[ConstructorArgument("icon")]
	public VideoOSIconBuiltInSource.Icons Icon { get; set; }

	public VideoOSIconBuiltInSource.Icons RtlIcon { get; set; } = VideoOSIconBuiltInSource.Icons.None;

	public bool FollowFlowDirection { get; set; }

	public VideoOSIconBuiltInSource.Appearances Appearance { get; set; }

	public VideoOSIconBuiltInSourceExtension(VideoOSIconBuiltInSource.Icons icon)
	{
		Icon = icon;
	}

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return new VideoOSIconBuiltInSource
		{
			Icon = Icon,
			RtlIcon = RtlIcon,
			Appearance = Appearance,
			FollowFlowDirection = FollowFlowDirection
		};
	}
}
