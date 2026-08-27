using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSMenuButtonAccentMedium : VideoOSMenuButtonBase
{
	public override MenuSizes MenuSize => MenuSizes.Large;

	static VideoOSMenuButtonAccentMedium()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSMenuButtonAccentMedium), new FrameworkPropertyMetadata(typeof(VideoOSMenuButtonAccentMedium)));
		ThemeManager.InitializeThemingSystem();
	}
}
