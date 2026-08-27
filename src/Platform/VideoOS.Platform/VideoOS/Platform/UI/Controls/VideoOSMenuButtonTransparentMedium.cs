using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSMenuButtonTransparentMedium : VideoOSMenuButtonBase
{
	public override MenuSizes MenuSize => MenuSizes.Large;

	static VideoOSMenuButtonTransparentMedium()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSMenuButtonTransparentMedium), new FrameworkPropertyMetadata(typeof(VideoOSMenuButtonTransparentMedium)));
		ThemeManager.InitializeThemingSystem();
	}
}
