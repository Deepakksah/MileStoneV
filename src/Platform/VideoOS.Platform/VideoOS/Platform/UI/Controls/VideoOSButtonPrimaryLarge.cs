using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSButtonPrimaryLarge : VideoOSButtonPrimaryBase
{
	static VideoOSButtonPrimaryLarge()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonPrimaryLarge), new FrameworkPropertyMetadata(typeof(VideoOSButtonPrimaryLarge)));
		ThemeManager.InitializeThemingSystem();
	}
}
