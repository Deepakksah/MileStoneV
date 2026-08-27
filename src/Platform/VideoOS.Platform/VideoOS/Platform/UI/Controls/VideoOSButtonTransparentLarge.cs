using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSButtonTransparentLarge : VideoOSButtonTransparentBase
{
	static VideoOSButtonTransparentLarge()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonTransparentLarge), new FrameworkPropertyMetadata(typeof(VideoOSButtonTransparentLarge)));
		ThemeManager.InitializeThemingSystem();
	}
}
