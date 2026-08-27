using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSActionListHorizontalExtraSmall : VideoOSActionListBase
{
	static VideoOSActionListHorizontalExtraSmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSActionListHorizontalExtraSmall), new FrameworkPropertyMetadata(typeof(VideoOSActionListHorizontalExtraSmall)));
		ThemeManager.InitializeThemingSystem();
	}
}
