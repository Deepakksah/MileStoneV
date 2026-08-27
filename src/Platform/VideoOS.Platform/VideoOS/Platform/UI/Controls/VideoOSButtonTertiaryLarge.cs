using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSButtonTertiaryLarge : VideoOSButtonTertiaryBase
{
	static VideoOSButtonTertiaryLarge()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonTertiaryLarge), new FrameworkPropertyMetadata(typeof(VideoOSButtonTertiaryLarge)));
		ThemeManager.InitializeThemingSystem();
	}
}
