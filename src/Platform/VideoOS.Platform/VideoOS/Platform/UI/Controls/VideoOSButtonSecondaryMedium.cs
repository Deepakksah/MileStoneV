using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSButtonSecondaryMedium : VideoOSButtonSecondaryBase
{
	static VideoOSButtonSecondaryMedium()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonSecondaryMedium), new FrameworkPropertyMetadata(typeof(VideoOSButtonSecondaryMedium)));
		ThemeManager.InitializeThemingSystem();
	}
}
