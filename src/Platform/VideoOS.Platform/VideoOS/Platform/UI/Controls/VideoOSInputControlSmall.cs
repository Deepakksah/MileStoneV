using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSInputControlSmall : VideoOSInputControlBase
{
	static VideoOSInputControlSmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSInputControlSmall), new FrameworkPropertyMetadata(typeof(VideoOSInputControlSmall)));
		ThemeManager.InitializeThemingSystem();
	}
}
