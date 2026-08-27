using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSToggleButtonIconExtraSmall : VideoOSToggleButtonIconBase
{
	static VideoOSToggleButtonIconExtraSmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSToggleButtonIconExtraSmall), new FrameworkPropertyMetadata(typeof(VideoOSToggleButtonIconExtraSmall)));
		ThemeManager.InitializeThemingSystem();
	}
}
