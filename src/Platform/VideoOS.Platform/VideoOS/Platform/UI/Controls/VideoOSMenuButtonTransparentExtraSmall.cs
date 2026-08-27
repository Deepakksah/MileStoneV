using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSMenuButtonTransparentExtraSmall : VideoOSMenuButtonBase
{
	public override MenuSizes MenuSize => MenuSizes.Small;

	static VideoOSMenuButtonTransparentExtraSmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSMenuButtonTransparentExtraSmall), new FrameworkPropertyMetadata(typeof(VideoOSMenuButtonTransparentExtraSmall)));
		ThemeManager.InitializeThemingSystem();
	}
}
