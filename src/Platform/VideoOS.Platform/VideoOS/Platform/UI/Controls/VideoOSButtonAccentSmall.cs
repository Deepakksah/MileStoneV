using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSButtonAccentSmall : VideoOSButtonAccentBase
{
	static VideoOSButtonAccentSmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonAccentSmall), new FrameworkPropertyMetadata(typeof(VideoOSButtonAccentSmall)));
		ThemeManager.InitializeThemingSystem();
	}
}
