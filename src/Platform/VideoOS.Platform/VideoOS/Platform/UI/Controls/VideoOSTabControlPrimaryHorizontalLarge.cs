using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSTabControlPrimaryHorizontalLarge : VideoOSTabControlPrimaryHorizontalBase
{
	static VideoOSTabControlPrimaryHorizontalLarge()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSTabControlPrimaryHorizontalLarge), new FrameworkPropertyMetadata(typeof(VideoOSTabControlPrimaryHorizontalLarge)));
		ThemeManager.InitializeThemingSystem();
	}
}
