using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSActionListHorizontalMedium : VideoOSActionListBase
{
	static VideoOSActionListHorizontalMedium()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSActionListHorizontalMedium), new FrameworkPropertyMetadata(typeof(VideoOSActionListHorizontalMedium)));
		ThemeManager.InitializeThemingSystem();
	}
}
