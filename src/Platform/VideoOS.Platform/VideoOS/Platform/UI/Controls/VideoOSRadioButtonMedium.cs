using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSRadioButtonMedium : VideoOSRadioButtonBase
{
	static VideoOSRadioButtonMedium()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSRadioButtonMedium), new FrameworkPropertyMetadata(typeof(VideoOSRadioButtonMedium)));
		ThemeManager.InitializeThemingSystem();
	}
}
