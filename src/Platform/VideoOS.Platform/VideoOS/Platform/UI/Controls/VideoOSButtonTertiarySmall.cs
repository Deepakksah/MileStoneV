using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSButtonTertiarySmall : VideoOSButtonTertiaryBase
{
	static VideoOSButtonTertiarySmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonTertiarySmall), new FrameworkPropertyMetadata(typeof(VideoOSButtonTertiarySmall)));
		ThemeManager.InitializeThemingSystem();
	}
}
