using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSButtonSecondarySmall : VideoOSButtonSecondaryBase
{
	static VideoOSButtonSecondarySmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonSecondarySmall), new FrameworkPropertyMetadata(typeof(VideoOSButtonSecondarySmall)));
		ThemeManager.InitializeThemingSystem();
	}
}
