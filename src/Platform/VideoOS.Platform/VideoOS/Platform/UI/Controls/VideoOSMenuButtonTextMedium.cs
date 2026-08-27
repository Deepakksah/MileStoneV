using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSMenuButtonTextMedium : VideoOSMenuButtonBase
{
	public override MenuSizes MenuSize => MenuSizes.Large;

	static VideoOSMenuButtonTextMedium()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSMenuButtonTextMedium), new FrameworkPropertyMetadata(typeof(VideoOSMenuButtonTextMedium)));
		ThemeManager.InitializeThemingSystem();
	}
}
