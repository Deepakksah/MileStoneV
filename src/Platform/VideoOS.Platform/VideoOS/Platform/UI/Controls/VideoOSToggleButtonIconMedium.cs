using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSToggleButtonIconMedium : VideoOSToggleButtonIconBase
{
	static VideoOSToggleButtonIconMedium()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSToggleButtonIconMedium), new FrameworkPropertyMetadata(typeof(VideoOSToggleButtonIconMedium)));
		ThemeManager.InitializeThemingSystem();
	}
}
