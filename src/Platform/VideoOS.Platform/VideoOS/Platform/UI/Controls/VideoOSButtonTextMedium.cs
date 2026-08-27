using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSButtonTextMedium : VideoOSButtonTextBase
{
	static VideoOSButtonTextMedium()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonTextMedium), new FrameworkPropertyMetadata(typeof(VideoOSButtonTextMedium)));
		ThemeManager.InitializeThemingSystem();
	}
}
