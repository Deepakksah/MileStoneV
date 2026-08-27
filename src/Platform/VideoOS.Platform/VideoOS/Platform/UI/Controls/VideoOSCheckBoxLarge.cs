using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSCheckBoxLarge : VideoOSCheckBoxBase
{
	static VideoOSCheckBoxLarge()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSCheckBoxLarge), new FrameworkPropertyMetadata(typeof(VideoOSCheckBoxLarge)));
		ThemeManager.InitializeThemingSystem();
	}
}
