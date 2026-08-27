using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSMenuButtonTertiaryLarge : VideoOSMenuButtonBase
{
	public override MenuSizes MenuSize => MenuSizes.Large;

	static VideoOSMenuButtonTertiaryLarge()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSMenuButtonTertiaryLarge), new FrameworkPropertyMetadata(typeof(VideoOSMenuButtonTertiaryLarge)));
		ThemeManager.InitializeThemingSystem();
	}
}
