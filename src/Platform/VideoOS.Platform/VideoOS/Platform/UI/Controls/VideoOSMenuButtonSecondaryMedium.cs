using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSMenuButtonSecondaryMedium : VideoOSMenuButtonBase
{
	public override MenuSizes MenuSize => MenuSizes.Large;

	static VideoOSMenuButtonSecondaryMedium()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSMenuButtonSecondaryMedium), new FrameworkPropertyMetadata(typeof(VideoOSMenuButtonSecondaryMedium)));
		ThemeManager.InitializeThemingSystem();
	}
}
