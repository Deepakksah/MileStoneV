using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSButtonTransparentMedium : VideoOSButtonTransparentBase
{
	static VideoOSButtonTransparentMedium()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonTransparentMedium), new FrameworkPropertyMetadata(typeof(VideoOSButtonTransparentMedium)));
		ThemeManager.InitializeThemingSystem();
	}
}
