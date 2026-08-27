using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSButtonAccentExtraSmall : VideoOSButtonAccentBase
{
	static VideoOSButtonAccentExtraSmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonAccentExtraSmall), new FrameworkPropertyMetadata(typeof(VideoOSButtonAccentExtraSmall)));
		ThemeManager.InitializeThemingSystem();
	}
}
