using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSInputControlMedium : VideoOSInputControlBase
{
	static VideoOSInputControlMedium()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSInputControlMedium), new FrameworkPropertyMetadata(typeof(VideoOSInputControlMedium)));
		ThemeManager.InitializeThemingSystem();
	}
}
