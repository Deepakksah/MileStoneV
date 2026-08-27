using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSButtonTextSmall : VideoOSButtonTextBase
{
	static VideoOSButtonTextSmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonTextSmall), new FrameworkPropertyMetadata(typeof(VideoOSButtonTextSmall)));
		ThemeManager.InitializeThemingSystem();
	}
}
