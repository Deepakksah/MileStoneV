using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSButtonTertiaryMedium : VideoOSButtonTertiaryBase
{
	static VideoOSButtonTertiaryMedium()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonTertiaryMedium), new FrameworkPropertyMetadata(typeof(VideoOSButtonTertiaryMedium)));
		ThemeManager.InitializeThemingSystem();
	}
}
