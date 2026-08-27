using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSCheckBoxMedium : VideoOSCheckBoxBase
{
	static VideoOSCheckBoxMedium()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSCheckBoxMedium), new FrameworkPropertyMetadata(typeof(VideoOSCheckBoxMedium)));
		ThemeManager.InitializeThemingSystem();
	}
}
