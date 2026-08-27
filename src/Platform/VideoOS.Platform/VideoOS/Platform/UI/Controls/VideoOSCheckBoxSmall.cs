using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSCheckBoxSmall : VideoOSCheckBoxBase
{
	static VideoOSCheckBoxSmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSCheckBoxSmall), new FrameworkPropertyMetadata(typeof(VideoOSCheckBoxSmall)));
		ThemeManager.InitializeThemingSystem();
	}
}
