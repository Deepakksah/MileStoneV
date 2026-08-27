using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSRadioButtonSmall : VideoOSRadioButtonBase
{
	static VideoOSRadioButtonSmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSRadioButtonSmall), new FrameworkPropertyMetadata(typeof(VideoOSRadioButtonSmall)));
		ThemeManager.InitializeThemingSystem();
	}
}
