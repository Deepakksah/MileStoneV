using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSMenuButtonTransparentLarge : VideoOSMenuButtonBase
{
	public override MenuSizes MenuSize => MenuSizes.Large;

	static VideoOSMenuButtonTransparentLarge()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSMenuButtonTransparentLarge), new FrameworkPropertyMetadata(typeof(VideoOSMenuButtonTransparentLarge)));
		ThemeManager.InitializeThemingSystem();
	}
}
