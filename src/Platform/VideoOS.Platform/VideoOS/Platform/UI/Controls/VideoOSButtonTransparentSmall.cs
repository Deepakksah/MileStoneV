using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSButtonTransparentSmall : VideoOSButtonTransparentBase
{
	static VideoOSButtonTransparentSmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonTransparentSmall), new FrameworkPropertyMetadata(typeof(VideoOSButtonTransparentSmall)));
		ThemeManager.InitializeThemingSystem();
	}
}
