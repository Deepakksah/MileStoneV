using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSMenuButtonPrimaryMedium : VideoOSMenuButtonBase
{
	public override MenuSizes MenuSize => MenuSizes.Large;

	static VideoOSMenuButtonPrimaryMedium()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSMenuButtonPrimaryMedium), new FrameworkPropertyMetadata(typeof(VideoOSMenuButtonPrimaryMedium)));
		ThemeManager.InitializeThemingSystem();
	}
}
