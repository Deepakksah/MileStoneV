using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSButtonHyperlinkBase : VideoOSButtonBase
{
	static VideoOSButtonHyperlinkBase()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonHyperlinkBase), new FrameworkPropertyMetadata(typeof(VideoOSButtonHyperlinkBase)));
		ThemeManager.InitializeThemingSystem();
	}

	public VideoOSButtonHyperlinkBase()
	{
	}
}
