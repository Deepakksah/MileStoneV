using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSButtonSecondaryLarge : VideoOSButtonSecondaryBase
{
	static VideoOSButtonSecondaryLarge()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonSecondaryLarge), new FrameworkPropertyMetadata(typeof(VideoOSButtonSecondaryLarge)));
		ThemeManager.InitializeThemingSystem();
	}
}
