using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSButtonAccentLarge : VideoOSButtonAccentBase
{
	static VideoOSButtonAccentLarge()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonAccentLarge), new FrameworkPropertyMetadata(typeof(VideoOSButtonAccentLarge)));
		ThemeManager.InitializeThemingSystem();
	}
}
