using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSMenuButtonSecondaryLarge : VideoOSMenuButtonBase
{
	public override MenuSizes MenuSize => MenuSizes.Large;

	static VideoOSMenuButtonSecondaryLarge()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSMenuButtonSecondaryLarge), new FrameworkPropertyMetadata(typeof(VideoOSMenuButtonSecondaryLarge)));
		ThemeManager.InitializeThemingSystem();
	}
}
