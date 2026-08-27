using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSButtonHyperlinkMedium : VideoOSButtonHyperlinkBase
{
	static VideoOSButtonHyperlinkMedium()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonHyperlinkMedium), new FrameworkPropertyMetadata(typeof(VideoOSButtonHyperlinkMedium)));
		ThemeManager.InitializeThemingSystem();
	}
}
