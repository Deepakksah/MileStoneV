using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSButtonAccentMedium : VideoOSButtonAccentBase
{
	static VideoOSButtonAccentMedium()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonAccentMedium), new FrameworkPropertyMetadata(typeof(VideoOSButtonAccentMedium)));
		ThemeManager.InitializeThemingSystem();
	}
}
