using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSButtonPrimaryMedium : VideoOSButtonPrimaryBase
{
	static VideoOSButtonPrimaryMedium()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonPrimaryMedium), new FrameworkPropertyMetadata(typeof(VideoOSButtonPrimaryMedium)));
		ThemeManager.InitializeThemingSystem();
	}
}
