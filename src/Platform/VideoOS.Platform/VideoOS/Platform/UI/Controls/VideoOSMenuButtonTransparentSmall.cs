using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSMenuButtonTransparentSmall : VideoOSMenuButtonBase
{
	public override MenuSizes MenuSize => MenuSizes.Small;

	static VideoOSMenuButtonTransparentSmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSMenuButtonTransparentSmall), new FrameworkPropertyMetadata(typeof(VideoOSMenuButtonTransparentSmall)));
		ThemeManager.InitializeThemingSystem();
	}
}
