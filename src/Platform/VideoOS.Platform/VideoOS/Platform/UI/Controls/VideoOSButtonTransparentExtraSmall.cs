using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSButtonTransparentExtraSmall : VideoOSButtonTransparentBase
{
	static VideoOSButtonTransparentExtraSmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonTransparentExtraSmall), new FrameworkPropertyMetadata(typeof(VideoOSButtonTransparentExtraSmall)));
		ThemeManager.InitializeThemingSystem();
	}
}
