using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSToggleButtonIconSmall : VideoOSToggleButtonIconBase
{
	static VideoOSToggleButtonIconSmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSToggleButtonIconSmall), new FrameworkPropertyMetadata(typeof(VideoOSToggleButtonIconSmall)));
		ThemeManager.InitializeThemingSystem();
	}
}
