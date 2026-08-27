using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSButtonHyperlinkSmall : VideoOSButtonHyperlinkBase
{
	static VideoOSButtonHyperlinkSmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonHyperlinkSmall), new FrameworkPropertyMetadata(typeof(VideoOSButtonHyperlinkSmall)));
		ThemeManager.InitializeThemingSystem();
	}
}
