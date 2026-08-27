using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSActionListHorizontalSmall : VideoOSActionListBase
{
	static VideoOSActionListHorizontalSmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSActionListHorizontalSmall), new FrameworkPropertyMetadata(typeof(VideoOSActionListHorizontalSmall)));
		ThemeManager.InitializeThemingSystem();
	}
}
