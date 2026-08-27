using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSMenuButtonAccentLarge : VideoOSMenuButtonBase
{
	public override MenuSizes MenuSize => MenuSizes.Large;

	static VideoOSMenuButtonAccentLarge()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSMenuButtonAccentLarge), new FrameworkPropertyMetadata(typeof(VideoOSMenuButtonAccentLarge)));
		ThemeManager.InitializeThemingSystem();
	}
}
