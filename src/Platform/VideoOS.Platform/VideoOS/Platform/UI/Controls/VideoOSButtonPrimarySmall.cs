using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSButtonPrimarySmall : VideoOSButtonPrimaryBase
{
	static VideoOSButtonPrimarySmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonPrimarySmall), new FrameworkPropertyMetadata(typeof(VideoOSButtonPrimarySmall)));
		ThemeManager.InitializeThemingSystem();
	}
}
