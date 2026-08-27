using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSMenuButtonTertiaryMedium : VideoOSMenuButtonBase
{
	public override MenuSizes MenuSize => MenuSizes.Large;

	static VideoOSMenuButtonTertiaryMedium()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSMenuButtonTertiaryMedium), new FrameworkPropertyMetadata(typeof(VideoOSMenuButtonTertiaryMedium)));
		ThemeManager.InitializeThemingSystem();
	}
}
