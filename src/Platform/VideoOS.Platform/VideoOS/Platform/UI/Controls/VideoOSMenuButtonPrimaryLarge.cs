using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSMenuButtonPrimaryLarge : VideoOSMenuButtonBase
{
	public override MenuSizes MenuSize => MenuSizes.Large;

	static VideoOSMenuButtonPrimaryLarge()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSMenuButtonPrimaryLarge), new FrameworkPropertyMetadata(typeof(VideoOSMenuButtonPrimaryLarge)));
		ThemeManager.InitializeThemingSystem();
	}
}
